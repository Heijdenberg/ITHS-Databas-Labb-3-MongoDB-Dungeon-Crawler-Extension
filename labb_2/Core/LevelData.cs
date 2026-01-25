using labb_2.Elements;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace labb_2.Core;

internal class LevelData
{
    private List<LevelElement> _elements = new();
    private int _levelWidth = 0;
    public int LevelHeight { get; set; }
    public int LevelWidth
    {
        get
        {
            return _levelWidth;
        }
        set
        {
            if (value > _levelWidth)
            {
                _levelWidth = value;
            }
        }
    }
    public List<LevelElement> Elements
    {
        get
        {
            return _elements;
        }
    }

    public int[] Load(string path)
    {
        int[] startPosition = [0, 0];
        using (StreamReader reader = new StreamReader(path))
        {
            string? line;

            for (int i = 0; (line = reader.ReadLine()) != null; i++)
            {
                for (int ii = 0; ii < line.Length; ii++)
                {
                    switch (line[ii])
                    {
                        case '@':
                            startPosition = [i, ii];
                            break;
                        case '#':
                            _elements.Add(new Wall(i, ii));
                            LevelWidth = ii;
                            LevelHeight = i;
                            break;
                        case 'r':
                            _elements.Add(new Rat(i, ii));
                            break;
                        case 's':
                            _elements.Add(new Snake(i, ii));
                            break;
                    }
                }
            }
        }
        return startPosition;
    }

    public LevelElement? GetElementAtPosition(int y, int x)
    {
        LevelElement? inhabitant = null;


        for (int i = 0; i < Elements.Count; i++)
        {
            if (Elements[i].Position.Y == y && Elements[i].Position.X == x)
            {
                inhabitant = Elements[i];
            }
        }
        return inhabitant;
    }
    public int GetElementIndexAtPosition(int y, int x)
    {
        int index = -1;

        for (int i = 0; i < Elements.Count; i++)
        {
            if (Elements[i].Position.Y == y && Elements[i].Position.X == x)
            {
                index = i;
            }
        }
        return index;
    }

    public int GetEnemyCount()
    {
        int enemyCount = 0;

        foreach (LevelElement element in _elements)
        {
            if (element is Enemy)
            {
                enemyCount++;
            }
        }

        return enemyCount;
    }
    public void removeElement(int y, int x)
    {
        int index = GetElementIndexAtPosition(y, x);
        Elements.RemoveAt(index);
    }

    public void SolidWalls()
    {
        LevelElement[,] walls = new LevelElement[LevelHeight + 100, LevelWidth + 100];

        foreach (LevelElement element in _elements.Where(e => e is Wall).Cast<Wall>())
        {
            Wall wall = (Wall)element;
            walls[wall.Position.Y, wall.Position.X] = wall;
        }

        foreach (LevelElement element in walls)
        {
            if (element is not null)
            {
                int wallType = 0;
                Wall wall = (Wall)element;
                int y = wall.Position.Y;
                int x = wall.Position.X;

                int H = walls.GetLength(0);
                int W = walls.GetLength(1);

                bool InBounds(int yy, int xx) => yy >= 0 && yy < H && xx >= 0 && xx < W;
                bool IsWall(int yy, int xx) => InBounds(yy, xx) && walls[yy, xx] is not null;

                if (IsWall(y - 1, x)) wallType |= 1;
                if (IsWall(y, x + 1)) wallType |= 2;
                if (IsWall(y + 1, x)) wallType |= 4;
                if (IsWall(y, x - 1)) wallType |= 8;

                wall.SetWall(wallType);
            }
        }
    }
}
