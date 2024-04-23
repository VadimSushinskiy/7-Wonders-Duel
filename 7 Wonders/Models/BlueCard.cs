﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _7_Wonders.Models
{
    public class BlueCard : Card
    {
        private BlueCard() { }

        public BlueCard(byte epochNumber, BuildLink linkToBuy, BuildLink addedLink, Resources cost, Resources reward, short fame, string name)
            : base(epochNumber, linkToBuy, addedLink, cost, reward, fame, name)
        {

        }

        public override void GetProfit(Player player)
        {
            //TODO
            base.GetProfit(player);
        }
    }
}
