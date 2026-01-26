using ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.Components;

namespace ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.Elements;

internal abstract class LevelElement
{
    protected LevelElement(char sprite, ConsoleColor spriteColor, int row, int col)
    {
        Sprite = sprite;
        SpriteColor = spriteColor;
        Position = new Position(row, col);
    }

    public char Sprite { get; set; }
    public Position Position { get; set; }
    public ConsoleColor SpriteColor { get; }

    public virtual void Draw()
    {
        Console.SetCursorPosition(Position.Col, Position.Row);
        Console.ForegroundColor = SpriteColor;
        Console.Write(Sprite);
        Console.ResetColor();
    }
}
