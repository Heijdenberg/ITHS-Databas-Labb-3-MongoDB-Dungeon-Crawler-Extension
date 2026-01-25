using labb_2.Components;
using labb_2.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace labb_2.UI
{
    internal class NameScreen
    {
        public static string SetName(int hight, int width)
        {
            Renderer.DrawBox(new Position(0, 0), hight, width);

            Console.SetCursorPosition((width / 2) - 10, (hight / 2));
            Console.Write("Name: ");
            string name = Console.ReadLine();
            name = char.ToUpper(name[0]) + name.Substring(1);

            Console.Clear();
            Renderer.DrawBox(new Position(0,0), hight, width);

            Console.SetCursorPosition((width / 2) - 10, (hight / 2));
            Console.WriteLine($"Welcome {name}!!!");
            Thread.Sleep(1500);

            Console.Clear();

            return name;
        }
    }
}
