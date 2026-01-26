using ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.Components;

namespace ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.Utilities;

static class GameMath
{
    public static bool IsWithinRange(Position a, Position b, double distance)
    {
        int rowDif = b.Row - a.Row;
        int colDif = b.Col - a.Col;

        return distance >= Math.Sqrt(Math.Pow(colDif, 2) + Math.Pow(rowDif, 2));
    }
}
