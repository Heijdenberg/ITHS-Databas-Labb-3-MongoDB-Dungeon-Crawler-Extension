using ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.Components;
using ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.Core;
using ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.UI;

namespace ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.Interfaces;

internal interface ICombatant
{
    string Name { get; }
    HitPoints HitPoints { get; set; }
    Dice AttackDice { get; }
    Dice DefenceDice { get; }
    public void Death(LevelData leveldata, MessageLog messageLog, ICombatant killer);
}
