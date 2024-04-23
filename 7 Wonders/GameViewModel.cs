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


        public GameViewModel()
        {
            for (int i = 0; i < 20; i++)
            {
                CardsVisibility.Add(Visibility.Visible);
                CardsName.Add(@"Image\BackFirstEpoch.png");
            }
            ShuffleCommand = new RelayCommand(obj =>
            {
                Game.InitializeGame();
                for (int i = 0; i < Game.FirstEpochCards.Count; i++)
                {
                    CardsName[i] = @"Image\" + Game.FirstEpochCards[i].Name + ".png";
                }

                FirstResource = Game.FirstPlayer.Resource;
                SecondResources = Game.SecondPlayer.Resource;
                OnPropertyChanged(nameof(CardsName));
            });

            BuyCommand = new RelayCommand(obj =>
            {
                int idx = int.Parse((string)obj);
                Game.CurrentPlayer.BuyCard(Game.FirstEpochCards[idx]);
                CardsVisibility[idx] = Visibility.Collapsed;
                FirstResource = Game.FirstPlayer.Resource;
                SecondResources = Game.SecondPlayer.Resource;
                OnPropertyChanged(nameof(CardsVisibility));
            }, obj =>
            {
                Card card = Game.FirstEpochCards[int.Parse((string)obj)];
                return (card.IsAvailable && Game.CurrentPlayer.CheckPrice(card) != -1);
            });
        }
        

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
    
}
