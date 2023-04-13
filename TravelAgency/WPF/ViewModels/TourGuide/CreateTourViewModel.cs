using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;

namespace SOSTeam.TravelAgency.WPF.ViewModels.TourGuide
{
    public class CreateTourViewModel : ViewModel
    {
        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged("Name");
                }
            }
        }
        private string _city;
        public string City
        {
            get => _city;
            set
            {
                if (_city != value)
                {
                    _city = value;
                    OnPropertyChanged("City");
                }
            }
        }
        private string _country;
        public string Country
        {
            get => _country;
            set
            {
                if (_country != value)
                {
                    _country = value;
                    OnPropertyChanged("Country");
                }
            }
        }
        private string _description;
        public string Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged("Description");
                }
            }
        }
        private string _language;
        public string Language
        {
            get => _language;
            set
            {
                if (_language != value)
                {
                    _description = value;
                    OnPropertyChanged("Language");
                }
            }
        }
        private int _maxNumOfGuests;
        public int MaxNumOfGuests
        {
            get => _maxNumOfGuests;
            set
            {
                if (_maxNumOfGuests != value)
                {
                    _maxNumOfGuests = value;
                    OnPropertyChanged("MaxNumOfGuests");
                }
            }
        }
        private int _duration;
        public int Duration
        {
            get => _duration;

            set
            {
                if (_duration != value)
                {
                    _duration = value;
                    OnPropertyChanged("Duration");
                }
            }
        }
        private readonly LocationService _locationService;
        private Location _location;

        public ObservableCollection<string> Countries { get; set; }

        public ObservableCollection<string> Cities { get; set; }

        public List<Location> Locations { get; set; }

        public RelayCommand CitySelectionChangedCommand { get; set; }
        public RelayCommand CountrySelectionChangedCommand { get; set; }

        public CreateTourViewModel()
        {
            _locationService = new LocationService();
            Locations = _locationService.GetAll();
            Countries = GetCountries();
            Cities = GetCities();
            _location = new Location();

            CountrySelectionChangedCommand = new RelayCommand(ExecuteCountrySelectionChanged, CanExecuteMethod);
            CitySelectionChangedCommand = new RelayCommand(ExecuteCitySelectionChanged, CanExecuteMethod);
        }
        private bool CanExecuteMethod(object parameter)
        {
            return true;
        }

        public ObservableCollection<string> GetCountries()
        {
            ObservableCollection<string> countries = new ObservableCollection<string>();
            foreach (Location location in Locations)
            {
                countries.Add(location.Country);
            }
            countries = new ObservableCollection<string>(countries.Distinct());
            return countries;
        }

        public ObservableCollection<string> GetCities()
        {
            ObservableCollection<string> citites = new ObservableCollection<string>();
            foreach (Location location in Locations)
            {
                citites.Add(location.City);
            }
            return citites;
        }

        public void ExecuteCountrySelectionChanged(object parameter)
        {
            CountrySelectionChanged(parameter, null);
        }

        public void ExecuteCitySelectionChanged(object parameter)
        {
            CitySelectionChanged(parameter, null);
        }

        // Event handler for country combo box SelectionChanged event
        public void CountrySelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Clear the Cities collection
            Cities.Clear();

            // Get the selected country from the country combo box
            string selectedCountry = Country;

            // Filter the locations based on the selected country
            var filteredLocations = Locations.Where(location => location.Country == selectedCountry).ToList();

            // Add the cities from the filtered locations to the Cities collection
            foreach (var location in filteredLocations)
            {
                Cities.Add(location.City);
            }
        }

        // Event handler for city combo box SelectionChanged event
        public void CitySelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Get the selected city from the city combo box
            string selectedCity = City;

            // Find the location with the selected city
            var selectedLocation = Locations.FirstOrDefault(location => location.City == selectedCity);

            if (selectedLocation != null)
            {
                // Update the City property with the selected city
                City = selectedCity;

                // Set the selected country to the country combo box
                if (_location.Country == null || _location.Country != selectedLocation.Country) // Check if _location.Country is null or not equal to selectedLocation.Country
                {
                    _location.Country = selectedLocation.Country;
                }
            }
        }

    }
}
