using ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.Interfaces;

internal interface IPlayerAwareDrawable
{
    public void Draw(Player player);
}
