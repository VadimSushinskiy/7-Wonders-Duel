using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _7_Wonders.Models
{
    public static class Game
    {
        public static Player FirstPlayer { get; private set; }
        public static Player SecondPlayer { get; private set; }
        public static Player CurrentPlayer { get; set; }
        public static int WarPoints { get; set; }
        public static byte Epoch { get; set; }
        public static int CardNumber { get; set; }
        public static List<Card> DiscardedCards { get; set; }
        public static List<Card> CardsList { get; set; }
        public static List<Wonder> WondersList { get; set; }
        public static Dictionary<Card, List<Card>> CardGraph { get; set; }
        public static List<Token> GameTokens { get; set; }
        public static List<Token> AdditionalTokens { get; set; }
        public static bool WonderIsAvailable { get; set; }
        public static Random rnd = new Random();

        public static void InitializeGame()
        {
            List<Card> cards;
            FirstPlayer = new Player("Player 1");
            SecondPlayer = new Player("Player 2");
            CurrentPlayer = FirstPlayer;
            FirstPlayer.Opponent = SecondPlayer;
            SecondPlayer.Opponent = FirstPlayer;
            WarPoints = 0;
            Epoch = 1;
            CardNumber = 20;
            CardsList = new List<Card>();
            cards = new List<Card>
            {
                new BlueCard(1, BuildLink.None, BuildLink.Moon, new Resources(), new Resources(), 3, "Altar"),
                new GreenCard(1, BuildLink.None, BuildLink.Gear, new Resources() { Gold = 2 }, new Resources(), 0, "Apothecary", ScienceSymbol.Pounder),
                new BlueCard(1, BuildLink.None, BuildLink.Water, new Resources() {Stone = 1}, new Resources(), 3, "Baths"),
                new GrayCard(1, BuildLink.None, BuildLink.None, new Resources() {Gold = 1}, new Resources() {Paper = 1}, 0, "BookPublishing"),
                new BrownCard(1, BuildLink.None, BuildLink.None, new Resources(), new Resources() {Brick = 1}, 0, "ClayMine"),
                new BrownCard(1, BuildLink.None, BuildLink.None, new Resources() {Gold = 1}, new Resources() {Brick = 1}, 0, "ClayQuarry"),
                new YellowCard(1, BuildLink.None, BuildLink.None, new Resources() {Gold = 3}, new Resources(), 0, "ClaySupply", Sale.BrickSale),
                new RedCard(1, BuildLink.None, BuildLink.Sword, new Resources() {Brick = 1}, new Resources(), 0, "Garrison", 1),
                new GrayCard(1, BuildLink.None, BuildLink.None, new Resources() {Gold = 1}, new Resources() {Glass = 1}, 0, "GlassWorkshop"),
                new BrownCard(1, BuildLink.None, BuildLink.None, new Resources() {Gold = 1}, new Resources() {Stone = 1}, 0, "GraniteQuarry"),
                new GreenCard(1, BuildLink.None, BuildLink.None, new Resources() {Glass = 1}, new Resources(), 1, "Herbalist", ScienceSymbol.Wheel),
                new BrownCard(1, BuildLink.None, BuildLink.None, new Resources() {Gold = 1}, new Resources() {Wood = 1}, 0, "LumberCamp"),
                new RedCard(1, BuildLink.None, BuildLink.Tower, new Resources() {Gold = 2}, new Resources(), 0, "Palisade", 1),
                new BrownCard(1, BuildLink.None, BuildLink.None, new Resources(), new Resources() {Stone = 1}, 0, "Quarry"),
                new GreenCard(1, BuildLink.None, BuildLink.Book, new Resources() {Gold = 2}, new Resources(), 0, "Scriptorium", ScienceSymbol.Feather),
                new RedCard(1, BuildLink.None, BuildLink.None, new Resources(), new Resources(), 0, "SecurityTower", 1),
                new RedCard(1, BuildLink.None, BuildLink.Horseshoe, new Resources() {Wood = 1}, new Resources(), 0, "Stable", 1),
                new YellowCard(1, BuildLink.None, BuildLink.None, new Resources() {Gold = 3}, new Resources(), 0, "StoneSupply", Sale.StoneSale),
                new YellowCard(1, BuildLink.None, BuildLink.Jug, new Resources(), new Resources() {Gold = 4}, 0, "Tavern", Sale.None),
                new BlueCard(1, BuildLink.None, BuildLink.Mask, new Resources(), new Resources(), 3, "Theater"),
                new BrownCard(1, BuildLink.None, BuildLink.None, new Resources(), new Resources() {Wood = 1}, 0, "TimberFelling"),
                new YellowCard(1, BuildLink.None, BuildLink.None, new Resources() {Gold = 3}, new Resources(), 0, "WoodSupply", Sale.WoodSale),
                new GreenCard(1, BuildLink.None, BuildLink.None, new Resources() {Paper = 1}, new Resources(), 1, "Workshop", ScienceSymbol.Ruler)
            };
            AddCards(cards);
            cards = new List<Card>
            {

            };
            //AddCards(cards);
            cards = new List<Card>
            {

            };
            //AddCards(cards);
            WondersList = new List<Wonder>
            {
                new Wonder(1, false, ComplexResource.None, Wonder.WonderEffect.StealGray, new Resources { Wood = 1, Stone = 2, Glass = 1}, new Resources(), 3, "CircusMaximus" ),
                new Wonder(0, true, ComplexResource.GrayResource, Wonder.WonderEffect.None, new Resources {Wood = 2, Stone = 1, Brick = 1}, new Resources(), 2, "Piraeus"),
                new Wonder(0, true, ComplexResource.None, Wonder.WonderEffect.StealGold, new Resources { Stone = 2, Brick = 2, Paper = 1}, new Resources {Gold = 3}, 3, "TheAppianWay"),
                new Wonder(2, false, ComplexResource.None, Wonder.WonderEffect.None, new Resources {Brick = 3, Glass = 1}, new Resources(), 3, "TheColossus"),
                new Wonder(0, false, ComplexResource.None, Wonder.WonderEffect.Token, new Resources {Wood = 3, Paper = 1, Glass = 1}, new Resources(), 4, "TheGreatLibrary"),
                new Wonder(0, false, ComplexResource.BrownResource, Wonder.WonderEffect.None, new Resources {Wood = 1, Stone = 1, Paper = 2}, new Resources(), 4, "TheGreatLighthouse"),
                new Wonder(0, true, ComplexResource.None, Wonder.WonderEffect.None, new Resources {Wood = 2, Glass= 1, Paper = 1}, new Resources {Gold = 6}, 3, "TheHangingGardens"),
                new Wonder(0, false, ComplexResource.None, Wonder.WonderEffect.TakeDiscard, new Resources {Brick = 2, Glass = 2, Paper = 1}, new Resources(), 2, "TheMausoleum"),
                new Wonder(0, false, ComplexResource.None, Wonder.WonderEffect.None, new Resources {Stone = 3, Paper = 1}, new Resources(), 9, "ThePyramids"),
                new Wonder(0, true, ComplexResource.None, Wonder.WonderEffect.None, new Resources {Stone = 1, Brick = 1, Glass = 2}, new Resources(), 6, "TheSphinx"),
                new Wonder(1, false, ComplexResource.None, Wonder.WonderEffect.StealBrown, new Resources {Wood = 1, Stone = 1, Brick = 1, Paper = 2}, new Resources(), 3, "TheStatueOfZeus"),
                new Wonder(0, true, ComplexResource.None, Wonder.WonderEffect.None, new Resources {Wood = 1, Stone = 1, Glass = 1, Paper = 1}, new Resources {Gold = 12}, 0, "TheTempleOfArtemis")
            };
            Shuffle(WondersList);
            for (int i = 0; i < 4;  i++)
            {
                FirstPlayer.Wonders[WondersList[i]] = false;
                SecondPlayer.Wonders[WondersList[i + 4]] = false;
            }
            DiscardedCards = new List<Card>();
            GameTokens = new List<Token>();
            AdditionalTokens = new List<Token>();
            WonderIsAvailable = true;


            CardGraph = new Dictionary<Card, List<Card>>
            {
                {CardsList[0], new List<Card> { CardsList[2], CardsList[3] }}, {CardsList[1], new List<Card> { CardsList[3], CardsList[4] }},
                {CardsList[2], new List<Card> { CardsList[5], CardsList[6] }}, {CardsList[3], new List<Card> { CardsList[6], CardsList[7] }},
                {CardsList[4], new List<Card> { CardsList[7], CardsList[8] }}, {CardsList[5], new List<Card> { CardsList[9], CardsList[10] }},
                {CardsList[6], new List<Card> { CardsList[10], CardsList[11] }}, {CardsList[7], new List<Card> { CardsList[11], CardsList[12] }},
                {CardsList[8], new List<Card> { CardsList[12], CardsList[13] }}, 
                {CardsList[9], new List<Card> { CardsList[14], CardsList[15] }}, {CardsList[10], new List<Card> { CardsList[15], CardsList[16] }},
                {CardsList[11], new List<Card> { CardsList[16], CardsList[17] }}, {CardsList[12], new List<Card> { CardsList[17], CardsList[18] }},
                {CardsList[13], new List<Card> { CardsList[18], CardsList[19] }}, {CardsList[14], new List<Card>()}, {CardsList[15], new List<Card>()},
                {CardsList[16], new List<Card>()}, {CardsList[17], new List<Card>()}, {CardsList[18], new List<Card>()}, {CardsList[19], new List<Card>()},

            };
            for (int i = 14; i < 20; i++)
            {
                CardsList[i].IsAvailable = true;
            }
        }

        public static void Shuffle<T>(List<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rnd.Next(n + 1);
                (list[k], list[n]) = (list[n], list[k]);
            }
        }

        public static void AddCards(List<Card> lst)
        {
            Shuffle(lst);
            lst.RemoveAt(22);
            lst.RemoveAt(21);
            lst.RemoveAt(20);
            CardsList.AddRange(lst);
        }

        public static void ChangeEpoch()
        {
            //TODO
        }

        public static void End(GameEnding ending)
        {
            //TODO
        }

        public enum GameEnding
        {
            Standard,
            War,
            Science
        }
    }
}
