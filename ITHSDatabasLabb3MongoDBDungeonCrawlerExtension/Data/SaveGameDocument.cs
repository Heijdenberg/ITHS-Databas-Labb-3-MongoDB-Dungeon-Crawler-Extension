using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.Data;

internal sealed class SaveGameDocument
{
    [BsonId]
    public ObjectId Id { get; set; }

    [BsonElement("playerName")]
    public string PlayerName { get; set; } = "";

    [BsonElement("archetypeId")]
    public ObjectId ArchetypeId { get; set; }

    [BsonElement("levelPath")]
    public string LevelPath { get; set; } = "";

    [BsonElement("levelHeight")]
    public int LevelHeight { get; set; }

    [BsonElement("levelWidth")]
    public int LevelWidth { get; set; }

    [BsonElement("player")]
    public PlayerStateDocument Player { get; set; } = new();

    [BsonElement("elements")]
    public List<ElementStateDocument> Elements { get; set; } = new();

    [BsonElement("messages")]
    public List<string> Messages { get; set; } = new();

    [BsonElement("createdUtc")]
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;

    [BsonElement("lastPlayedUtc")]
    public DateTime LastPlayedUtc { get; set; } = DateTime.UtcNow;

    [BsonElement("turnCount")]
    public int TurnCount { get; set; }

    [BsonElement("initialEnemyCount")]
    public int InitialEnemyCount { get; set; }
}

internal sealed class PlayerStateDocument
{
    [BsonElement("row")]
    public int Row { get; set; }

    [BsonElement("col")]
    public int Col { get; set; }

    [BsonElement("hp")]
    public int Hp { get; set; }

    [BsonElement("attackModifier")]
    public int AttackModifier { get; set; }

    [BsonElement("defenceModifier")]
    public int DefenceModifier { get; set; }

    [BsonElement("visionRange")]
    public int VisionRange { get; set; }
}

internal sealed class ElementStateDocument
{
    [BsonElement("type")]
    public string Type { get; set; } = "";

    [BsonElement("row")]
    public int Row { get; set; }

    [BsonElement("col")]
    public int Col { get; set; }

    [BsonElement("hp")]
    public int? Hp { get; set; }

    [BsonElement("name")]
    public string? Name { get; set; }

    [BsonElement("isDiscovered")]
    public bool? IsDiscovered { get; set; }
}
