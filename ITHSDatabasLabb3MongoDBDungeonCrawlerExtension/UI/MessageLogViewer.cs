using ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.Components;
using ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.Core;

namespace ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.UI;

internal class MessageLogViewer
{
    private readonly MessageLog _log;

    public MessageLogViewer(MessageLog log)
    {
        _log = log;
    }

    public void Run()
    {
        Console.Clear();

        int topIndex = 0;

        while (true)
        {
            Draw(ref topIndex);

            ConsoleKey key = Console.ReadKey(intercept: true).Key;

            if (key == ConsoleKey.M || key == ConsoleKey.Escape)
                break;

            int height = Math.Max(10, Console.WindowHeight);
            int width = Math.Max(20, Console.WindowWidth);

            int contentHeight = height - 4;
            int maxTop = Math.Max(0, _log.Messages.Count - contentHeight);

            switch (key)
            {
                case ConsoleKey.UpArrow:
                    topIndex = Math.Max(0, topIndex - 1);
                    break;

                case ConsoleKey.DownArrow:
                    topIndex = Math.Min(maxTop, topIndex + 1);
                    break;

                case ConsoleKey.PageUp:
                    topIndex = Math.Max(0, topIndex - contentHeight);
                    break;

                case ConsoleKey.PageDown:
                    topIndex = Math.Min(maxTop, topIndex + contentHeight);
                    break;

                case ConsoleKey.Home:
                    topIndex = 0;
                    break;

                case ConsoleKey.End:
                    topIndex = maxTop;
                    break;
            }
        }

        Console.Clear();
    }

    private void Draw(ref int topIndex)
    {
        int height = Math.Max(10, Console.WindowHeight);
        int width = Math.Max(20, Console.WindowWidth);

        int boxHeight = Math.Max(3, height - 1);
        int boxWidth = Math.Max(3, width - 1);

        Console.SetCursorPosition(0, 0);
        Renderer.DrawBox(new Position(0, 0), boxHeight, boxWidth);

        string header = "MESSAGE LOG  (Up/Down, PgUp/PgDn, Home/End)   [M/Esc to return]";
        header = header.Length > (boxWidth - 3) ? header.Substring(0, boxWidth - 3) : header;
        Console.SetCursorPosition(2, 1);
        Console.Write(header.PadRight(boxWidth - 3, ' '));

        int contentStartRow = 2;
        int contentHeight = boxHeight - 4;
        int contentWidth = boxWidth - 3;

        int maxTop = Math.Max(0, _log.Messages.Count - contentHeight);
        if (topIndex < 0) topIndex = 0;
        if (topIndex > maxTop) topIndex = maxTop;

        for (int i = 0; i < contentHeight; i++)
        {
            int row = contentStartRow + i;
            Console.SetCursorPosition(2, row);
            Console.Write(new string(' ', contentWidth));
        }

        for (int i = 0; i < contentHeight; i++)
        {
            int msgIndex = topIndex + i;
            if (msgIndex >= _log.Messages.Count) break;

            string msg = _log.Messages[msgIndex] ?? "";
            msg = msg.Replace('\t', ' ');

            if (msg.Length > contentWidth)
                msg = msg.Substring(0, contentWidth);

            Console.SetCursorPosition(2, contentStartRow + i);
            Console.Write(msg.PadRight(contentWidth, ' '));
        }

        string footer = _log.Messages.Count == 0
            ? "0 messages"
            : $"Messages {topIndex + 1}-{Math.Min(_log.Messages.Count, topIndex + contentHeight)} of {_log.Messages.Count}";

        footer = footer.Length > contentWidth ? footer.Substring(0, contentWidth) : footer;
        Console.SetCursorPosition(2, boxHeight - 2);
        Console.Write(footer.PadRight(contentWidth, ' ')); // <- spaces, not '═'
    }
}
