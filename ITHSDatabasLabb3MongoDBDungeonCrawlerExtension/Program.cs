using System.Text;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

using ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.Core;
using ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.Data;
using ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.UI;

namespace ITHSDatabasLabb3MongoDBDungeonCrawlerExtension;

internal class Program
{
    static async Task Main()
    {
        var config = new ConfigurationBuilder()
            .AddUserSecrets<Program>()
            .AddEnvironmentVariables()
            .Build();

        var mongoConnString =
            config.GetConnectionString("MongoDb")
            ?? "mongodb://localhost:27017";

        IMongoDatabase db;
        GameRepository repo;

        try
        {
            db = await MongoDbSetup.EnsureDatabaseAndSeedAsync(mongoConnString);
            repo = new GameRepository(db);
            await repo.InitializeAsync();
        }
        catch (MongoConfigurationException ex)
        {
            Console.WriteLine("MongoDB configuration error.");
            Console.WriteLine(ex.Message);
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey(true);
            return;
        }
        catch (MongoConnectionException ex)
        {
            Console.WriteLine("Could not connect to MongoDB.");
            Console.WriteLine("Is MongoDB installed and running?");
            Console.WriteLine($"Connection string: {mongoConnString}");
            Console.WriteLine(ex.Message);
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey(true);
            return;
        }
        catch (TimeoutException ex)
        {
            Console.WriteLine("MongoDB connection timed out.");
            Console.WriteLine("Is the server running at the configured address?");
            Console.WriteLine($"Connection string: {mongoConnString}");
            Console.WriteLine(ex.Message);
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey(true);
            return;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Unexpected error during DB startup:");
            Console.WriteLine(ex);
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey(true);
            return;
        }

        Console.CursorVisible = false;
        Console.OutputEncoding = Encoding.UTF8;

        StartUpScreen.Draw();

        var activeSave = await CharacterSelectMenu.RunAsync(repo);

        LevelData levelData = new();
        levelData.LoadFromSave(activeSave);

        var startPos = new[] { activeSave.Player.Row, activeSave.Player.Col };
        Player player = new(startPos, activeSave.PlayerName);

        player.HitPoints.HP = activeSave.Player.Hp;
        player.AttackDice.Modifier = activeSave.Player.AttackModifier;
        player.DefenceDice.Modifier = activeSave.Player.DefenceModifier;

        MessageLog messageLog = new(levelData.LevelHeight, levelData.LevelWidth);
        if (activeSave.Messages is not null && activeSave.Messages.Count > 0)
        {
            messageLog.LoadMessages(activeSave.Messages, keepLast: 200);
        }

        Sidebar sidebar = new(levelData.LevelHeight, levelData.LevelWidth, player, levelData, activeSave.InitialEnemyCount);

        int startTurnCount = activeSave.TurnCount;
        int initialEnemyCount = activeSave.InitialEnemyCount;

        GameLoop gameLoop = new(
            levelData,
            player,
            messageLog,
            sidebar,
            repo,
            activeSave,
            startTurnCount,
            initialEnemyCount
        );

        if (OperatingSystem.IsWindows())
        {
            Console.BufferWidth += levelData.LevelWidth + sidebar.Width;
        }

        await gameLoop.StartLoopAsync();
    }
}
