using labb_2.Core;
using labb_2.Interfaces;
using labb_2.Utilities;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace labb_2.Elements;

internal class Wall : LevelElement, IPlayerAwareDrawable
{
    public Wall(int y, int x) : base('#', ConsoleColor.Gray, y, x) { }
    public override void Draw()
    {
        base.Draw();
    }
    public void Draw(Player player)
    {
        if (GameMath.IsWithinRange(Position, player.Position, player.VisionRange))
        {
            base.Draw();
        }
    }

    public void SetWall(int WallType)
    {
        Sprite = WallTypes[WallType];
    }

    static readonly Dictionary<int, char> WallTypes = new()
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
