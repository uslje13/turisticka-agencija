using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Owner
{
    internal class AddAccommodationPageViewModel
    {
        
        private AccommodationService _accommodationService;
        private LocationService _locationService;
        private MainWindowViewModel _mainwindowVM;

        public User LoggedInUser { get; private set; }
        public string AName { get; set; }
        public Accommodation.AccommodationType Type { get; set; }
        public string LocationId { get; set; }
        public int MaxGuests { get; set; }
        public int MinDaysStay { get; set; }
        public int MinDaysForCancelation { get; set; }
        public ObservableCollection<Image> Images { get; set; }
        public List<Location> Locations { get; set; }
        public ReadOnlyObservableCollection<string> Countries { get; set; }
        public ObservableCollection<string> Cities { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public bool CountryBoxEnabled { get; set; }
        public AddAccommodationPageViewModel(User user, MainWindowViewModel mainWindowVM)
        {
            LoggedInUser = user;
            _accommodationService = new AccommodationService();
            _locationService = new();
            _mainwindowVM = mainWindowVM;

            

            Images = new ObservableCollection<Image>();
            Locations = new List<Location>(_locationService.GetAll());
            Countries = new ReadOnlyObservableCollection<string>(GetCountries());
            Cities = new ObservableCollection<string>();
            LoggedInUser = user;
            CountryBoxEnabled = true;

            AName = string.Empty;
            Country = string.Empty;
            City = string.Empty;
            MinDaysForCancelation = 1;
        }

        public void CheckedType(object sender)
        {
            System.Windows.Controls.RadioButton radioButton = sender as System.Windows.Controls.RadioButton;
            if (radioButton == null) return;

            if (radioButton.Content.Equals("Apartment")) Type = Accommodation.AccommodationType.APARTMENT;
            else if (radioButton.Content.Equals("House")) Type = Accommodation.AccommodationType.HOUSE;
            else Type = Accommodation.AccommodationType.HUT;
        }
        private void ButtonClickCancel(object sender)
        {
            
        }

        private void ButtonClickAdd(object sender)
        {
            if (!IsValid())
            {
                return;
            }


            int locationId = FindLocationId();
            Accommodation accommodation = new Accommodation(AName, Type, locationId, MaxGuests, MinDaysStay, LoggedInUser.Id, MinDaysForCancelation);
            _accommodationService.Save(accommodation);
            
        }

        

        private void CountryComboBoxSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            CountryBoxEnabled = false;
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
            if (AName.Length < 1 || Images.Count < 1 || City.Length < 1)
            {
                return false;
            }
            return true;
        }
    }
}
