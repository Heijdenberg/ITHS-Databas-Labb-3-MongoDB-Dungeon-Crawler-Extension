using ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.Components;
using ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.Elements;
using ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.Interfaces;
using ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.UI;
using ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.Utilities;

namespace ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.Core;

internal class Player : LevelElement, ICombatant
{
    public Player(int[] startPosition, string name)
        : base(sprite: '@',
               spriteColor: ConsoleColor.Cyan,
               row: startPosition[0],
               col: startPosition[1])
    {
        Name = name;
        AttackDice = new(2, 6, 2);
        DefenceDice = new(2, 6, 0);
        HitPoints = new HitPoints(100);
        VisionRange = 5;
    }

    public string Name { get; }
    public HitPoints HitPoints { get; set; }
    public Dice AttackDice { get; }
    public Dice DefenceDice { get; }
    public int VisionRange { get; }

    public void Death(LevelData levelData, MessageLog messageLog, ICombatant killer)
    {
        messageLog.AddMassage($"{Name} is dead, slayed by {killer.Name}");
        Thread.Sleep(800);
        new GameOverScreen().GameOver(levelData.LevelHeight, levelData.LevelWidth);
    }

    public void Update(ConsoleKey direction, LevelData levelData, MessageLog messageLog)
    {
        int row = Position.Row;
        int col = Position.Col;

        if (direction == ConsoleKey.UpArrow || direction == ConsoleKey.W) row--;
        else if (direction == ConsoleKey.DownArrow || direction == ConsoleKey.S) row++;
        else if (direction == ConsoleKey.LeftArrow || direction == ConsoleKey.A) col--;
        else if (direction == ConsoleKey.RightArrow || direction == ConsoleKey.D) col++;

        LevelElement? next = levelData.GetElementAtPosition(row, col);

        if (next is null)
        {
            Renderer.AddToRemoveList(Position);
            Position.Row = row;
            Position.Col = col;
        }
        else if (next is Enemy)
        {
            messageLog.AddMassage("Found an Enemy!");
            new Combat(this, (ICombatant)next).Battle(messageLog, levelData);
        }
    }
}
