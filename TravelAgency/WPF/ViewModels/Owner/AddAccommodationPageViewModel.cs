using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Owner
{
    internal class AddAccommodationPageViewModel : ViewModel
    {
        
        private AccommodationService _accommodationService;
        private LocationService _locationService;
        private MainWindowViewModel _mainwindowVM;

        public User LoggedInUser { get; private set; }
        public string AName { get; set; }
        public Accommodation.AccommodationType SelectedType { get; set; }
        public string LocationId { get; set; }
        public int MaxGuests { get; set; }
        public int MinDaysStay { get; set; }
        public int MinDaysForCancelation { get; set; }
        public ObservableCollection<Image> Images { get; set; }
        public List<Location> Locations { get; set; }
        public ReadOnlyObservableCollection<string> Countries { get; set; }
        public ObservableCollection<string> Cities { get; set; }

        private string _country;
        public string Country
        {
            get => _country;
            set
            {
                if (_country != value)
                {
                    _country = value;
                    CountryComboBoxSelectionChanged();
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
                }
            }
        }

        public bool CountryBoxEnabled { get; set; }
        public RelayCommand Cancel { get; private set; }
        public RelayCommand AddAccommodation { get; private set; }
        public AddAccommodationPageViewModel(User user, MainWindowViewModel mainWindowVM)
        {
            LoggedInUser = user;
            _accommodationService = new AccommodationService();
            _locationService = new();
            _mainwindowVM = mainWindowVM;


            AddAccommodation = new RelayCommand(Execute_AddAccommodation, CanExecuteAddAccommodation);
            Cancel = new RelayCommand(Execute_Cancel, CanExecuteCancel);
            Images = new ObservableCollection<Image>();
            Locations = new List<Location>(_locationService.GetAll());
            Countries = new ReadOnlyObservableCollection<string>(GetCountries());
            Cities = new ObservableCollection<string>();
            SelectedType = Accommodation.AccommodationType.HUT;
            LoggedInUser = user;
            CountryBoxEnabled = true;

            AName = string.Empty;
            Country = string.Empty;
            City = string.Empty;
            MinDaysForCancelation = 1;
        }

        private bool CanExecuteAddAccommodation(object obj)
        {
            return IsValid();
        }

        private bool CanExecuteCancel(object obj)
        {
            return true;
        }

        private void Execute_AddAccommodation(object obj)
        {
            int locationId = FindLocationId();
            Accommodation accommodation = new Accommodation(AName, SelectedType, locationId, MaxGuests, MinDaysStay, LoggedInUser.Id, MinDaysForCancelation);
            _accommodationService.Save(accommodation);
            _mainwindowVM.Execute_NavigationButtonCommand("Accommodation");
            return;
        }

        private void Execute_Cancel(object obj)
        {
            _mainwindowVM.Execute_NavigationButtonCommand("Accommodation");
            return;
        }


        

        private void CountryComboBoxSelectionChanged()
        {
            City = "";
            Cities.Clear();
            GetCities();
        }



        public ObservableCollection<string> GetCountries()
        {
            ObservableCollection<string> coutries = new ObservableCollection<string>();
            foreach (Location location in Locations)
            {
                coutries.Add(location.Country);
            }
            coutries = new ObservableCollection<string>(coutries.Distinct());
            return coutries;
        }

        private void SetImagesTourId(Accommodation accommodation)
        {
            foreach (Image image in Images)
            {
                image.EntityId = accommodation.Id;
            }
        }

        public void GetCities()
        {
            foreach (Location location in Locations)
            {
                if (location.Country.Equals(Country))
                {
                    Cities.Add(location.City);
                }
            }
        }

        private int FindLocationId()
        {
            List<Location> locations = new List<Location>(_locationService.GetAll());
            Location location = locations.Find(l => l.Country.Equals(Country) && l.City.Equals(City));
            return location.Id;
        }

        private bool IsValid()
        {
            if(City == null || Country == null) 
            {
                return false;
            }
            if (AName.Length < 1 || City.Length < 1 || Country.Length < 1)
            {
                return false;
            }
            return true;
        }
    }

    public class EnumToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Accommodation.AccommodationType enumValue = (Accommodation.AccommodationType)value;
            Accommodation.AccommodationType targetValue = (Accommodation.AccommodationType)parameter;
            return enumValue == targetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isChecked = (bool)value;
            Accommodation.AccommodationType targetValue = (Accommodation.AccommodationType)parameter;
            return isChecked ? targetValue : Binding.DoNothing;
        }
    }
}
