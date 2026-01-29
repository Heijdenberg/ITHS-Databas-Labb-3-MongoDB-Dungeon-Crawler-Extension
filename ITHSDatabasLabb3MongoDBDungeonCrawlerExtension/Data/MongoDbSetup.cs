using MongoDB.Driver;

namespace ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.Data;

internal static class MongoDbSetup
{
    public const string DatabaseName = "JensHeijdenberg";
    public const string ArchetypesCollectionName = "archetypes";

    public static async Task<IMongoDatabase> EnsureDatabaseAndSeedAsync(string connectionString, CancellationToken ct = default)
    {
        var client = new MongoClient(connectionString);
        var db = client.GetDatabase(DatabaseName);

        var collectionNamesCursor = await db.ListCollectionNamesAsync(cancellationToken: ct);
        var collectionNames = await collectionNamesCursor.ToListAsync(ct);

        if (!collectionNames.Contains(ArchetypesCollectionName))
            await db.CreateCollectionAsync(ArchetypesCollectionName, cancellationToken: ct);

        var archetypes = db.GetCollection<ArchetypeDocument>(ArchetypesCollectionName);

        var count = await archetypes.CountDocumentsAsync(FilterDefinition<ArchetypeDocument>.Empty, cancellationToken: ct);

        if (count == 0)
        {
            await archetypes.InsertManyAsync(new[]
            {
                new ArchetypeDocument { Name = "Warrior", Description = "Tough melee fighter." },
                new ArchetypeDocument { Name = "Rogue",   Description = "Fast and sneaky." },
                new ArchetypeDocument { Name = "Mage",    Description = "Spellcaster with high damage." },
                new ArchetypeDocument { Name = "Priest",  Description = "Support healer and buffer." },
            }, cancellationToken: ct);
        }

        return db;
    }
}
