using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest2
{
    public class SearchToursViewModel : ViewModel
    {
        private Window _window;
        private string _searchedLanguage;
        private string _searchedCity;
        private string _searchedCountry;
        private int _searchedOcupancy;
        private int _searchedDuration;
        public User LoggedInUser { get; set; }
        public static ObservableCollection<TourViewModel> TourDTOs { get; set; }

        private RelayCommand cancelCommand;
        public RelayCommand CancelCommand
        {
            get { return cancelCommand; }
            set
            {
                cancelCommand = value;
            }
        }

        private RelayCommand searchCommand;

        public RelayCommand SearchCommand
        {
            get { return searchCommand; }
            set
            {
                searchCommand = value;
            }
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

        public SearchToursViewModel(ObservableCollection<TourViewModel> tourDTOs, User loggedInUser, Window window)
        {
            TourDTOs = new ObservableCollection<TourViewModel>();
            TourDTOs = tourDTOs;
            LoggedInUser = loggedInUser;
            SearchCommand = new RelayCommand(Execute_SearchToursClick, CanExecuteMethod);
            CancelCommand = new RelayCommand(Execute_CancelClick, CanExecuteMethod);
            _window = window;
        }

        private bool CanExecuteMethod(object parameter)
        {
            return true;
        }
        private void Execute_SearchToursClick(object sender)
        {
            //LoadEnteredRequests();
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

       /* private void LoadEnteredRequests()
        {
            SearchedLanguage = language.Text;
            SearchedCity = city.Text;
            SearchedCountry = country.Text;
            string loadedDuration = duration.Text;
            SearchedDuration = int.Parse(loadedDuration);
            string loadedOcupancy = ocupancy.Text;
            SearchedOcupancy = int.Parse(loadedOcupancy);
        }*/

        private void ShowResults(ObservableCollection<TourViewModel> searchResult)
        {
            if (searchResult.Count > 0)
            {
                ToursOverviewWindow overview = new ToursOverviewWindow(LoggedInUser);
                overview.ToursGrid.ItemsSource = searchResult;
                overview.Show();
                _window.Close();
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

        private void Execute_CancelClick(object sender)
        {
            ToursOverviewWindow overview = new ToursOverviewWindow(LoggedInUser);
            overview.Show();
            _window.Close();
        }
    }
}
