using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _7_Wonders.Models
{
    public class YellowCard : Card
    {
        public Sale Sale { get; private set; }
        public ComplexResource Complex { get; private set; }
        public RewardTarget Target { get; private set; }

        private YellowCard() { }
        public YellowCard(byte epochNumber, BuildLink linkToBuy, BuildLink addedLink, Resources cost, Resources reward, short fame, string name, Sale sale, ComplexResource complex, RewardTarget target)
            : base(epochNumber, linkToBuy, addedLink, cost, reward, fame, name)
        {
            Sale = sale;
            Complex = complex;
            Target = target;
        }
        public YellowCard(byte epochNumber, BuildLink linkToBuy, BuildLink addedLink, Resources cost, Resources reward, short fame, string name, Sale sale)
            : base(epochNumber, linkToBuy, addedLink, cost, reward, fame, name)
        {
            Sale = sale;
        }
        public YellowCard(byte epochNumber, BuildLink linkToBuy, BuildLink addedLink, Resources cost, Resources reward, short fame, string name, ComplexResource complex) 
            : base(epochNumber, linkToBuy, addedLink, cost, reward, fame, name)
        {
            Complex = complex;
        }
        public YellowCard(byte epochNumber, BuildLink linkToBuy, BuildLink addedLink, Resources cost, Resources reward, short fame, string name, RewardTarget target)
            : base(epochNumber, linkToBuy, addedLink, cost, reward, fame, name)
        {
            Target = target;
        }
        public override void GetProfit(Player player)
        {
            base.GetProfit(player);
            if (Sale != Sale.None)
            {
                player.Sales[Sale] = true;
            }

            if (Complex != ComplexResource.None)
            {
                player.ComplexResources[Complex]++;
            }

            if (Target != RewardTarget.None)
            {
                //TODO
            }

            player.YellowCards.Add(this);
        }

        public enum RewardTarget
        {
            None,
            Red,
            Gray,
            Brown,
            Yellow,
            Wonder
        }
    }
}
