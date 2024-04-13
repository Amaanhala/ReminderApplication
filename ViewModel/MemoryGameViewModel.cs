using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace ReminderApplication.ViewModel
{
    public class MemoryGameViewModel : INotifyPropertyChanged
    {

        public TaskListViewModel TaskListViewModel { get; set; }

        public ObservableCollection<Card> Cards { get; set; }

        public ICommand CardTappedCommand { get; set; }

        private Card _previousCard;

        private bool _isVisible;
        public bool IsVisible
        {
            get => _isVisible;
            set
            {
                _isVisible = value;
                OnPropertyChanged();
            }
        }

        private bool _isGameWon;
        public bool IsGameWon
        {
            get => _isGameWon;
            set
            {
                _isGameWon = value;
                OnPropertyChanged();
            }
        }
        public ICommand ReplayCommand { get; set; }
        private static readonly Random _random = new Random();

        public MemoryGameViewModel()
        {          
            TaskListViewModel = new TaskListViewModel();

            Cards = new ObservableCollection<Card>
            {
                new Card { ImageSource = "image1.jpg", Id = 1 },
                new Card { ImageSource = "image2.jpg", Id = 2 },
                new Card { ImageSource = "image3.jpg", Id = 3 },
                new Card { ImageSource = "image4.jpg", Id = 4 },
                //new Card { ImageSource = "image5.jpg", Id = 5 },
                //new Card { ImageSource = "image6.jpg", Id = 6 },
                //new Card { ImageSource = "image7.jpg", Id = 7 },
                //new Card { ImageSource = "image8.jpg", Id = 8 },
                new Card { ImageSource = "image1.jpg", Id = 1 },
                new Card { ImageSource = "image2.jpg", Id = 2 },
                new Card { ImageSource = "image3.jpg", Id = 3 },
                new Card { ImageSource = "image4.jpg", Id = 4 },
                //new Card { ImageSource = "image5.jpg", Id = 5 },
                //new Card { ImageSource = "image6.jpg", Id = 6 },
                //new Card { ImageSource = "image7.jpg", Id = 7 },
                //new Card { ImageSource = "image8.jpg", Id = 8 }
            };

            Dictionary<string, int> imageToIdMap = new Dictionary<string, int>();
            int currentId = 1;
            foreach (var card in Cards)
            {
                if (!imageToIdMap.ContainsKey(card.ImageSource))
                {
                    imageToIdMap[card.ImageSource] = currentId++;
                }
                card.Id = imageToIdMap[card.ImageSource];
            }

            Shuffle(Cards);

            CardTappedCommand = new Command<Card>(OnCardTapped);
            ReplayCommand = new Command(ReplayGame);
            IsVisible = false;
        }
        private void Shuffle<T>(IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = _random.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
        private bool AreAllCardsMatched()
        {
            foreach (var card in Cards)
            {
                if (!card.IsMatched)
                {
                    return false;
                }
            }
            return true;
        }

        private void ReplayGame()
        {
            foreach (var card in Cards)
            {
                card.IsFlipped = false;
                card.IsMatched = false;
            }
            IsGameWon = false;
            IsVisible = false;
            Debug.WriteLine("Game replayed.");

        }
        private void OnCardTapped(Card card)
        {
            if (card.IsFlipped || card.IsMatched)
                return;

            card.IsFlipped = true;

            if (_previousCard == null)
            {
                _previousCard = card;
            }
            else
            {
                if (_previousCard.Id == card.Id)
                {
                    _previousCard.IsMatched = true;
                    card.IsMatched = true;
                    _previousCard = null;
                }
                else
                {
                    IsVisible = true;
                    Task.Delay(1000).ContinueWith(t =>
                    {
                        _previousCard.IsFlipped = false;
                        card.IsFlipped = false;
                        _previousCard = null;
                        IsVisible = false;
                    }, TaskScheduler.FromCurrentSynchronizationContext());
                }
            }

            if (AreAllCardsMatched() && AreAllCardsFlippedOver())
            {
                IsGameWon = true;
                Debug.WriteLine("Game won!");
            }
        }

        private bool AreAllCardsFlippedOver()
        {
            foreach (var card in Cards)
            {
                if (!card.IsFlipped)
                {
                    return false;
                }
            }
            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            if (propertyName == nameof(IsGameWon) && IsGameWon)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Application.Current.MainPage.DisplayAlert("Congratulations!", "You won the game!", "OK");

                    bool shouldReplay = await Application.Current.MainPage.DisplayAlert("Replay", "Do you want to play again?", "Yes", "No");

                    if (shouldReplay)
                    {
                        ReplayGame();
                    }
                });
            }
            else if (AreAllCardsMatched() && AreAllCardsFlippedOver())
            {
                IsGameWon = true;
                Debug.WriteLine("Game won!");
            }
        }

    }

    public class Card : INotifyPropertyChanged
    {
        public int Id { get; set; }

        public string ImageSource { get; set; }

        private bool _isFlipped;

        public string FrontImageSource { get; set; } = "backofcardimage.jpg";

        public bool IsFlipped
        {
            get => _isFlipped;
            set
            {
                _isFlipped = value;
                OnPropertyChanged();
            }
        }

        private bool _isMatched;
        public bool IsMatched
        {
            get => _isMatched;
            set
            {
                _isMatched = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}