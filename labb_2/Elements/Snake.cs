using labb_2.Core;
using labb_2.Interfaces;
using labb_2.UI;
using labb_2.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace labb_2.Elements;

internal class Snake : Enemy, IPlayerAwareDrawable
{
    public Snake(int y, int x)
        : base(name: "Snake",
            hp: 25,
            attackDice: new Dice(3, 4, 2),
            defenceDice: new Dice(1, 8, 5),
            sprite: 's',
            color: ConsoleColor.Green,
            y, x)
    { }
    public override void Update(LevelData levelData, MessageLog messageLog, Player player)
    {
        if (HitPoints.HP > 0)
        {
            int y = Position.Y;
            int x = Position.X;

            int yDif = player.Position.Y - y;
            int xDif = player.Position.X - x;

            if (GameMath.IsWithinRange(Position, player.Position, 2.0))
            {
                if (Math.Abs(yDif) == Math.Abs(xDif))
                {
                    int randomDirection = GameRandom.Random.Next(0, 2);
                    if (randomDirection == 0)
                    {
                        if (yDif > 0.0)
                        {
                            y--;
                        }
                        else
                        {
                            y++;
                        }
                    }
                    else
                    {
                        if (xDif > 0.0)
                        {
                            x--;
                        }
                        else
                        {
                            x++;
                        }
                    }
                }
                else if (Math.Abs(yDif) > Math.Abs(xDif))
                {
                    if (yDif > 0.0)
                    {
                        y--;
                    }
                    else
                    {
                        y++;
                    }
                }
                else
                {
                    if (xDif > 0.0)
                    {
                        x--;
                    }
                    else
                    {
                        x++;
                    }
                }
            }

            LevelElement? nextPostionInhabitant = levelData.GetElementAtPosition(y, x);

            if (nextPostionInhabitant == null)
            {
                Renderer.AddToRemoveList(Position);
                Position.Y = y;
                Position.X = x;
            }
        }
    }

    public override void Death(LevelData leveldata, MessageLog messageLog, ICombatant killer)
    {
        base.Death(leveldata, messageLog, killer);

        int modifierGain = 2;
        killer.AttackDice.Modifier = modifierGain;
        messageLog.AddMassage($"You eat {Name} and you feal more powerful");
    }
}
