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
                switch (Target)
                {
                    case RewardTarget.Red:
                    {
                        player.Resource.Gold += (short)player.RedCards.Count;
                        break;
                    }
                    case RewardTarget.Yellow:
                    {
                        player.Resource.Gold += (short)player.YellowCards.Count;
                        break;
                    }
                    case RewardTarget.Brown:
                    {
                        player.Resource.Gold += (short)(2 * player.BrownCards.Count);
                        break;
                    }
                    case RewardTarget.Gray:
                    {
                        player.Resource.Gold += (short)(3 * player.GrayCards.Count);
                        break;
                    }
                    case RewardTarget.Wonder:
                    {
                        player.Resource.Gold += (short)(2 * player.WondersCount);
                        break;
                    }
                }
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
