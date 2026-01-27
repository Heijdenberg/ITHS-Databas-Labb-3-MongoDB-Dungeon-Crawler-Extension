using ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.Core;
using ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.Interfaces;
using ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.UI;
using ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.Utilities;

namespace ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.Elements;

internal class Snake : Enemy, IPlayerAwareDrawable
{
    public Snake(int row, int col)
        : base(name: "Snake",
               hp: 25,
               attackDice: new Dice(3, 4, 2),
               defenceDice: new Dice(1, 8, 5),
               sprite: 's',
               color: ConsoleColor.Green,
               row, col)
    { }

    public override void Update(LevelData levelData, MessageLog messageLog, Player player)
    {
        if (HitPoints.HP <= 0) return;

        int row = Position.Row;
        int col = Position.Col;

        int rowDif = player.Position.Row - row;
        int colDif = player.Position.Col - col;

        if (GameMath.IsWithinRange(Position, player.Position, 2.0))
        {
            if (Math.Abs(rowDif) == Math.Abs(colDif))
            {
                int randomAxis = GameRandom.Random.Next(0, 2);

                if (randomAxis == 0)
                    row += (rowDif > 0 ? -1 : 1);
                else
                    col += (colDif > 0 ? -1 : 1);
            }
            else if (Math.Abs(rowDif) > Math.Abs(colDif))
            {
                row += (rowDif > 0 ? -1 : 1);
            }
            else
            {
                col += (colDif > 0 ? -1 : 1);
            }
        }

        LevelElement? next = levelData.GetElementAtPosition(row, col);

        if (next is null)
        {
            Renderer.AddToRemoveList(Position);
            Position.Row = row;
            Position.Col = col;
        }
    }

    public override void Death(LevelData levelData, MessageLog messageLog, ICombatant killer)
    {
        base.Death(levelData, messageLog, killer);

        int modifierGain = 2;
        killer.AttackDice.Modifier += modifierGain;
        messageLog.AddMassage($"You eat {Name} and you feal more powerful");
    }
}
