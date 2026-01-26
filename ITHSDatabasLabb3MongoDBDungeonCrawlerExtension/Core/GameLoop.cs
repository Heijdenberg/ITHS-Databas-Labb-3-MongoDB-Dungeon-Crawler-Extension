using ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.Elements;
using ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.Core;

internal class GameLoop
{
    private LevelData _levelData;
    private Player _player;
    private MessageLog _messageLog;
    private Renderer _renderer;
    private Sidebar _sidebar;
    private int _turnCount;

    public GameLoop(LevelData levelData, Player player, MessageLog messageLog, Sidebar sidebar)
    {
        _levelData = levelData;
        _player = player;
        _messageLog = messageLog;
        _renderer = new(levelData, player, messageLog, sidebar);
        _turnCount = 0;
        _sidebar = sidebar;
    }

    public void StartLoop()
    {
        _renderer.DrawAll();
        StartMsg();

        while (true)
        {
            ConsoleKey thePressedKey = Console.ReadKey(intercept: true).Key;

            if (thePressedKey == ConsoleKey.M)
            {
                new MessageLogViewer(_messageLog).Run();
                _renderer.DrawAll();
                continue;
            }

            _player.Update(thePressedKey, _levelData, _messageLog);
            _renderer.DrawAll();
            UpdateEnemys();
            IncrementTurnCount();
            _renderer.DrawAll();

            if (thePressedKey == ConsoleKey.Enter || _levelData.GetEnemyCount() <= 0)
            {
                VictoryScreen victoryScreen = new();
                victoryScreen.Victory(_levelData.LevelHeight, _levelData.LevelWidth);
            }
            else if (thePressedKey == ConsoleKey.Escape || _player.HitPoints.HP <= 0)
            {
                GameOverScreen gameOverScreen = new();
                gameOverScreen.GameOver(_levelData.LevelHeight, _levelData.LevelWidth);
            }
        }
    }

    private void IncrementTurnCount()
    {
        _turnCount++;
        _sidebar.TurnCount = _turnCount;
    }
    private void UpdateEnemys()
    {
        foreach (var element in _levelData.Elements
                                 .OfType<Enemy>()
                                 .ToList())
        {
            if (element is Enemy enemy)
            {
                enemy.Update(_levelData, _messageLog, _player);
            }
        }
    }

    private void StartMsg()
    {
        _messageLog.AddMassage(" ");
        _messageLog.AddMassage("@ is you.");
        _messageLog.AddMassage("Walk by using WASD or ↑←↓→.");
        _messageLog.AddMassage("Your goal is to kill all the enemies.");
        _messageLog.AddMassage("Killing enemies will give you power ups.");
    }
}
