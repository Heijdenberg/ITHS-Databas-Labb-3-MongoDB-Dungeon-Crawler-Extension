using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.Data;

internal sealed class ArchetypeDocument
{
    [BsonId]
    public ObjectId Id { get; set; }

    [BsonElement("name")]
    public string Name { get; set; } = "";

    [BsonElement("description")]
    public string? Description { get; set; }
}
