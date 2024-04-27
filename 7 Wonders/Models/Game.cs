using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace _7_Wonders.Models
{
    public static class Game
    {
        public static Player FirstPlayer { get; private set; }
        public static Player SecondPlayer { get; private set; }
        public static Player CurrentPlayer { get; set; }
        public static Player Winner { get; set; }
        public static GameEnding Ending { get; set; }
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
            List<Token> tokens;
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
                new BlueCard(2, BuildLink.Water, BuildLink.None, new Resources {Stone = 3}, new Resources(), 5, "Aqueduct"),
                new RedCard(2, BuildLink.Sword, BuildLink.None, new Resources {Gold = 3}, new Resources(), 0, "Barracks", 1),
                new YellowCard(2, BuildLink.None, BuildLink.Barrel, new Resources(), new Resources {Gold = 6}, 0, "Brewery", ComplexResource.None),
                new BrownCard(2, BuildLink.None, BuildLink.None, new Resources {Gold = 2}, new Resources {Brick = 2}, 0, "BrickWorkshop"),
                new YellowCard(2, BuildLink.None, BuildLink.None, new Resources {Gold = 2, Glass = 1, Paper = 1 }, new Resources(), 0, "Caravanserai", ComplexResource.BrownResource),
                new BrownCard(2, BuildLink.None, BuildLink.None, new Resources {Gold = 2}, new Resources {Stone = 2}, 0, "Career"),
                new YellowCard(2, BuildLink.None, BuildLink.None, new Resources {Gold = 4}, new Resources(), 0, "Customs", Sale.PaperAndGlassSale),
                new GreenCard(2, BuildLink.Gear, BuildLink.None, new Resources {Stone = 1, Brick = 2}, new Resources(), 2, "Dispensary", ScienceSymbol.Pounder),
                new GrayCard(2, BuildLink.None, BuildLink.None, new Resources(), new Resources {Paper = 1}, 0, "DryingRoom"),
                new YellowCard(2, BuildLink.None, BuildLink.None, new Resources {Gold = 3, Brick = 1}, new Resources(), 0, "Forum", ComplexResource.GrayResource),
                new GrayCard(2, BuildLink.None, BuildLink.None, new Resources(), new Resources {Glass =1 }, 0, "Glassblower"),
                new RedCard(2, BuildLink.Horseshoe, BuildLink.None, new Resources {Brick = 1, Wood = 1}, new Resources(), 0, "HorseBreeders", 1),
                new GreenCard(2, BuildLink.None, BuildLink.Lamp, new Resources {Wood = 1, Glass = 2}, new Resources(), 1, "Laboratory", ScienceSymbol.Ruler),
                new GreenCard(2, BuildLink.Book, BuildLink.None, new Resources {Wood = 1, Stone = 1, Glass = 1}, new Resources(), 2, "Library", ScienceSymbol.Feather),
                new RedCard(2, BuildLink.None, BuildLink.Helmet, new Resources {Brick = 2, Glass = 1}, new Resources(), 0, "Parade", 2),
                new BrownCard(2, BuildLink.None, BuildLink.None, new Resources {Gold = 2}, new Resources {Wood = 2}, 0, "Sawmill"),
                new GreenCard(2, BuildLink.None, BuildLink.Harp, new Resources {Wood = 1, Paper = 2}, new Resources(), 1, "School", ScienceSymbol.Wheel),
                new RedCard(2, BuildLink.None, BuildLink.Target, new Resources {Wood = 1, Stone = 1, Paper = 1}, new Resources(), 0, "ShootingRange", 2),
                new BlueCard(2, BuildLink.Mask, BuildLink.Column, new Resources {Brick = 2}, new Resources(), 4, "Statue"),
                new BlueCard(2, BuildLink.Moon, BuildLink.Sun, new Resources {Wood = 1, Paper = 1}, new Resources(), 4, "Temple"),
                new BlueCard(2, BuildLink.None, BuildLink.None, new Resources {Wood = 2, Glass = 1}, new Resources(), 5, "Tribunal"),
                new BlueCard(2, BuildLink.None, BuildLink.Building, new Resources {Stone = 1, Wood = 1}, new Resources(), 4, "Tribune"),
                new RedCard(2, BuildLink.None, BuildLink.None, new Resources {Stone = 2}, new Resources(), 0, "Walls", 2)
            };
            AddCards(cards);
            cards = new List<Card>
            {
                new GreenCard(3,  BuildLink.None, BuildLink.None, new Resources {Stone = 1, Wood = 1, Glass = 2}, new Resources(), 3, "Academy", ScienceSymbol.Clock),
                new YellowCard(3, BuildLink.Barrel, BuildLink.None, new Resources {Brick = 1, Wood = 1, Stone = 1}, new Resources(), 3, "Arena", YellowCard.RewardTarget.Wonder),
                new YellowCard(3, BuildLink.None, BuildLink.None, new Resources {Stone = 2, Glass = 1}, new Resources(), 3, "Armory", YellowCard.RewardTarget.Red),
                new RedCard(3, BuildLink.None, BuildLink.None, new Resources {Wood = 2, Brick = 3}, new Resources(), 0, "Arsenal", 3),
                new BlueCard(3, BuildLink.None, BuildLink.None, new Resources {Wood = 1, Stone = 1, Brick = 1, Glass = 2}, new Resources(), 7, "Castle"),
                new YellowCard(3, BuildLink.None, BuildLink.None, new Resources {Paper = 2}, new Resources(), 3, "ChamberOfCommerce", YellowCard.RewardTarget.Gray),
                new RedCard(3, BuildLink.Helmet, BuildLink.None, new Resources {Brick = 2, Stone = 2}, new Resources(), 0, "Circus", 2),
                new RedCard(3, BuildLink.None, BuildLink.None, new Resources {Gold = 8}, new Resources(), 0, "Court", 3),
                new RedCard(3, BuildLink.Tower, BuildLink.None, new Resources {Stone = 2, Brick = 1, Paper = 1}, new Resources(), 0, "Fortification", 2),
                new BlueCard(3, BuildLink.Column, BuildLink.None, new Resources {Brick = 2, Wood = 2}, new Resources(), 6, "Gardens"),
                new YellowCard(3, BuildLink.Jug, BuildLink.None, new Resources {Brick = 2, Glass = 1}, new Resources(), 3, "Lighthouse", YellowCard.RewardTarget.Yellow),
                new BlueCard(3, BuildLink.None, BuildLink.None, new Resources {Stone = 2, Glass = 1}, new Resources(), 5, "Obelisk"),
                new GreenCard(3, BuildLink.Lamp, BuildLink.None, new Resources {Stone = 3, Paper = 2}, new Resources(), 2, "Observatories", ScienceSymbol.Globe),
                new BlueCard(3, BuildLink.Sun, BuildLink.None, new Resources {Wood = 1, Brick = 1, Paper = 2}, new Resources(), 6, "Pantheon"),
                new YellowCard(3, BuildLink.None, BuildLink.None, new Resources {Wood = 1, Glass = 1, Paper = 1}, new Resources(), 3, "Port", YellowCard.RewardTarget.Brown),
                new RedCard(3, BuildLink.Target, BuildLink.None, new Resources {Wood = 3, Glass = 1}, new Resources(), 0, "SedimentationWorkshop", 2),
                new BlueCard(3, BuildLink.Building, BuildLink.None, new Resources {Stone = 1, Brick = 2, Paper = 1}, new Resources(), 5, "Senate"),
                new GreenCard(3, BuildLink.None, BuildLink.None, new Resources {Wood = 2, Glass = 1, Paper = 1}, new Resources(), 3, "Studios", ScienceSymbol.Clock),
                new BlueCard(3, BuildLink.None, BuildLink.None, new Resources {Wood = 2, Stone = 3}, new Resources(), 7, "TownHall"),
                new GreenCard(3, BuildLink.Harp, BuildLink.None, new Resources {Brick = 1, Glass = 1, Paper = 1}, new Resources(), 2, "University", ScienceSymbol.Globe)
            };
            Shuffle(cards);
            cards.RemoveAt(19);
            cards.RemoveAt(18);
            cards.RemoveAt(17);
            List<Card> guilds = new()
            {
                new PurpleCard(3, BuildLink.None, BuildLink.None, new Resources {Stone = 2, Wood = 1, Brick = 1, Glass = 1}, new Resources(), 0, "BuildersGuild", PurpleCard.Guild.Wonder),
                new PurpleCard(3, BuildLink.None, BuildLink.None, new Resources {Wood = 2, Brick = 1, Paper = 1}, new Resources(), 0, "JudgesGuild", PurpleCard.Guild.Blue),
                new PurpleCard(3, BuildLink.None, BuildLink.None, new Resources {Wood = 1, Brick = 1, Glass = 1, Paper = 1}, new Resources(), 0, "MerchantsGuild", PurpleCard.Guild.Yellow),
                new PurpleCard(3, BuildLink.None, BuildLink.None, new Resources {Wood = 2, Stone = 2}, new Resources(), 0, "MoneylendersGuild", PurpleCard.Guild.Gold),
                new PurpleCard(3, BuildLink.None, BuildLink.None, new Resources {Wood = 2, Brick = 2}, new Resources(), 0, "ScientistsGuild", PurpleCard.Guild.Green),
                new PurpleCard(3, BuildLink.None, BuildLink.None, new Resources {Brick = 1, Stone = 1, Glass = 1, Paper = 1}, new Resources(), 0, "ShipownersGuild", PurpleCard.Guild.Resources),
                new PurpleCard(3, BuildLink.None, BuildLink.None, new Resources {Brick = 1, Stone = 2, Paper = 1}, new Resources(), 0, "WarlordsGuild", PurpleCard.Guild.Red)
            };
            Shuffle(guilds);
            guilds.RemoveRange(3, 4);
            cards.AddRange(guilds);
            Shuffle(cards);
            CardsList.AddRange(cards);
            WondersList = new List<Wonder>
            {
                new Wonder(1, false, ComplexResource.None, Wonder.WonderEffect.StealGray, new Resources { Wood = 1, Stone = 2, Glass = 1}, new Resources(), 3, "CircusMaximus", "Негайно отримайте 1 Щит та 3 Слави. Ваш суперник скине обрану вами сіру карту" ),
                new Wonder(0, true, ComplexResource.GrayResource, Wonder.WonderEffect.None, new Resources {Wood = 2, Stone = 1, Brick = 1}, new Resources(), 2, "Piraeus", "Негайно отримайте 2 Слави та складний ресурс Папір/Скло. Зробіть ще один хід"),
                new Wonder(0, true, ComplexResource.None, Wonder.WonderEffect.StealGold, new Resources { Stone = 2, Brick = 2, Paper = 1}, new Resources {Gold = 3}, 3, "TheAppianWay", "Негайно отримайте 3 Слави та 3 Золота. Ваш суперник втрачає до 3 Золота. Зробіть ще один хід"),
                new Wonder(2, false, ComplexResource.None, Wonder.WonderEffect.None, new Resources {Brick = 3, Glass = 1}, new Resources(), 3, "TheColossus", "Негайно отримайте 3 Слави та 2 Щита"),
                new Wonder(0, false, ComplexResource.None, Wonder.WonderEffect.Token, new Resources {Wood = 3, Paper = 1, Glass = 1}, new Resources(), 4, "TheGreatLibrary", "Негайно отримайте 4 Слави. Оберіть один з 3 Жетонів, які не приймають участь в грі"),
                new Wonder(0, false, ComplexResource.BrownResource, Wonder.WonderEffect.None, new Resources {Wood = 1, Stone = 1, Paper = 2}, new Resources(), 4, "TheGreatLighthouse", "Негайно отримайте 4 Слави та складний ресурс Дерево/Камінь/Цегла"),
                new Wonder(0, true, ComplexResource.None, Wonder.WonderEffect.None, new Resources {Wood = 2, Glass= 1, Paper = 1}, new Resources {Gold = 6}, 3, "TheHangingGardens", "Негайно отримайте 3 Слави та 6 Золота. Зробіть ще один хід"),
                new Wonder(0, false, ComplexResource.None, Wonder.WonderEffect.TakeDiscard, new Resources {Brick = 2, Glass = 2, Paper = 1}, new Resources(), 2, "TheMausoleum", "Негайно отримайте 2 Слави. Візьміть одну з скинутих під час гри карт"),
                new Wonder(0, false, ComplexResource.None, Wonder.WonderEffect.None, new Resources {Stone = 3, Paper = 1}, new Resources(), 9, "ThePyramids", "Негайно отримайте 9 Слави"),
                new Wonder(0, true, ComplexResource.None, Wonder.WonderEffect.None, new Resources {Stone = 1, Brick = 1, Glass = 2}, new Resources(), 6, "TheSphinx", "Негайно отримайте 6 Слави. Зробіть ще один хід"),
                new Wonder(1, false, ComplexResource.None, Wonder.WonderEffect.StealBrown, new Resources {Wood = 1, Stone = 1, Brick = 1, Paper = 2}, new Resources(), 3, "TheStatueOfZeus", "Негайно отримайте 3 Слави та 1 Щит. Ваш суперник скине обрану вами коричневу карту"),
                new Wonder(0, true, ComplexResource.None, Wonder.WonderEffect.None, new Resources {Wood = 1, Stone = 1, Glass = 1, Paper = 1}, new Resources {Gold = 12}, 0, "TheTempleOfArtemis", "Негайно отримайте 12 Золота. Зробіть ще один хід")
            };
            Shuffle(WondersList);
            for (int i = 0; i < 4;  i++)
            {
                FirstPlayer.Wonders[WondersList[i]] = false;
                SecondPlayer.Wonders[WondersList[i + 4]] = false;
            }
            tokens = new List<Token>
            {
                new Token(6, 4, ScienceSymbol.None, Token.TokenEffect.None, "Agriculture", "Негайно отримайте 6 Золота та 4 Слави"),
                new Token(0, 0, ScienceSymbol.None, Token.TokenEffect.Economy, "Economy", "Кожен раз, як ваш суперник буде витрачати Золото для покупки ресурсів - він буде віддавати це Золото вам"),
                new Token(0, 0, ScienceSymbol.Wage, Token.TokenEffect.None, "Law", "Отримайте Науковий символ"),
                new Token(0, 0, ScienceSymbol.None, Token.TokenEffect.Math, "Mathematics", "В кінці гри отримайте 3 Слави за кожен Жетон, включаючи цей"),
                new Token(0, 7, ScienceSymbol.None, Token.TokenEffect.None, "Philosophy", "Негайно отримайте 7 Слави"),
                new Token(0, 0, ScienceSymbol.None, Token.TokenEffect.Strategy, "Strategy", "Кожен раз, будуючи військову будівлю(купляючи червоні карти), ви будете отримувати на 1 щит більше"),
                new Token(0, 0, ScienceSymbol.None, Token.TokenEffect.Theology, "Theology", "Кожне ваше Диво буде мати ефект двійного ходу. Не впливає на ті дива, які вже мають двійний хід"),
                new Token(6, 0, ScienceSymbol.None, Token.TokenEffect.Urbanism, "Urbanism", "Негайно отримайте 6 Золота. Кожен раз, як ви будете отримувати карту по зв'язку - ви будете негайно отримувати 4 Золота")
            };
            Shuffle(tokens);
            GameTokens = tokens[..5];
            AdditionalTokens = tokens[5..];
            DiscardedCards = new List<Card>();
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
                {CardsList[20], new List<Card> {CardsList[26]} }, {CardsList[21], new List<Card> {CardsList[26], CardsList[27] } },
                {CardsList[22], new List<Card> {CardsList[27], CardsList[28] } }, {CardsList[23], new List<Card> {CardsList[28], CardsList[29] } },
                {CardsList[24], new List<Card> {CardsList[29], CardsList[30] } }, {CardsList[25], new List<Card> {CardsList[30] } },
                {CardsList[26], new List<Card> {CardsList[31]} }, {CardsList[27], new List<Card> {CardsList[31], CardsList[32] } },
                {CardsList[28], new List<Card> {CardsList[32], CardsList[33] } }, {CardsList[29], new List<Card> {CardsList[33], CardsList[34] } },
                {CardsList[30], new List<Card> {CardsList[34] } }, {CardsList[31], new List<Card> {CardsList[35] } },
                {CardsList[32], new List<Card> {CardsList[35], CardsList[36] } }, {CardsList[33], new List<Card> {CardsList[36], CardsList[37] } },
                {CardsList[34], new List<Card> {CardsList[37]} }, {CardsList[35], new List<Card> {CardsList[38]} },
                {CardsList[36], new List<Card> {CardsList[38], CardsList[39] } }, {CardsList[37], new List<Card> {CardsList[39] } },
                {CardsList[38], new List<Card>() }, {CardsList[39], new List<Card>() },
                {CardsList[40], new List<Card> {CardsList[42], CardsList[43] } }, {CardsList[41], new List<Card> {CardsList[43], CardsList[44] } },
                {CardsList[42], new List<Card> {CardsList[45], CardsList[46] } }, {CardsList[43], new List<Card> {CardsList[46], CardsList[47] } },
                {CardsList[44], new List<Card> {CardsList[47], CardsList[48] } }, {CardsList[45], new List<Card> {CardsList[49] } },
                {CardsList[46], new List<Card> {CardsList[49] } }, {CardsList[47], new List<Card> {CardsList[50] } },
                {CardsList[48], new List<Card> {CardsList[50] } }, {CardsList[49], new List<Card> {CardsList[51], CardsList[52] } },
                {CardsList[50], new List<Card> {CardsList[53], CardsList[54] } }, {CardsList[51], new List<Card> {CardsList[55] } },
                {CardsList[52], new List<Card> {CardsList[55], CardsList[56] } }, {CardsList[53], new List<Card> {CardsList[56], CardsList[57] } },
                {CardsList[54], new List<Card> {CardsList[57] } }, {CardsList[55], new List<Card> {CardsList[58] } },
                {CardsList[56], new List<Card> {CardsList[58], CardsList[59] } }, {CardsList[57], new List<Card> {CardsList[59] } },
                {CardsList[58], new List<Card>() }, {CardsList[59], new List<Card>() },

            };
            for (int i = 14; i < 20; i++)
            {
                CardsList[i].IsAvailable = true;
            }
            CardsList[38].IsAvailable = true;
            CardsList[39].IsAvailable = true;
            CardsList[58].IsAvailable = true;
            CardsList[59].IsAvailable = true;
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

        public static void WarPunish()
        {
            if (WarPoints >= 3 && !SecondPlayer.FirstDefeat)
            {
                SecondPlayer.Resource.Gold = (SecondPlayer.Resource.Gold >= 2) ? (short)(SecondPlayer.Resource.Gold - 2) : (short)0;
                SecondPlayer.FirstDefeat = true;
            }
            if (WarPoints >= 6 && !SecondPlayer.SecondDefeat)
            {
                SecondPlayer.Resource.Gold = (SecondPlayer.Resource.Gold >= 5) ? (short)(SecondPlayer.Resource.Gold - 5) : (short)0;
                SecondPlayer.SecondDefeat = true;
            }
            if (WarPoints <= -3 && !FirstPlayer.FirstDefeat)
            {
                FirstPlayer.Resource.Gold = (FirstPlayer.Resource.Gold >= 2) ? (short)(FirstPlayer.Resource.Gold - 2) : (short)0;
                FirstPlayer.FirstDefeat = true;
            }
            if (WarPoints <= -6 && !FirstPlayer.SecondDefeat)
            {
                FirstPlayer.Resource.Gold = (FirstPlayer.Resource.Gold >= 5) ? (short)(FirstPlayer.Resource.Gold - 5) : (short)0;
                FirstPlayer.SecondDefeat = true;
            }
        }

        public static void ChangeEpoch()
        {
            CardNumber = 20;
            Epoch++;
            if (Epoch == 4)
            {
                End(GameEnding.Standard);
            }
        }

        public static void CalculateWinningPoints(Player player)
        {
            int points = 0;
            points += player.Fame;
            if (player == FirstPlayer)
            {
                if (WarPoints >= 6)
                {
                    points += 10;
                }
                else if (WarPoints >= 3)
                {
                    points += 5;
                }
                else if (WarPoints >= 1)
                {
                    points += 2;
                }
            }
            else
            {
                if (WarPoints <= -6)
                {
                    points += 10;
                }
                else if (WarPoints <= -3)
                {
                    points += 5;
                }
                else if (WarPoints <= -1)
                {
                    points += 2;
                }
            }
            points += player.Resource.Gold / 3;
            if (player.TokenEffects[Token.TokenEffect.Math]) points += player.Tokens.Count * 3;
            if (player.Guilds[PurpleCard.Guild.Wonder]) points += 2 * Math.Max(player.WondersCount, player.Opponent.WondersCount);
            if (player.Guilds[PurpleCard.Guild.Gold]) points += Math.Max(player.Resource.Gold / 3, player.Opponent.Resource.Gold / 3);
            if (player.Guilds[PurpleCard.Guild.Green]) points += Math.Max(player.GreenCards.Count, player.Opponent.GreenCards.Count);
            if (player.Guilds[PurpleCard.Guild.Red]) points += Math.Max(player.RedCards.Count, player.Opponent.RedCards.Count);
            if (player.Guilds[PurpleCard.Guild.Yellow]) points += Math.Max(player.YellowCards.Count, player.Opponent.YellowCards.Count);
            if (player.Guilds[PurpleCard.Guild.Blue]) points += Math.Max(player.BlueCards.Count, player.Opponent.BlueCards.Count);
            if (player.Guilds[PurpleCard.Guild.Resources]) points += Math.Max(player.BrownCards.Count + player.GrayCards.Count, player.Opponent.BrownCards.Count + player.Opponent.GrayCards.Count);
            player.WinningPoints = points;
            
        }

        public static void End(GameEnding ending)
        {
            Ending = ending;
            switch (ending)
            {
                case GameEnding.Science:
                {
                    int countSymbols = 0;
                    for (int i = 0; i < FirstPlayer.Symbols.Length; i++)
                    {
                        if (FirstPlayer.Symbols[i]) countSymbols++; 
                    }
                    if (countSymbols >= 6)
                    {
                        Winner = FirstPlayer;
                    }
                    else
                    {
                        Winner = SecondPlayer;
                    }
                    break;
                }
                case GameEnding.War:
                {
                    if (WarPoints >= 9)
                    {
                        Winner = FirstPlayer;
                    }
                    else
                    {
                        Winner = SecondPlayer;
                    }
                    break;
                }
                case GameEnding.Standard:
                    {
                        CalculateWinningPoints(FirstPlayer);
                        CalculateWinningPoints(SecondPlayer);
                        if (FirstPlayer.WinningPoints > SecondPlayer.WinningPoints) Winner = FirstPlayer;
                        else if (SecondPlayer.WinningPoints > FirstPlayer.WinningPoints) Winner = SecondPlayer;
                        break;
                    }
            }
            Epoch = 4;
        }

        public enum GameEnding
        {
            Standard,
            War,
            Science
        }
    }
}
