using labb_2.Core;
using labb_2.Interfaces;
using labb_2.UI;
using labb_2.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace labb_2.Elements;

internal class Rat : Enemy, IPlayerAwareDrawable
{
    public Rat(int y, int x)
        : base(name: "rat",
            hp: 10,
            attackDice: new Dice(1, 6, 3),
            defenceDice: new Dice(1, 6, 1),
            sprite: 'r',
            color: ConsoleColor.Red,
            y, x)
    { }

    public override void Update(LevelData levelData, MessageLog messageLog, Player player)
    {
        if (HitPoints.HP > 0)
        {
            int y = Position.Y;
            int x = Position.X;
            int direction = GameRandom.Random.Next(0, 4);

            if (direction == 0)
            {
                x--;
            }
            else if (direction == 1)
            {
                y--;
            }
            else if (direction == 2)
            {
                x++;
            }
            else if (direction == 3)
            {
                y++;
            }

            LevelElement? nextPostionInhabitant = levelData.GetElementAtPosition(y, x);

            if (nextPostionInhabitant == null && (y != player.Position.Y || x != player.Position.X))
            {
                Renderer.AddToRemoveList(Position);
                Position.Y = y;
                Position.X = x;
            }
            else if (y == player.Position.Y && x == player.Position.X)
            {
                messageLog.AddMassage($"{Name} attacked player!");
                Combat combat = new(this, (ICombatant)player);
                combat.Battle(messageLog, levelData);
            }
        }
    }

    public override void Death(LevelData leveldata, MessageLog messageLog, ICombatant killer)
    {
        base.Death(leveldata, messageLog, killer);

        int hpDrop = 10;
        killer.HitPoints.HP += hpDrop;
        messageLog.AddMassage($"You eat {Name} and got {hpDrop}HP");
    }
}


