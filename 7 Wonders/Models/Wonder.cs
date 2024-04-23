using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _7_Wonders.Models
{
    public class Wonder : Building
    {
        public short WarPoint { get; }
        public bool SecondTurn { get; }
        public ComplexResource Complex { get; }
        public WonderEffect Effect { get; }

        private Wonder() {}
        public Wonder(short warPoint, bool secondTurn, ComplexResource complex, WonderEffect effect, Resources cost, Resources reward, short fame, string name)
            : base(cost, reward, fame, name)
        {
            WarPoint = warPoint;
            SecondTurn = secondTurn;
            Complex = complex;
            Effect = effect;
        }

        public void GetProfit()
        {
            //TODO
        }

        public enum WonderEffect
        {
            None,
            StealGold,
            StealGray,
            StealBrown,
            TakeDiscard,
            Token
        }
    }
}
