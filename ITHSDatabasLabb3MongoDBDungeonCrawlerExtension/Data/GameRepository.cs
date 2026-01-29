using ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.Core;
using ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.Elements;
using ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.UI;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.Data;

internal sealed class GameRepository
{
    private const string SavesCollectionName = "saves";
    private const string ArchetypesCollectionName = MongoDbSetup.ArchetypesCollectionName;

    private readonly IMongoCollection<SaveGameDocument> _saves;
    private readonly IMongoCollection<ArchetypeDocument> _archetypes;

    public GameRepository(IMongoDatabase db)
    {
        _saves = db.GetCollection<SaveGameDocument>(SavesCollectionName);
        _archetypes = db.GetCollection<ArchetypeDocument>(ArchetypesCollectionName);
    }

    public async Task InitializeAsync(CancellationToken ct = default)
    {
        var indexKeys = Builders<SaveGameDocument>.IndexKeys.Descending(x => x.LastPlayedUtc);
        await _saves.Indexes.CreateOneAsync(
            new CreateIndexModel<SaveGameDocument>(indexKeys),
            cancellationToken: ct
        );
    }

    public async Task<List<SaveGameDocument>> GetAllSavesAsync(CancellationToken ct = default)
        => await _saves.Find(FilterDefinition<SaveGameDocument>.Empty)
                       .SortByDescending(x => x.LastPlayedUtc)
                       .ToListAsync(ct);

    public async Task<SaveGameDocument?> GetSaveAsync(ObjectId id, CancellationToken ct = default)
        => await _saves.Find(x => x.Id == id)
                       .FirstOrDefaultAsync(ct);

    public async Task<List<ArchetypeDocument>> GetAllArchetypesAsync(CancellationToken ct = default)
        => await _archetypes.Find(FilterDefinition<ArchetypeDocument>.Empty)
                            .SortBy(x => x.Name)
                            .ToListAsync(ct);

    public async Task<SaveGameDocument> CreateNewSaveAsync(
        string playerName,
        ObjectId archetypeId,
        string levelPath,
        int[] startPos,
        CancellationToken ct = default)
    {
        var doc = new SaveGameDocument
        {
            PlayerName = playerName,
            ArchetypeId = archetypeId,
            LevelPath = levelPath,

            Player = new PlayerStateDocument
            {
                Row = startPos[0],
                Col = startPos[1],
                Hp = 100,
                AttackModifier = 2,
                DefenceModifier = 0,
                VisionRange = 5
            },

            CreatedUtc = DateTime.UtcNow,
            LastPlayedUtc = DateTime.UtcNow
        };

        await _saves.InsertOneAsync(doc, cancellationToken: ct);
        return doc;
    }

    public async Task TouchLastPlayedAsync(ObjectId saveId, CancellationToken ct = default)
    {
        var update = Builders<SaveGameDocument>.Update.Set(x => x.LastPlayedUtc, DateTime.UtcNow);
        await _saves.UpdateOneAsync(x => x.Id == saveId, update, cancellationToken: ct);
    }

    public async Task UpdateSaveFullAsync(
        ObjectId saveId,
        SaveGameDocument originalSave,
        LevelData levelData,
        Player player,
        MessageLog messageLog,
        int turnCount,
        int initialEnemyCount,
        CancellationToken ct = default)
    {
        var playerState = new PlayerStateDocument
        {
            Row = player.Position.Row,
            Col = player.Position.Col,
            Hp = player.HitPoints.HP,
            AttackModifier = player.AttackDice.Modifier,
            DefenceModifier = player.DefenceDice.Modifier,
            VisionRange = player.VisionRange
        };

        var elementStates = new List<ElementStateDocument>(levelData.Elements.Count);
        foreach (var el in levelData.Elements)
        {
            switch (el)
            {
                case Wall w:
                    elementStates.Add(new ElementStateDocument
                    {
                        Type = "Wall",
                        Row = w.Position.Row,
                        Col = w.Position.Col,
                        IsDiscovered = w.IsDiscovered
                    });
                    break;

                case Rat r:
                    elementStates.Add(new ElementStateDocument
                    {
                        Type = "Rat",
                        Row = r.Position.Row,
                        Col = r.Position.Col,
                        Hp = r.HitPoints.HP,
                        Name = r.Name
                    });
                    break;

                case Snake s:
                    elementStates.Add(new ElementStateDocument
                    {
                        Type = "Snake",
                        Row = s.Position.Row,
                        Col = s.Position.Col,
                        Hp = s.HitPoints.HP,
                        Name = s.Name
                    });
                    break;
            }
        }

        var messages = messageLog.Messages.ToList();

        var update = Builders<SaveGameDocument>.Update
            .Set(x => x.LevelPath, originalSave.LevelPath)
            .Set(x => x.LevelHeight, levelData.LevelHeight)
            .Set(x => x.LevelWidth, levelData.LevelWidth)
            .Set(x => x.PlayerName, player.Name)
            .Set(x => x.Player, playerState)
            .Set(x => x.Elements, elementStates)
            .Set(x => x.Messages, messages)
            .Set(x => x.TurnCount, turnCount)
            .Set(x => x.InitialEnemyCount, initialEnemyCount)
            .Set(x => x.LastPlayedUtc, DateTime.UtcNow);

        await _saves.UpdateOneAsync(x => x.Id == saveId, update, cancellationToken: ct);
    }

    public async Task MarkDeadAsync(ObjectId saveId, CancellationToken ct = default)
    {
        var update = Builders<SaveGameDocument>.Update
            .Set(x => x.IsDead, true)
            .Set(x => x.DiedUtc, DateTime.UtcNow)
            .Set(x => x.LastPlayedUtc, DateTime.UtcNow);

        await _saves.UpdateOneAsync(x => x.Id == saveId, update, cancellationToken: ct);
    }
}
