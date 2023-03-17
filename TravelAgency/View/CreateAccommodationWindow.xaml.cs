using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
using TravelAgency.Repository;

namespace TravelAgency.View
{
    /// <summary>
    /// Interaction logic for CreateAccommodation.xaml
    /// </summary>
    public partial class CreateAccommodationWindow : Window
    {
        public User LoggedInUser { get; set; }
        public string AName { get; set; }
        public Accommodation.AccommodationType Type { get; set; }
        public string LocationId { get; set; }
        public int MaxGuests { get; set; }
        public int MinDaysStay { get; set; }
        public ObservableCollection<Model.Image> Images { get; set; }
        public List<Location> Locations { get; set; }
        public ReadOnlyObservableCollection<string> Countries { get; set; }
        public ObservableCollection<string> Cities { get; set; }
        public string Country { get; set; }
        public string City { get; set; }


        private AccommodationRepository _accommodationRepository;
        private LocationRepository _locationRepository;
        public CreateAccommodationWindow(AccommodationRepository accommodationRepository, User user)
        {
            DataContext = this;

            _accommodationRepository = accommodationRepository;
            _locationRepository = new();

            Images = new ObservableCollection<Model.Image>();
            Locations = new List<Location>(_locationRepository.GetAll());
            Countries = new ReadOnlyObservableCollection<string>(GetCountries());
            Cities = new ObservableCollection<string>();
            LoggedInUser = user;

            AName = string.Empty;
            Country = string.Empty;
            City = string.Empty;

            InitializeComponent();
        }

        public void CheckedType(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
            if (radioButton == null) return;

            if (radioButton.Content.Equals("Apartment")) Type = Accommodation.AccommodationType.APARTMENT;
            else if (radioButton.Content.Equals("House")) Type = Accommodation.AccommodationType.HOUSE;
            else Type = Accommodation.AccommodationType.HUT;
        }
        private void ButtonClickCancel(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ButtonClickAdd(object sender, RoutedEventArgs e)
        {
            if (!IsValid()) return;

            int locationId = FindLocationId();
            Accommodation accommodation = new Accommodation(AName,Type , locationId, MaxGuests, MinDaysStay, "URL", LoggedInUser.Id);
            _accommodationRepository.Save(accommodation);
            Close();
        }

        private void AddImagesButtonClick(object sender, RoutedEventArgs e)
        {
            AddImagesWindow addImagesWindow = new AddImagesWindow(Images);
            addImagesWindow.Owner = Window.GetWindow(this);
            addImagesWindow.Show();
        }

        private void CountryComboBoxSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            countryComboBox.IsEnabled = false;
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
            List<Location> locations = new List<Location>(_locationRepository.GetAll());
            Location location = locations.Find(l => l.Country.Equals(Country) && l.City.Equals(City));
            return location.Id;
        }

        private bool IsValid()
        {
            if ( AName.Length < 1 || Images.Count < 1 || City.Length < 1 )
            {
                return false;
            }
            return true;
        }
    }
}
