using System;
using System.Linq;

namespace ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.UI;

internal class GameOverScreen
{
    public void GameOver(int height, int width)
    {
        Console.Clear();

        string emptyLine = string.Concat(Enumerable.Repeat("■ ", width / 2));
        string gameOver = "Game Over!!";

        Console.ForegroundColor = ConsoleColor.Red;

        for (int i = 0; i < height; i++)
        {
            Console.WriteLine(emptyLine);
            Thread.Sleep(100);
        }

        Console.ResetColor();

        Console.SetCursorPosition((width / 2) - 4, height / 2);

        foreach (char c in gameOver)
        {
            Console.Write(c);
            Thread.Sleep(100);
        }

        Console.SetCursorPosition(0, height);
        Console.ReadKey();

        return;
    }
}
