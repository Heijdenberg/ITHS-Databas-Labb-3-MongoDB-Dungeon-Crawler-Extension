using ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.Components;
using ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.Core;
using ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.Interfaces;
using ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.UI;
using ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.Elements;

internal abstract class Enemy : LevelElement, ICombatant
{
    public Enemy(string name, int hp, Dice attackDice, Dice defenceDice, char sprite, ConsoleColor color, int y, int x)
        : base(sprite, color, y, x)
    {
        Name = name;
        HitPoints = new HitPoints(hp);
        AttackDice = attackDice;
        DefenceDice = defenceDice;
    }

    public string Name { get; }
    public HitPoints HitPoints { get; set; }
    public Dice AttackDice { get; }
    public Dice DefenceDice { get; }

    public override void Draw()
    {
        base.Draw();
    }
    public virtual void Draw(Player player)
    {
        if (GameMath.IsWithinRange(Position, player.Position, player.VisionRange))
        {
            base.Draw();
        }
        else
        {
            Renderer.AddToRemoveList(Position);
        }
    }
    public abstract void Update(LevelData levelData, MessageLog messageLog, Player player);

    public virtual void Death(LevelData leveldata, MessageLog messageLog, ICombatant killer)
    {
        messageLog.AddMassage($"{Name} is dead, slayed by {killer.Name}");
        leveldata.removeElement(Position.Y, Position.X);
        Renderer.AddToRemoveList(Position);
    }
}
