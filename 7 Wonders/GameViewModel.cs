using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using _7_Wonders.Models;

namespace _7_Wonders
{
    class GameViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        
        public RelayCommand ShuffleCommand { get; set; }
        public RelayCommand BuyCommand { get; set; }
        public RelayCommand SellCommand { get; set; }

        public int[] BackCards = {2, 3, 4, 9, 10, 11, 12, 13};
        public ObservableCollection<string> CardsName { get; } = new();

        public ObservableCollection<Visibility> CardsVisibility { get; } = new();

        private Resources _firstResources;

        public Resources FirstResource
        {
            get => _firstResources;
            set
            {
                _firstResources = value;
                OnPropertyChanged(nameof(FirstResource));
            }
        }
        private Resources _secondResources;

        public Resources SecondResources
        {
            get => _secondResources;
            set
            {
                _secondResources = value;
                OnPropertyChanged(nameof(SecondResources));
            }
        }

        private int _war;

        public int War
        {
            get => _war;
            set
            {
                _war = value;
                OnPropertyChanged(nameof(War));
            }
        }

        public string[] Backs = { @"Images\Cards\BackFirstEpoch.png", @"Images\Cards\BackSecondEpoch.png" , @"Images\Cards\BackThirdEpoch.png" };

        public GameViewModel()
        {
            for (int i = 0; i < 20; i++)
            {
                CardsVisibility.Add(Visibility.Visible);
                CardsName.Add(@"Images\Cards\BackFirstEpoch.png");
            }
            ShuffleCommand = new RelayCommand(obj =>
            {
                Game.InitializeGame();
                for (int i = 0; i < Game.CardsList.Count; i++)
                {
                    if (BackCards.Contains(i)) continue;
                    CardsName[i] = @"Images\Cards\" + Game.CardsList[i].Name + ".png";
                }

                War = 0;
                FirstResource = Game.FirstPlayer.Resource;
                SecondResources = Game.SecondPlayer.Resource;
                OnPropertyChanged(nameof(CardsName));
            });

            BuyCommand = new RelayCommand(obj =>
            {
                int idx = int.Parse((string)obj);
                Game.CurrentPlayer.BuyCard(Game.CardsList[idx]);
                CardsVisibility[idx] = Visibility.Collapsed;
                FirstResource = Game.FirstPlayer.Resource;
                SecondResources = Game.SecondPlayer.Resource;
                if (War != Game.WarPoints)
                {
                    War = Game.WarPoints;
                }
                CheckBacks();
                OnPropertyChanged(nameof(CardsVisibility));
            }, obj =>
            {
                Card card = Game.CardsList[int.Parse((string)obj)];
                return (card.IsAvailable && Game.CurrentPlayer.CheckPrice(card) != -1);
            });

            SellCommand = new RelayCommand(obj =>
            {
                int idx = int.Parse((string)obj);
                Game.CurrentPlayer.SellCard(Game.CardsList[idx]);
                CardsVisibility[idx] = Visibility.Collapsed;
                FirstResource = Game.FirstPlayer.Resource;
                SecondResources = Game.SecondPlayer.Resource;
                OnPropertyChanged(nameof(CardsVisibility));
                CheckBacks();
            }, obj =>
            {
                Card card = Game.CardsList[int.Parse((string)obj)];
                return (card.IsAvailable);
            });
        }

        public void CheckBacks()
        {
            bool flag = false;
            for (int i = 0; i < Game.CardsList.Count; i++)
            {
                if (Backs.Contains(CardsName[i]) && Game.CardsList[i].IsAvailable)
                {
                    CardsName[i] = @"Images\Cards\" + Game.CardsList[i].Name + ".png";
                    flag = true;
                }
            }
            if (flag) OnPropertyChanged(nameof(CardsName));
        }

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
