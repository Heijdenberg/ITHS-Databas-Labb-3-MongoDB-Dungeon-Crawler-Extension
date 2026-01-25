using ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.Utilities;

static class GameMath
{
    public static bool IsWithinRange(Position a, Position b, double distance)
    {
        int y = a.Y;
        int x = a.X;

        int yDif = b.Y - y;
        int xDif = b.X - x;

        if (distance >= Math.Sqrt(Math.Pow(xDif, 2) + Math.Pow(yDif, 2)))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
