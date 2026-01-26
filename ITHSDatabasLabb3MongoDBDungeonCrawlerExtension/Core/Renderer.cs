using ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.Components;
using ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.Elements;
using ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.Interfaces;
using ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.UI;

namespace ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.Core;

internal class Renderer
{
    private readonly LevelData _levelData;
    private readonly Player _player;
    private readonly MessageLog _messageLog;
    private readonly Sidebar _sidebar;

    private static readonly List<Position> _removeList = new();

    public Renderer(LevelData levelData, Player player, MessageLog messageLog, Sidebar sidebar)
    {
        _levelData = levelData;
        _player = player;
        _messageLog = messageLog;
        _sidebar = sidebar;
    }

    public void DrawAll()
    {
        foreach (Position position in _removeList)
            EraseAtCoords(position);

        _removeList.Clear();

        foreach (LevelElement element in _levelData.Elements)
        {
            if (element is IPlayerAwareDrawable playerAwareDrawable)
                playerAwareDrawable.Draw(_player);
            else
                element.Draw();
        }

        _player.Draw();
        _messageLog.Draw();
        _sidebar.Draw();
    }

    public static void AddToRemoveList(Position position)
    {
        _removeList.Add(new Position(position.Row, position.Col));
    }

    public static void EraseAtCoords(Position position)
    {
        Console.SetCursorPosition(position.Col, position.Row);
        Console.Write(' ');
    }

    public static void DrawBox(Position startPos, int height, int width)
    {
        if (width < 3) width = 3;
        if (height < 3) height = 3;

        Console.SetCursorPosition(startPos.Col, startPos.Row);

        Console.WriteLine($"╔{new string('═', width - 2)}╗");
        for (int i = 0; i < height - 2; i++)
            Console.WriteLine($"║{new string(' ', width - 2)}║");
        Console.WriteLine($"╚{new string('═', width - 2)}╝");
    }
}
