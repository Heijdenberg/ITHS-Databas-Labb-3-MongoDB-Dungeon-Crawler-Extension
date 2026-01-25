using ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.Components;
using ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.Elements;
using ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.Interfaces;
using ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.Core;

internal class Renderer
{
    private LevelData _levelData;
    private Player _player;
    private MessageLog _messageLog;
    private Sidebar _sidebar;
    static private List<Position> _removeList = new();

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
        {
            EraseAtCords(position);
        }
        _removeList.Clear();

        foreach (LevelElement element in _levelData.Elements)
        {
            if (element is IPlayerAwareDrawable playerAwareDrawable)
            {
                playerAwareDrawable.Draw(_player);
            }
            else
            {
                element.Draw();
            }
        }

        _player.Draw();
        _messageLog.Draw();
        _sidebar.Draw();
    }

    static public void AddToRemoveList(Position position)
    {
        _removeList.Add(new Position(position.Y, position.X));
    }

    static public void EraseAtCords(Position position)
    {
        Console.SetCursorPosition(position.X, position.Y);
        Console.Write(' ');
    }

    static public void DrawBox(Position startPos, int hight, int width)
    {
        if (width < 3)
        {
            width = 3;
        }

        if (hight < 3)
        {
            hight = 3;
        }

        Console.SetCursorPosition(startPos.Y, startPos.X);

        Console.WriteLine($"╔{new string('═', width - 2)}╗");
        for (int ii = 0; ii < hight-2; ii++)
        {
            Console.WriteLine($"║{new string(' ', width - 2)}║");
        }
        Console.WriteLine($"╚{new string('═', width - 2)}╝");

    }
}




