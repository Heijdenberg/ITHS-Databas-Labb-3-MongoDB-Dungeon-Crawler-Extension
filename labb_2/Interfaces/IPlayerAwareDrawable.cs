using labb_2.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace labb_2.Interfaces;

internal interface IPlayerAwareDrawable
{
    public void Draw(Player player);
}
