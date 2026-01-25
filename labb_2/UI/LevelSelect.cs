using labb_2.Components;
using labb_2.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace labb_2.UI;

internal class LevelSelect
{
    public static string GetFilePath()
    {
        string folderPath = @".\Levels";
        string[] files = Directory.GetFiles(folderPath);
        int levelNum = 0;
        int width = 20;

        Renderer.DrawBox(new Position(0,0), files.Length+2, width);

        levelNum = levelSelect(files);

        return files[levelNum];
    }
    private static int levelSelect(string[] files)
    {
        int levelNum =0;
        while (true)
        {
            Console.SetCursorPosition(2, 1);
            for (int i = 0; i < files.Length; i++)
            {
                Console.SetCursorPosition(2, Console.CursorTop);
                if (i == levelNum)
                {
                    Console.Write(">");
                }
                else
                {
                    Console.Write(" ");
                }
                Console.WriteLine($"{i}.{files[i].Substring(9)}");
            }

            ConsoleKey key = Console.ReadKey(intercept: true).Key;
            if (key == ConsoleKey.Enter)
            {
                break;
            }
            else if (key == ConsoleKey.S || key == ConsoleKey.DownArrow)
            {
                if (levelNum < files.Length - 1)
                {
                    levelNum++;
                }
            }
            else if (key == ConsoleKey.W || key == ConsoleKey.UpArrow)
            {
                if (levelNum > 0)
                {
                    levelNum--;
                }
            }
        }

        return levelNum;
    }
}
