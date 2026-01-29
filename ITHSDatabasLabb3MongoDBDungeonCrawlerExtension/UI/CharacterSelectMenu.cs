using ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.Components;
using ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.Core;
using ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.Data;
using MongoDB.Bson;

namespace ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.UI;

internal static class CharacterSelectMenu
{
    public static async Task<SaveGameDocument> RunAsync(GameRepository repo)
    {
        int selected = 0;

        while (true)
        {
            Console.Clear();

            var saves = await repo.GetAllSavesAsync();

            var lines = saves.Select(s =>
            {
                string deadTag = s.IsDead ? " — DEAD" : "";
                return $"{s.PlayerName}{deadTag}  (Last played: {s.LastPlayedUtc:yyyy-MM-dd HH:mm})";
            }).ToList();

            lines.Add("New character");

            if (selected < 0) selected = 0;
            if (selected > lines.Count - 1) selected = lines.Count - 1;

            string help = "W/S or ↑↓ move, Enter select, X delete, Esc exit";

            int contentWidth = Math.Max(
                lines.Max(l => l.Length),
                help.Length
            );

            int width = Math.Max(30, contentWidth + 6);

            width = Math.Min(width, Math.Max(30, Console.WindowWidth - 1));

            int height = Math.Max(6, lines.Count + 4);

            Renderer.DrawBox(new Position(0, 0), height, width);

            Console.SetCursorPosition(2, 1);
            Console.Write("CHARACTER SELECT");

            Console.SetCursorPosition(2, 2);
            Console.Write(help);

            for (int i = 0; i < lines.Count; i++)
            {
                Console.SetCursorPosition(2, 3 + i);

                Console.Write(i == selected ? ">" : " ");

                string text;
                if (i < saves.Count)
                    text = $"{i}. {lines[i]}";
                else
                    text = $"{i}. {lines[i]}";

                if (text.Length > width - 4) text = text.Substring(0, width - 4);
                Console.Write(text.PadRight(width - 4, ' '));
            }

            ConsoleKey key = Console.ReadKey(intercept: true).Key;

            if (key == ConsoleKey.Escape)
            {
                Console.Clear();
                Environment.Exit(0);
            }

            // DELETE
            if (key == ConsoleKey.X || key == ConsoleKey.Delete)
            {
                // Don't delete the "New character" row
                if (selected == lines.Count - 1)
                    continue;

                var chosen = saves[selected];

                Console.SetCursorPosition(2, height - 2);
                Console.Write(new string(' ', width - 4)); // clear row
                Console.SetCursorPosition(2, height - 2);
                Console.Write($"Delete '{chosen.PlayerName}'? (Y/N) ");

                var confirm = Console.ReadKey(intercept: true).Key;
                if (confirm == ConsoleKey.Y)
                {
                    await repo.DeleteSaveAsync(chosen.Id);

                    // Keep selection valid next redraw
                    selected = Math.Max(0, selected - 1);
                }

                continue;
            }

            if (key == ConsoleKey.S || key == ConsoleKey.DownArrow)
            {
                if (selected < lines.Count - 1) selected++;
                continue;
            }

            if (key == ConsoleKey.W || key == ConsoleKey.UpArrow)
            {
                if (selected > 0) selected--;
                continue;
            }

            if (key == ConsoleKey.Enter)
            {
                if (selected == lines.Count - 1)
                    return await CreateNewCharacterFlowAsync(repo);

                var chosen = saves[selected];

                if (chosen.IsDead)
                {
                    Console.SetCursorPosition(2, height - 2);
                    Console.Write("That character is dead and cannot be loaded. Press any key...");
                    Console.ReadKey(true);
                    continue;
                }

                await repo.TouchLastPlayedAsync(chosen.Id);
                return await repo.GetSaveAsync(chosen.Id) ?? chosen;
            }
        }
    }

    private static async Task<SaveGameDocument> CreateNewCharacterFlowAsync(GameRepository repo)
    {
        Console.Clear();
        Console.WriteLine("=== NEW CHARACTER ===\n");

        Console.Write("Enter character name: ");
        string name = (Console.ReadLine() ?? "").Trim();
        if (string.IsNullOrWhiteSpace(name))
            name = "Player";

        var archetypes = await repo.GetAllArchetypesAsync();
        if (archetypes.Count == 0)
        {
            Console.WriteLine("\nNo archetypes found in DB (seed failed?). Press any key...");
            Console.ReadKey(true);
            return await RunAsync(repo);
        }

        Console.WriteLine("\nChoose archetype:");
        for (int i = 0; i < archetypes.Count; i++)
            Console.WriteLine($"{i + 1}. {archetypes[i].Name}");

        int archIndex = ReadChoice(1, archetypes.Count) - 1;
        ObjectId archetypeId = archetypes[archIndex].Id;

        Console.Clear();
        Console.WriteLine("=== SELECT LEVEL ===\n");
        string levelPath = LevelSelect.GetFilePath();

        LevelData levelData = new();
        int[] startPosition = levelData.Load(levelPath);
        levelData.SolidWalls();

        int initialEnemyCount = levelData.GetEnemyCount();

        var save = await repo.CreateNewSaveAsync(name, archetypeId, levelPath, startPosition);

        var tempPlayer = new Player(startPosition, name);
        tempPlayer.AttackDice.Modifier = 2;
        tempPlayer.DefenceDice.Modifier = 0;

        var tempLog = new MessageLog(levelData.LevelHeight, levelData.LevelWidth);

        await repo.UpdateSaveFullAsync(
            save.Id,
            save,
            levelData,
            tempPlayer,
            tempLog,
            turnCount: 0,
            initialEnemyCount: initialEnemyCount
        );

        return await repo.GetSaveAsync(save.Id) ?? save;
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
