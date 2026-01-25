using ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.Components;
using ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.Core;
using ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.Interfaces;

internal interface ICombatant
{
    string Name { get; }
    HitPoints HitPoints { get; set; }
    Dice AttackDice { get; }
    Dice DefenceDice { get; }
    public void Death(LevelData leveldata, MessageLog messageLog, ICombatant killer);
}
