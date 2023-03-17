using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TravelAgency.Model;
using static TravelAgency.Model.AccommodationDTO;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;
using System.Data;
using System.Xml.Linq;

namespace TravelAgency.View
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
        public static ObservableCollection<TourDTO> TourDTOs { get; set; }
        public SearchToursWindow(ObservableCollection<TourDTO> tourDTOs, User user)
        {
            InitializeComponent();
            DataContext = this;
            TourDTOs = new ObservableCollection<TourDTO>();
            TourDTOs = tourDTOs;
            LoggedInUser= user;
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
            ObservableCollection<TourDTO> searchResult = new ObservableCollection<TourDTO>();

            foreach (var item in TourDTOs)
            {
                bool isCorrect = IsApropriate(item);
                if(isCorrect)
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

        private void ShowResults(ObservableCollection<TourDTO> searchResult)
        {
            if (searchResult.Count > 0)
            {
                ShowAndSearchTours showAndSearchTours = new ShowAndSearchTours(LoggedInUser);
                showAndSearchTours.ToursGrid.ItemsSource= searchResult;
                showAndSearchTours.Show();
                Close();
            }
            else
            {
                MessageBox.Show("Nepostojeća kombinacija podataka. Pokušajte ponovo.");
            }
        }

        private bool IsApropriate(TourDTO tourDTO)
        {
            bool checkCity = tourDTO.City.ToLower().Contains(SearchedCity.ToLower()) || SearchedCity.Equals(string.Empty);
            bool checkCountry = tourDTO.Country.Contains(SearchedCountry) || SearchedCountry.Equals(string.Empty);
            bool checkLanguage = tourDTO.Language.Contains(SearchedLanguage) || SearchedLanguage.Equals(string.Empty);
            bool checkOcupancy = SearchedOcupancy <= tourDTO.MaxNumOfGuests;
            bool checkDuration = SearchedDuration == tourDTO.Duration || SearchedDuration == 0;

            return checkCity && checkCountry && checkLanguage && checkOcupancy && checkDuration;
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            ShowAndSearchTours showAndSearchTours = new ShowAndSearchTours(LoggedInUser);
            showAndSearchTours.Show();
            Close();
        }
    }
}
