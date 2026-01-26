using ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.Core;
using ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.Interfaces;
using ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.UI;
using ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.Utilities;

namespace ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.Elements;

internal class Rat : Enemy, IPlayerAwareDrawable
{
    public Rat(int row, int col)
        : base(name: "rat",
               hp: 10,
               attackDice: new Dice(1, 6, 3),
               defenceDice: new Dice(1, 6, 1),
               sprite: 'r',
               color: ConsoleColor.Red,
               row, col)
    { }

    public override void Update(LevelData levelData, MessageLog messageLog, Player player)
    {
        if (HitPoints.HP <= 0) return;

        int row = Position.Row;
        int col = Position.Col;

        int direction = GameRandom.Random.Next(0, 4);

        if (direction == 0) col--;
        else if (direction == 1) row--;
        else if (direction == 2) col++;
        else if (direction == 3) row++;

        LevelElement? next = levelData.GetElementAtPosition(row, col);

        if (next is null && (row != player.Position.Row || col != player.Position.Col))
        {
            Renderer.AddToRemoveList(Position);
            Position.Row = row;
            Position.Col = col;
        }
        else if (row == player.Position.Row && col == player.Position.Col)
        {
            messageLog.AddMassage($"{Name} attacked player!");
            new Combat(this, player).Battle(messageLog, levelData);
        }
    }

    public override void Death(LevelData levelData, MessageLog messageLog, ICombatant killer)
    {
        base.Death(levelData, messageLog, killer);

        int hpDrop = 10;
        killer.HitPoints.HP += hpDrop;
        messageLog.AddMassage($"You eat {Name} and got {hpDrop}HP");
    }
}
