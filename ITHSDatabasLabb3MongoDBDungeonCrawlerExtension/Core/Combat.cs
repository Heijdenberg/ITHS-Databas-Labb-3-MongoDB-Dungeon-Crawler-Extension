using ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.Interfaces;
using ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.UI;

namespace ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.Core;

internal class Combat
{
    public Combat(ICombatant attacker, ICombatant defender)
    {
        Attacker = attacker;
        Defender = defender;
    }
    private ICombatant Attacker { get; }
    private ICombatant Defender { get; }
    public void Battle(MessageLog messageLog, LevelData levelData)
    {
        int damage = Attack(Attacker.AttackDice, Defender.DefenceDice);

        if (damage < 0)
        {
            damage = 0;
        }

        Defender.HitPoints.HP -= damage;

        messageLog.AddMassage($"{Attacker.Name} attacks {Defender.Name} for {damage} damage.");

        if (Defender.HitPoints.HP <= 0)
        {
            Defender.Death(levelData, messageLog, Attacker);
            return;
        }

        int counterDamage = Attack(Defender.AttackDice, Attacker.DefenceDice);

        if (counterDamage < 0)
        {
            counterDamage = 0;
        }
        Attacker.HitPoints.HP -= counterDamage;

        messageLog.AddMassage($"{Defender.Name} Counter attacks {Attacker.Name} for {counterDamage} damage.");
        if (Attacker.HitPoints.HP <= 0)
        {
            Attacker.Death(levelData, messageLog, Defender);
            return;
        }
    }

    private int Attack(Dice atkDice, Dice defDice)
    {
        int damage = atkDice.Throw() - defDice.Throw();
        if (damage <= 0)
        {
            damage = 0;
        }

        return damage;
    }
}
