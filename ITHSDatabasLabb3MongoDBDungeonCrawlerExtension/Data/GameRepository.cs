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

        var indexKeys = Builders<SaveGameDocument>.IndexKeys.Descending(x => x.LastPlayedUtc);
        _saves.Indexes.CreateOne(new CreateIndexModel<SaveGameDocument>(indexKeys));
    }

    public List<SaveGameDocument> GetAllSaves()
        => _saves.Find(FilterDefinition<SaveGameDocument>.Empty)
                 .SortByDescending(x => x.LastPlayedUtc)
                 .ToList();

    public SaveGameDocument? GetSave(ObjectId id)
        => _saves.Find(x => x.Id == id).FirstOrDefault();

    public List<ArchetypeDocument> GetAllArchetypes()
        => _archetypes.Find(FilterDefinition<ArchetypeDocument>.Empty)
                      .SortBy(x => x.Name)
                      .ToList();

    public SaveGameDocument CreateNewSave(string playerName, ObjectId archetypeId, string levelPath, int[] startPos)
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

        _saves.InsertOne(doc);
        return doc;
    }


    public void TouchLastPlayed(ObjectId saveId)
    {
        var update = Builders<SaveGameDocument>.Update.Set(x => x.LastPlayedUtc, DateTime.UtcNow);
        _saves.UpdateOne(x => x.Id == saveId, update);
    }

    public void UpdateSaveFull(ObjectId saveId,
                           SaveGameDocument originalSave,
                           LevelData levelData,
                           Player player,
                           MessageLog messageLog,
                           int turnCount,
                           int initialEnemyCount)
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
            .Set(x => x.LastPlayedUtc, DateTime.UtcNow)
            .Set(x => x.TurnCount, turnCount)
            .Set(x => x.InitialEnemyCount, initialEnemyCount)
            .Set(x => x.LastPlayedUtc, DateTime.UtcNow);

        _saves.UpdateOne(x => x.Id == saveId, update);
    }
    public void MarkDead(ObjectId saveId)
    {
        var update = Builders<SaveGameDocument>.Update
            .Set(x => x.IsDead, true)
            .Set(x => x.DiedUtc, DateTime.UtcNow)
            .Set(x => x.LastPlayedUtc, DateTime.UtcNow);

        _saves.UpdateOne(x => x.Id == saveId, update);
    }

}
