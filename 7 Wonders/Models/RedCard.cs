using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _7_Wonders.Models
{
    public class RedCard : Card
    {
        public short WarPoint { get; private set; }

        private RedCard() {}
        public RedCard(byte epochNumber, BuildLink linkToBuy, BuildLink addedLink, Resources cost, Resources reward, short fame, string name, short warPoint)
        : base(epochNumber, linkToBuy, addedLink, cost, reward, fame, name)
        {
            WarPoint = (warPoint >= 0) ? warPoint : (short)0;
        }
        public override void GetProfit(Player player)
        {
            //TODO
            base.GetProfit(player);
        }
    }
}
