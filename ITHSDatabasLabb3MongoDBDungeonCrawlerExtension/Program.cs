using System.Text;
using Microsoft.Extensions.Configuration;

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

        var db = await MongoDbSetup.EnsureDatabaseAndSeedAsync(mongoConnString);

        var repo = new GameRepository(db);
        await repo.InitializeAsync();

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
