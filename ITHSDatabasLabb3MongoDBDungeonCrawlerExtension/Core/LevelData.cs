using ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.Elements;

namespace ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.Core;

internal class LevelData
{
    private readonly List<LevelElement> _elements = new();
    private int _levelWidth = 0;

    public int LevelHeight { get; set; }

    public int LevelWidth
    {
        get => _levelWidth;
        set
        {
            if (value > _levelWidth)
                _levelWidth = value;
        }
    }

    public List<LevelElement> Elements => _elements;

    public int[] Load(string path)
    {
        int[] startPosition = [0, 0];

        using StreamReader reader = new(path);
        string? line;

        for (int row = 0; (line = reader.ReadLine()) != null; row++)
        {
            string scanLine = line.TrimEnd(' ');

            for (int col = 0; col < scanLine.Length; col++)
            {
                switch (scanLine[col])
                {
                    case '@':
                        startPosition = [row, col];
                        break;

                    case '#':
                        _elements.Add(new Wall(row, col));

                        LevelWidth = col;
                        LevelHeight = row;
                        break;

                    case 'r':
                        _elements.Add(new Rat(row, col));
                        break;

                    case 's':
                        _elements.Add(new Snake(row, col));
                        break;
                }
            }
        }

        return startPosition;
    }


    public LevelElement? GetElementAtPosition(int row, int col)
    {
        for (int i = 0; i < Elements.Count; i++)
        {
            if (Elements[i].Position.Row == row && Elements[i].Position.Col == col)
                return Elements[i];
        }
        return null;
    }

    public int GetElementIndexAtPosition(int row, int col)
    {
        for (int i = 0; i < Elements.Count; i++)
        {
            if (Elements[i].Position.Row == row && Elements[i].Position.Col == col)
                return i;
        }
        return -1;
    }

    public int GetEnemyCount()
    {
        int enemyCount = 0;
        foreach (LevelElement element in _elements)
        {
            if (element is Enemy)
                enemyCount++;
        }
        return enemyCount;
    }

    public void RemoveElement(int row, int col)
    {
        int index = GetElementIndexAtPosition(row, col);
        if (index >= 0 && index < Elements.Count)
            Elements.RemoveAt(index);
    }

    public void SolidWalls()
    {
        LevelElement[,] walls = new LevelElement[LevelHeight + 100, LevelWidth + 100];

        foreach (LevelElement element in _elements.Where(e => e is Wall))
        {
            Wall wall = (Wall)element;
            walls[wall.Position.Row, wall.Position.Col] = wall;
        }

        foreach (LevelElement element in walls)
        {
            if (element is null) continue;

            int wallType = 0;
            Wall wall = (Wall)element;

            int row = wall.Position.Row;
            int col = wall.Position.Col;

            int H = walls.GetLength(0);
            int W = walls.GetLength(1);

            bool InBounds(int r, int c) => r >= 0 && r < H && c >= 0 && c < W;
            bool IsWall(int r, int c) => InBounds(r, c) && walls[r, c] is not null;

            if (IsWall(row, col - 1)) wallType |= 1;
            if (IsWall(row + 1, col)) wallType |= 2;
            if (IsWall(row, col + 1)) wallType |= 4;
            if (IsWall(row - 1, col)) wallType |= 8;

            wall.SetWall(wallType);
        }
    }
}
