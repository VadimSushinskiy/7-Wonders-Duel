using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _7_Wonders.Models
{
    public class PurpleCard : Card
    {
        public Guilds Guild { get; }

        private PurpleCard() { }

        public PurpleCard(byte epochNumber, BuildLink linkToBuy, BuildLink addedLink, Resources cost, Resources reward, short fame, string name, Guilds guild) 
            : base(epochNumber, linkToBuy, addedLink, cost, reward, fame, name)
        {
            Guild = guild;
        }

        public enum Guilds
        {
            Wonder,
            Gold,
            Green,
            Resources,
            Yellow,
            Blue,
            Red
        }

        public override void GetProfit(Player player)
        {
            base.GetProfit(player);
        }
    }
}
