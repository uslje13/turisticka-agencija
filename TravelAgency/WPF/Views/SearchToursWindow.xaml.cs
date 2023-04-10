﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.ViewModels.Guest2;

namespace SOSTeam.TravelAgency.WPF.Views
{
    /// <summary>
    /// Interaction logic for SearchToursWindow.xaml
    /// </summary>
    public partial class SearchToursWindow : Window, INotifyPropertyChanged
    {
        private string _searchedLanguage;
        private string _searchedCity;
        private string _searchedCountry;
        private int _searchedOcupancy;
        private int _searchedDuration;
        public User LoggedInUser { get; set; }
        public static ObservableCollection<TourViewModel> TourDTOs { get; set; }
        public SearchToursWindow(ObservableCollection<TourViewModel> tourDTOs, User loggedInUser)
        {
            InitializeComponent();
            DataContext = this;
            TourDTOs = new ObservableCollection<TourViewModel>();
            TourDTOs = tourDTOs;
            LoggedInUser = loggedInUser;
        }

        public string SearchedLanguage
        {
            get => _searchedLanguage;
            set
            {
                if (value != _searchedLanguage)
                {
                    _searchedLanguage = value;
                    OnPropertyChanged();
                }
            }
        }

        public string SearchedCity
        {
            get => _searchedCity;
            set
            {
                if (value != _searchedCity)
                {
                    _searchedCity = value;
                    OnPropertyChanged();
                }
            }
        }
        public string SearchedCountry
        {
            get => _searchedCountry;
            set
            {
                if (value != _searchedCountry)
                {
                    _searchedCountry = value;
                    OnPropertyChanged();
                }
            }
        }
        public int SearchedOcupancy
        {
            get => _searchedOcupancy;
            set
            {
                if (value != _searchedOcupancy)
                {
                    _searchedOcupancy = value;
                    OnPropertyChanged();
                }
            }
        }
        public int SearchedDuration
        {
            get => _searchedDuration;
            set
            {
                if (value != _searchedDuration)
                {
                    _searchedDuration = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void SearchToursClick(object sender, RoutedEventArgs e)
        {
            LoadEnteredRequests();
            ObservableCollection<TourViewModel> searchResult = new ObservableCollection<TourViewModel>();

            foreach (var item in TourDTOs)
            {
                bool isCorrect = IsApropriate(item);
                if (isCorrect)
                {
                    searchResult.Add(item);
                }
            }
            ShowResults(searchResult);
        }

        private void LoadEnteredRequests()
        {
            SearchedLanguage = language.Text;
            SearchedCity = city.Text;
            SearchedCountry = country.Text;
            string loadedDuration = duration.Text;
            SearchedDuration = int.Parse(loadedDuration);
            string loadedOcupancy = ocupancy.Text;
            SearchedOcupancy = int.Parse(loadedOcupancy);
        }

        private void ShowResults(ObservableCollection<TourViewModel> searchResult)
        {
            if (searchResult.Count > 0)
            {
                ToursOverviewWindow overview = new ToursOverviewWindow(LoggedInUser);
                overview.ToursGrid.ItemsSource = searchResult;
                overview.Show();
                Close();
            }
            else
            {
                MessageBox.Show("Nepostojeća kombinacija podataka. Pokušajte ponovo.");
            }
        }

        private bool IsApropriate(TourViewModel tourDTO)
        {
            bool checkCity = tourDTO.City.ToLower().Contains(SearchedCity.ToLower()) || SearchedCity.Equals(string.Empty);
            bool checkCountry = tourDTO.Country.ToLower().Contains(SearchedCountry.ToLower()) || SearchedCountry.Equals(string.Empty);
            bool checkLanguage = tourDTO.Language.ToLower().Contains(SearchedLanguage.ToLower()) || SearchedLanguage.Equals(string.Empty);
            bool checkOcupancy = SearchedOcupancy <= tourDTO.MaxNumOfGuests;
            bool checkDuration = SearchedDuration == tourDTO.Duration || SearchedDuration == 0;

            return checkCity && checkCountry && checkLanguage && checkOcupancy && checkDuration;
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            ToursOverviewWindow overview = new ToursOverviewWindow(LoggedInUser);
            overview.Show();
            Close();
        }
    }
}
