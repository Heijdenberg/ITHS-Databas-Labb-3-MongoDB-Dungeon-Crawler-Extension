using ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.Components;
using ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.Core;

namespace ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.UI;

internal class MessageLog
{
    private readonly List<string> _messages = new();
    private readonly int _maxVisible = 5;

    public MessageLog(int startRow, int maxWidth)
    {
        StartRow = startRow;
        MaxWidth = maxWidth;
    }

    private int StartRow { get; }
    private int MaxWidth { get; }

    public void Draw()
    {
        Renderer.DrawBox(new Position(StartRow + 1, 0), _maxVisible + 2, MaxWidth);

        var visible = _messages
            .TakeLast(_maxVisible)
            .ToList();

        int blanks = _maxVisible - visible.Count;
        for (int i = 0; i < blanks; i++)
            visible.Insert(0, "");

        for (int i = 0; i < _maxVisible; i++)
        {
            int row = StartRow + 2 + i;
            Console.SetCursorPosition(2, row);

            string msg = visible[i] ?? "";
            msg = msg.Length > (MaxWidth - 3) ? msg.Substring(0, MaxWidth - 3) : msg;
            msg = msg.PadRight(MaxWidth - 3, ' ');

            Console.Write(msg);
        }
    }

    public void AddMassage(string newMessage)
    {
        _messages.Add(newMessage);
        Draw();

        Thread.Sleep(500);

        while (Console.KeyAvailable)
            Console.ReadKey(intercept: true);
    }
}
