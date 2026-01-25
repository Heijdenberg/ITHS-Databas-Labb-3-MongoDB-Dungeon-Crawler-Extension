using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace labb_2.UI
{
    internal class VictoryScreen
    {
        public void Victory(int height, int width)
        {
            Console.Clear();
            string emptyLine = string.Concat(Enumerable.Repeat("■ ", width / 2));
            string gameOver = "You Win!!";
            Console.ForegroundColor = ConsoleColor.Green;

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
            Environment.Exit(0);
        }
    }
}
