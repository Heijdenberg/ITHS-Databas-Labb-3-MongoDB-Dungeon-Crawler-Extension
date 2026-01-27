using MongoDB.Bson;
using ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.Data;
using ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.Core;

namespace ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.UI;

internal static class CharacterSelectMenu
{
    public static SaveGameDocument Run(GameRepository repo)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== CHARACTER SELECT ===\n");

            var saves = repo.GetAllSaves();

            if (saves.Count == 0)
            {
                Console.WriteLine("No characters found.");
            }
            else
            {
                for (int i = 0; i < saves.Count; i++)
                {
                    var s = saves[i];
                    Console.WriteLine($"{i + 1}. {s.PlayerName}  (Last played: {s.LastPlayedUtc:yyyy-MM-dd HH:mm})");
                }
            }

            Console.WriteLine("\nN = New character");
            Console.WriteLine("ESC = Exit");
            Console.Write("\nChoose: ");

            var key = Console.ReadKey(intercept: true);

            if (key.Key == ConsoleKey.Escape)
                Environment.Exit(0);

            if (key.Key == ConsoleKey.N)
                return CreateNewCharacterFlow(repo);

            if (char.IsDigit(key.KeyChar))
            {
                int chosen = (int)char.GetNumericValue(key.KeyChar);
                int idx = chosen - 1;

                if (idx >= 0 && idx < saves.Count)
                {
                    repo.TouchLastPlayed(saves[idx].Id);
                    return repo.GetSave(saves[idx].Id) ?? saves[idx];
                }
            }
        }
    }

    private static SaveGameDocument CreateNewCharacterFlow(GameRepository repo)
    {
        Console.Clear();
        Console.WriteLine("=== NEW CHARACTER ===\n");

        Console.Write("Enter character name: ");
        string name = (Console.ReadLine() ?? "").Trim();
        if (string.IsNullOrWhiteSpace(name))
            name = "Player";

        var archetypes = repo.GetAllArchetypes();
        if (archetypes.Count == 0)
        {
            Console.WriteLine("\nNo archetypes found in DB (seed failed?). Press any key...");
            Console.ReadKey(true);
            return Run(repo);
        }

        Console.WriteLine("\nChoose archetype:");
        for (int i = 0; i < archetypes.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {archetypes[i].Name}");
        }

        int archIndex = ReadChoice(1, archetypes.Count) - 1;
        ObjectId archetypeId = archetypes[archIndex].Id;

        Console.Clear();
        Console.WriteLine("=== SELECT LEVEL ===\n");
        string levelPath = LevelSelect.GetFilePath();

        LevelData levelData = new();
        int[] startPosition = levelData.Load(levelPath);
        levelData.SolidWalls();

        int initialEnemyCount = levelData.GetEnemyCount();

        var save = repo.CreateNewSave(name, archetypeId, levelPath, startPosition);

        var tempPlayer = new Player(startPosition, name);
        var tempLog = new MessageLog(levelData.LevelHeight, levelData.LevelWidth);

        repo.UpdateSaveFull(
            save.Id,
            save,
            levelData,
            tempPlayer,
            tempLog,
            turnCount: 0,
            initialEnemyCount: initialEnemyCount
        );

        return repo.GetSave(save.Id) ?? save;
    }

    private static int ReadChoice(int min, int max)
    {
        while (true)
        {
            Console.Write($"\nEnter {min}-{max}: ");
            string? input = Console.ReadLine();

            if (int.TryParse(input, out int value) && value >= min && value <= max)
                return value;
        }
    }
}
