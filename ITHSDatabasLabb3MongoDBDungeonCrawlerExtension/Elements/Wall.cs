using ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.Core;
using ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.Interfaces;
using ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.Utilities;

namespace ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.Elements;

internal class Wall : LevelElement, IPlayerAwareDrawable
{
    public Wall(int row, int col)
        : base(sprite: '#', spriteColor: ConsoleColor.Gray, row: row, col: col)
    {
    }

    public bool IsDiscovered { get; set; }

    public void Draw(Player player)
    {
        if (GameMath.IsWithinRange(Position, player.Position, player.VisionRange))
        {
            IsDiscovered = true;
            base.Draw();
        }
        else if (IsDiscovered)
        {
            base.Draw();
        }
        else
        {
            Renderer.AddToRemoveList(Position);
        }
    }

    public void SetWall(int wallType)
    {
        Sprite = WallTypes[wallType];
    }

    private static readonly Dictionary<int, char> WallTypes = new()
    {
        { 0b0000, ' ' },
        { 0b0001, '╹' },
        { 0b0010, '╺' },
        { 0b0100, '╻' },
        { 0b1000, '╸' },
        { 0b0101, '━' },
        { 0b1010, '┃' },
        { 0b0011, '┓' },
        { 0b0110, '┏' },
        { 0b1100, '┗' },
        { 0b1001, '┛' },
        { 0b0111, '┣' },
        { 0b1110, '┫' },
        { 0b1101, '┳' },
        { 0b1011, '┻' },
        { 0b1111, '╋' }
    };
}
