using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents.DocumentStructures;

namespace _7_Wonders.Models
{
    public class Player
    {
        public string Name { get; set; }
        public Resources Resource { get; set; }
        public bool[] Symbols { get; set; }
        public short Fame { get; set; }
        public int WondersCount { get; set; }
        public List<RedCard> RedCards { get; set; }
        public List<BlueCard> BlueCards { get; set; }
        public List<GreenCard> GreenCards { get; set; }
        public List<BrownCard> BrownCards { get; set; }
        public List<GrayCard> GrayCards { get; set; }
        public List<YellowCard> YellowCards { get; set; }
        public List<PurpleCard> PurpleCards { get; set; }
        public Dictionary<Sale, bool> Sales { get; set; }
        public Dictionary<ComplexResource, byte> ComplexResources { get; set; }
        public Dictionary<BuildLink, bool> BuildLinks { get; set; }
        public List<Token> Tokens { get; set; }
        public Dictionary<Wonder, bool> Wonders { get; set; }
        public Player Opponent { get; set; }
        public bool IsEconomy { get; set; }
        public bool IsStrategy { get; set; }
        public bool IsTheology { get; set; }
        public bool IsUrbanism { get; set; }
        public bool FirstDefeat { get; set; }
        public bool SecondDefeat { get; set; }

        private Player() {}

        public Player(string name)
        {
            Name = name;
            Resource = new Resources() {Gold = 7};
            Symbols = new bool[7];
            Fame = 0;
            WondersCount = 0;
            RedCards = new List<RedCard>();
            BlueCards = new List<BlueCard>();
            GreenCards = new List<GreenCard>();
            BrownCards = new List<BrownCard>();
            GrayCards = new List<GrayCard>();
            YellowCards = new List<YellowCard>();
            PurpleCards = new List<PurpleCard>();
            Sales = new Dictionary<Sale, bool>
            {
                { Sale.BrickSale, false }, { Sale.StoneSale, false }, { Sale.WoodSale, false }, { Sale.PaperAndGlassSale, false }
            };
            ComplexResources = new Dictionary<ComplexResource, byte>
            {
                { ComplexResource.BrownResource, 0 }, { ComplexResource.GrayResource, 0 }
            };
            BuildLinks = new Dictionary<BuildLink, bool>();
            for (int i = 1; i < 18; i++)
            {
                BuildLinks[(BuildLink)i] = false;
            }
            Tokens = new List<Token>();
        }

        public int CheckPrice(Building building)
        {
            if (building is Card card)
            {
                if (card.LinkToBuy != BuildLink.None && BuildLinks[card.LinkToBuy]) return 0;
            }

            if (building is Wonder && !Game.WonderIsAvailable) return -1;
            if (building.Cost.Gold > Resource.Gold) return -1;
            Resources deficit = new Resources();
            deficit.Wood = (building.Cost.Wood - Resource.Wood > 0) ? (short)(building.Cost.Wood - Resource.Wood) : (short)0;
            deficit.Stone = (building.Cost.Stone - Resource.Stone > 0) ? (short)(building.Cost.Stone - Resource.Stone) : (short)0;
            deficit.Brick = (building.Cost.Brick - Resource.Brick > 0) ? (short)(building.Cost.Brick - Resource.Brick) : (short)0;
            deficit.Glass = (building.Cost.Glass - Resource.Glass > 0) ? (short)(building.Cost.Glass - Resource.Glass) : (short)0;
            deficit.Paper = (building.Cost.Paper - Resource.Paper > 0) ? (short)(building.Cost.Paper - Resource.Paper) : (short)0;
            if (deficit is { Wood: 0, Stone: 0, Brick: 0, Glass: 0, Paper: 0 } ||
                (deficit.Wood + deficit.Stone + deficit.Brick <= ComplexResources[ComplexResource.BrownResource] && deficit.Glass + deficit.Paper <= ComplexResources[ComplexResource.GrayResource]))
            {
                return building.Cost.Gold;
            }
            int price = CalculatePay(deficit);
            if (price > Resource.Gold) return -1;
            return price;
        }

        public int CalculatePay(Resources deficit)
        {
            int[] prices = {2, 2, 2, 2, 2};
            int res = 0;
            prices[0] += Opponent.Resource.Brick;
            prices[1] += Opponent.Resource.Stone;
            prices[2] += Opponent.Resource.Wood;
            prices[3] += Opponent.Resource.Paper;
            prices[4] += Opponent.Resource.Glass;
            for (int i = 0; i < 4; i++)
            {
                if (Sales[(Sale)(i + 1)])
                {
                    prices[i] = 1;
                    if (i == 3)
                    {
                        prices[4] = 1;
                    }
                }
            }

            for (int i = 0; i < ComplexResources[ComplexResource.BrownResource]; i++)
            {
                int max = 0, idx = 0;
                for (int j = 0; j < 3; j++)
                {
                    if (deficit[j] > 0 && prices[j] > max)
                    {
                        max = prices[j];
                        idx = j;
                    }
                }
                if (max == 0) break;
                deficit[idx]--;
            }
            for (int i = 0; i < ComplexResources[ComplexResource.GrayResource]; i++)
            {
                int max = 0, idx = 0;
                for (int j = 3; j < 5; j++)
                {
                    if (deficit[j] > 0 && prices[j] > max)
                    {
                        max = prices[j];
                        idx = j;
                    }
                }
                if (max == 0) break;
                deficit[idx]--;
            }

            for (int i = 0; i < 5; i++)
            {
                res += deficit[i] * prices[i];
            }

            return res;
        }

        public void BuyCard(Card card)
        {
            int price = CheckPrice(card);
            if (price == -1)
            {
                throw new Exception("You can't buy this card!");
            }
            Resource.Gold -= (short)price;
            if (Opponent.IsEconomy && price != 0)
            {
                Opponent.Resource.Gold += (short)(price - card.Cost.Gold);
            }
            card.GetProfit(this);
            DeleteCard(card);
            EndTurn();
        }

        public void SellCard(Card card)
        {
            Game.DiscardedCards.Add(card);
            Resource.Gold += (short)(2 + YellowCards.Count);
            DeleteCard(card);
            EndTurn();
        }

        public void BuildWonder(Wonder wonder, Card card)
        {
            int price = CheckPrice(wonder);
            if (price == -1)
            {
                throw new Exception("You can't build this wonder!");
            }

            Resource.Gold -= (short)price;
            wonder.GetProfit();
            DeleteCard(card);
            if (Game.CardNumber == 0 || (!wonder.SecondTurn && !IsTheology))
            {
                EndTurn();
            }
        }

        public void DeleteCard(Card card)
        {
            for (int i = 0; i < 20; i++)
            {
                if (Game.FirstEpochGraph[Game.FirstEpochCards[i]].Contains(card))
                {
                    Game.FirstEpochGraph[Game.FirstEpochCards[i]].Remove(card);
                    if (Game.FirstEpochGraph[Game.FirstEpochCards[i]].Count == 0)
                    {
                        Game.FirstEpochCards[i].IsAvailable = true;
                    }
                }
            }
            Game.CardNumber--;
        }
        public void EndTurn()
        {
            Game.CurrentPlayer = Opponent;
            if (Game.CardNumber == 0)
            {
                Game.ChangeEpoch();
            }
        }
    }
}
