using ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.Core;
using ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.UI;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

namespace ITHSDatabasLabb3MongoDBDungeonCrawlerExtension;

internal class Program
{
    static void Main()
    {
        Console.CursorVisible = false;
        Console.OutputEncoding = Encoding.UTF8;

        //StartUpScreen.Draw();
        
        LevelData levelData = new();
        string path = LevelSelect.GetFilePath();
        int[] startPosition = levelData.Load(path);
        string playerName = NameScreen.SetName(levelData.LevelHeight, levelData.LevelWidth);
        Player player = new(startPosition, playerName);
        MessageLog messageLog = new(levelData.LevelHeight, levelData.LevelWidth);
        Sidebar sidebar = new(levelData.LevelHeight, levelData.LevelWidth, player, levelData);
        GameLoop gameLoop = new(levelData, player, messageLog, sidebar);

        if (OperatingSystem.IsWindows())
        {
            Console.BufferWidth += levelData.LevelWidth + sidebar.Width;
        }

        levelData.SolidWalls();
        gameLoop.StartLoop();
    }
}

