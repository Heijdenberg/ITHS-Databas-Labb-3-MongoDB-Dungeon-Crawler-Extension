using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.Data;

internal static class MongoDbSetup
{
    public const string DatabaseName = "JensHeijdenberg";
    public const string ArchetypesCollectionName = "archetypes";

    public static IMongoDatabase EnsureDatabaseAndSeed(string connectionString)
    {
        var client = new MongoClient(connectionString);

        var db = client.GetDatabase(DatabaseName);

        var collectionNames = db.ListCollectionNames().ToList();
        if (!collectionNames.Contains(ArchetypesCollectionName))
        {
            db.CreateCollection(ArchetypesCollectionName);
        }

        var archetypes = db.GetCollection<ArchetypeDocument>(ArchetypesCollectionName);
        var count = archetypes.CountDocuments(FilterDefinition<ArchetypeDocument>.Empty);

        if (count == 0)
        {
            archetypes.InsertMany(new[]
            {
                new ArchetypeDocument { Name = "Warrior", Description = "Tough melee fighter." },
                new ArchetypeDocument { Name = "Rogue",   Description = "Fast and sneaky." },
                new ArchetypeDocument { Name = "Mage",    Description = "Spellcaster with high damage." },
                new ArchetypeDocument { Name = "Priest",  Description = "Support healer and buffer." },
            });
        }

        return db;
    }
}
