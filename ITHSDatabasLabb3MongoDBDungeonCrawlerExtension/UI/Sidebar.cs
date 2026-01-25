using ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.UI;

internal class Sidebar
{
    private readonly int _height;
    private readonly int _width;
    private readonly int _x;
    private readonly Player _player;
    private int _turnCount = 0;
    private int _enemyStartCount;
    private LevelData _levelData;

    public Sidebar(int levelHeight, int levelWidth, Player player, LevelData levelData)
    {
        _height = 5;
        _width = 24;
        _x = levelWidth + 1;
        _player = player;
        _levelData = levelData;
        _enemyStartCount = levelData.GetEnemyCount();
    }

    public int TurnCount
    {
        set { _turnCount = value; }
    }

    public int Width
    {
        get { return _width; }
    }

    public void Draw()
    {
        DrawBox();
        DrawLifeCounter();
        DrawGameStats(3);
        DrawGoal(10);
    }

    private void DrawBox()
    {
        Console.SetCursorPosition(_x, 0);
        Console.WriteLine($"╔═ Life {new string('═', _width - 9)}╗");
        Console.SetCursorPosition(_x, Console.GetCursorPosition().Top);
        Console.WriteLine($"║{new string(' ', _width - 2)}║");
        Console.SetCursorPosition(_x, Console.GetCursorPosition().Top);
        Console.WriteLine($"╠{new string('═', _width - 2)}╣");
        Console.SetCursorPosition(_x, Console.GetCursorPosition().Top);

        for (int i = 0; i < _height; i++)
        {
            Console.SetCursorPosition(_x, Console.GetCursorPosition().Top);
            Console.WriteLine($"║{new string(' ', _width - 2)}║");
        }

        Console.SetCursorPosition(_x, Console.GetCursorPosition().Top);
        Console.WriteLine($"╚{new string('═', _width - 2)}╝");
        Console.SetCursorPosition(_x + 1, 1);
        Console.ForegroundColor = ConsoleColor.Red;
        Console.SetCursorPosition(_x + 2, Console.GetCursorPosition().Top);
    }

    private void DrawLifeCounter()
    {
        int hearts = _player.HitPoints.HP / 5;

        if (hearts == 0)
        {
            hearts = 1;
        }

        while (hearts > 0)
        {
            Console.Write('♥');
            if (hearts % (_width - 2) == 1)
            {
                Console.WriteLine();
                Console.SetCursorPosition(_x + 2, Console.GetCursorPosition().Top);
            }
            hearts--;
        }

        Console.ResetColor();
    }

    private void DrawGameStats(int startLine)
    {
        Console.SetCursorPosition(_x + 1, startLine);
        Console.WriteLine($" Player: {_player.Name}");
        startLine++;

        Console.SetCursorPosition(_x + 1, startLine);
        Console.WriteLine($" HP: {_player.HitPoints.HP}");
        startLine++;

        Console.SetCursorPosition(_x + 1, startLine);
        Console.WriteLine($" Turn: {_turnCount}");
        startLine++;

        Console.SetCursorPosition(_x + 1, startLine);
        Console.WriteLine($" Enemys: {_levelData.GetEnemyCount()} of {_enemyStartCount}");
        startLine++;

        Console.SetCursorPosition(_x + 1, startLine);
        Console.WriteLine($" Attack modifier: {_player.AttackDice.Modifier}");
    }

    private void DrawGoal(int startLine)
    {
        Console.SetCursorPosition(_x, startLine);
        Console.WriteLine($"╔═ Goal {new string('═', _width - 9)}╗");
        startLine++;

        Console.SetCursorPosition(_x, startLine);
        Console.WriteLine($"║{new string(' ', _width - 2)}║");
        startLine++;

        Console.SetCursorPosition(_x, startLine);
        Console.WriteLine($"╚{new string('═', _width - 2)}╝");
        startLine--;

        Console.SetCursorPosition(_x + 2, startLine);
        Console.WriteLine($"♦ Kill all enemies");
    }
}
