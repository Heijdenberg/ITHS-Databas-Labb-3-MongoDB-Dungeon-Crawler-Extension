using MongoDB.Bson;
using ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.Data;
using ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.Elements;
using ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.UI;

namespace ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.Core;

internal class GameLoop
{
    private readonly LevelData _levelData;
    private readonly Player _player;
    private readonly MessageLog _messageLog;
    private readonly Renderer _renderer;
    private readonly Sidebar _sidebar;

    private readonly GameRepository _repo;
    private readonly SaveGameDocument _activeSave;
    private readonly int _initialEnemyCount;

    private int _turnCount;

    public GameLoop(LevelData levelData,
                    Player player,
                    MessageLog messageLog,
                    Sidebar sidebar,
                    GameRepository repo,
                    SaveGameDocument activeSave,
                    int startTurnCount,
                    int initialEnemyCount)
    {
        _levelData = levelData;
        _player = player;
        _messageLog = messageLog;
        _sidebar = sidebar;
        _renderer = new(levelData, player, messageLog, sidebar);

        _repo = repo;
        _activeSave = activeSave;

        _turnCount = startTurnCount;
        _initialEnemyCount = initialEnemyCount;

        _sidebar.TurnCount = _turnCount;
    }

    public async Task StartLoopAsync()
    {
        Console.Clear();
        _renderer.DrawAll();

        if (_activeSave.Messages is null || _activeSave.Messages.Count == 0)
        {
            StartMsg();
            _renderer.DrawAll();
        }

        while (true)
        {
            ConsoleKey thePressedKey = Console.ReadKey(intercept: true).Key;

            if (thePressedKey == ConsoleKey.M)
            {
                new MessageLogViewer(_messageLog).Run();
                _renderer.DrawAll();
                continue;
            }

            if (thePressedKey == ConsoleKey.Escape || thePressedKey == ConsoleKey.Q)
            {
                await SaveAndQuitAsync();
                return;
            }

            _player.Update(thePressedKey, _levelData, _messageLog);
            _renderer.DrawAll();

            UpdateEnemys();
            IncrementTurnCount();
            _renderer.DrawAll();

            if (thePressedKey == ConsoleKey.Enter || _levelData.GetEnemyCount() <= 0)
            {
                await SaveSnapshotOnlyAsync();

                VictoryScreen victoryScreen = new();
                victoryScreen.Victory(_levelData.LevelHeight, _levelData.LevelWidth);
                return;
            }

            if (_player.HitPoints.HP <= 0)
            {
                await SaveSnapshotOnlyAsync();
                await _repo.MarkDeadAsync(_activeSave.Id);

                GameOverScreen gameOverScreen = new();
                gameOverScreen.GameOver(_levelData.LevelHeight, _levelData.LevelWidth);
                return;
            }
        }
    }

    private async Task SaveAndQuitAsync()
    {
        await _repo.UpdateSaveFullAsync(_activeSave.Id,
                                       _activeSave,
                                       _levelData,
                                       _player,
                                       _messageLog,
                                       turnCount: _turnCount,
                                       initialEnemyCount: _initialEnemyCount);
    }

    private async Task SaveSnapshotOnlyAsync()
    {
        await _repo.UpdateSaveFullAsync(_activeSave.Id,
                                       _activeSave,
                                       _levelData,
                                       _player,
                                       _messageLog,
                                       turnCount: _turnCount,
                                       initialEnemyCount: _initialEnemyCount);
    }

    private void IncrementTurnCount()
    {
        _turnCount++;
        _sidebar.TurnCount = _turnCount;
    }

    private void UpdateEnemys()
    {
        foreach (var enemy in _levelData.Elements
                                        .OfType<Enemy>()
                                        .ToList())
        {
            enemy.Update(_levelData, _messageLog, _player);
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
