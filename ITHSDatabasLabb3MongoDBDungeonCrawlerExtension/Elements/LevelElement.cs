using ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.Components;
using ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.Elements;

internal abstract class LevelElement
{
    protected LevelElement(char sprite, ConsoleColor spriteColor, int y, int x)
    {
        Sprite = sprite;
        SpriteColor = spriteColor;
        Position = new Position(y, x);
    }
    public char Sprite { get; set; }
    public Position Position { get; set; }
    public ConsoleColor SpriteColor { get; }

    public virtual void Draw()
    {
        Console.SetCursorPosition(Position.Y, Position.X);
        Console.ForegroundColor = SpriteColor;
        Console.Write(Sprite);
        Console.ResetColor();
    }
}
