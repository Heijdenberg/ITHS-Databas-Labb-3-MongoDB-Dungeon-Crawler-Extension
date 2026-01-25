using labb_2.Components;
using labb_2.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace labb_2.UI;

internal class MessageLog
{
    private List<string> _messages = new() { "", "", "", "", "" };
    private readonly int _maxMessages = 5;

    public MessageLog(int atartRow, int maxWidth)
    {
        StartRow = atartRow;
        MaxWidth = maxWidth;
    }

    private int StartRow { get; set; }
    private int MaxWidth { get; set; }

    public void Draw()
    {
        Renderer.DrawBox(new Position(StartRow+1,0), _maxMessages+2, MaxWidth);

        Console.SetCursorPosition(1, StartRow + 2);
        foreach (string message in _messages)
        {
            Console.SetCursorPosition(2, Console.CursorTop);
            string messageLine = $"{message}{new string(' ', MaxWidth - 2)}{new string(' ', MaxWidth - 2)}";
            messageLine = messageLine.Substring(0, MaxWidth - 3);
            Console.WriteLine(messageLine);
        }
    }
    public void AddMassage(string newMessages)
    {
        _messages.Add(newMessages);

        if (_messages.Count > _maxMessages)
        {
            _messages.RemoveAt(0);
        }
        Draw();
        Thread.Sleep(500);

        while (Console.KeyAvailable)
        {
            Console.ReadKey(intercept: true);
        }
    }
}
