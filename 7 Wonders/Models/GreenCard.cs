using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _7_Wonders.Models
{
    public class GreenCard : Card
    {
        public ScienceSymbol Symbol { get; private set; }

        private GreenCard() { }
        public GreenCard(byte epochNumber, BuildLink linkToBuy, BuildLink addedLink, Resources cost, Resources reward, short fame, string name, ScienceSymbol symbol)
        : base(epochNumber, linkToBuy, addedLink, cost, reward, fame, name)
        {
            Symbol = symbol;
        }

        public override void GetProfit(Player player)
        {
            //TODO
            base.GetProfit(player);
        }
    }
}
