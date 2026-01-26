using ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.Components;
using ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.Core;
using ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.Interfaces;
using ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.UI;
using ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.Utilities;

namespace ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.Elements;

internal abstract class Enemy : LevelElement, ICombatant
{
    protected Enemy(string name, int hp, Dice attackDice, Dice defenceDice, char sprite, ConsoleColor color, int row, int col)
        : base(sprite, color, row, col)
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

    public virtual void Draw(Player player)
    {
        if (GameMath.IsWithinRange(Position, player.Position, player.VisionRange))
            base.Draw();
        else
            Renderer.AddToRemoveList(Position);
    }

    public abstract void Update(LevelData levelData, MessageLog messageLog, Player player);

    public virtual void Death(LevelData levelData, MessageLog messageLog, ICombatant killer)
    {
        messageLog.AddMassage($"{Name} is dead, slayed by {killer.Name}");
        levelData.RemoveElement(Position.Row, Position.Col);
        Renderer.AddToRemoveList(Position);
    }
}
