using ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.Core;

namespace ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.UI;

internal class Sidebar
{
    private readonly int _width;
    private readonly int _x;

    private readonly Player _player;
    private readonly LevelData _levelData;

    private int _turnCount = 0;
    private readonly int _enemyStartCount;

    private const int RowHearts = 1;
    private const int RowStatsStart = 3;
    private const int RowGoalTitle = 8;
    private const int RowGoalContent = 9;
    private const int RowControlsTitle = 10;
    private const int RowControlsStart = 11;
    private const int TotalLines = 15;

    public Sidebar(int levelHeight, int levelWidth, Player player, LevelData levelData, int initialEnemyCount)
    {
        _width = 24;
        _x = levelWidth + 1;

        _player = player;
        _levelData = levelData;
        _enemyStartCount = initialEnemyCount;
    }

    public int TurnCount
    {
        set { _turnCount = value; }
    }

    public int Width => _width;

    public void Draw()
    {
        DrawLayout();
        DrawLifeCounter(RowHearts);
        DrawGameStats(RowStatsStart);
        DrawGoal(RowGoalContent);
        DrawControls(RowControlsStart);
    }

    private void DrawLayout()
    {
        WriteTitleLine(0, '╔', '╗', "Life");

        WriteEmptyLine(RowHearts);

        WriteDividerLine(2);

        for (int r = RowStatsStart; r < RowStatsStart + 5; r++)
            WriteEmptyLine(r);

        WriteTitleLine(RowGoalTitle, '╠', '╣', "Goal");
        WriteEmptyLine(RowGoalContent);

        WriteTitleLine(RowControlsTitle, '╠', '╣', "Controls");
        WriteEmptyLine(RowControlsStart);
        WriteEmptyLine(RowControlsStart + 1);
        WriteEmptyLine(RowControlsStart + 2);

        Console.SetCursorPosition(_x, TotalLines - 1);
        Console.WriteLine($"╚{new string('═', _width - 2)}╝");
    }

    private void WriteEmptyLine(int row)
    {
        Console.SetCursorPosition(_x, row);
        Console.WriteLine($"║{new string(' ', _width - 2)}║");
    }

    private void WriteDividerLine(int row)
    {
        Console.SetCursorPosition(_x, row);
        Console.WriteLine($"╠{new string('═', _width - 2)}╣");
    }

    private void WriteTitleLine(int row, char left, char right, string title)
    {
        int fillCount = _width - (title.Length + 5);
        if (fillCount < 0) fillCount = 0;

        Console.SetCursorPosition(_x, row);
        Console.WriteLine($"{left}═ {title} {new string('═', fillCount)}{right}");
    }

    private void DrawLifeCounter(int row)
    {
        int hearts = _player.HitPoints.HP / 5;
        if (hearts <= 0) hearts = 1;

        Console.SetCursorPosition(_x + 2, row);
        Console.ForegroundColor = ConsoleColor.Red;

        int maxHeartsOnLine = _width - 4;
        hearts = Math.Min(hearts, maxHeartsOnLine);

        for (int i = 0; i < hearts; i++)
            Console.Write('♥');

        Console.ResetColor();
    }

    private void DrawGameStats(int startLine)
    {
        WriteText(startLine + 0, $" Player: {_player.Name}");
        WriteText(startLine + 1, $" HP: {_player.HitPoints.HP}");
        WriteText(startLine + 2, $" Turn: {_turnCount}");
        WriteText(startLine + 3, $" Enemys: {_levelData.GetEnemyCount()} of {_enemyStartCount}");
        WriteText(startLine + 4, $" Attack mod: {_player.AttackDice.Modifier}");
    }

    private void DrawGoal(int row)
    {
        WriteText(row, "♦ Kill all enemies");
    }

    private void DrawControls(int startRow)
    {
        WriteText(startRow + 0, "Move: WASD/↑↓←→");
        WriteText(startRow + 1, "Save/Quit: Q/Esc");
        WriteText(startRow + 2, "Messages: M");
    }

    private void WriteText(int row, string text)
    {
        int max = _width - 4;

        Console.SetCursorPosition(_x + 1, row);
        Console.Write(new string(' ', _width - 2));

        Console.SetCursorPosition(_x + 2, row);
        if (text.Length > max) text = text.Substring(0, max);
        Console.Write(text);
    }
}
