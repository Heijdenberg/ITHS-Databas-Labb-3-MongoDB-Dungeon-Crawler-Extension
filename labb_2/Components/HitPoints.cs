using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace labb_2.Components
{
    internal class HitPoints
    {
        private int _hitPoints;
        private int _maxHitPoints;
        public HitPoints(int hitPoints)
        {
            _hitPoints = hitPoints;
            _maxHitPoints = hitPoints;
        }

        public int HP
        {
            get { return _hitPoints; }
            set
            {
                if (value > _maxHitPoints)
                {
                    _hitPoints = _maxHitPoints;
                }
                else if (value < 0)
                {
                    _hitPoints = 0;
                }
                else
                {
                    _hitPoints = value;
                }
            }

        }
    }
}
