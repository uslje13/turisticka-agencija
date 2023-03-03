using System.Collections.ObjectModel;
using System.Windows;
using System.Xml.Linq;
using TravelAgency.Model;
using TravelAgency.Repository;

namespace TravelAgency.View
{
    /// <summary>
    /// Interaction logic for TourOverview.xaml
    /// </summary>
    public partial class ToursOverview : Window
    {
        public static ObservableCollection<Tour> Tours { get; set; }
        public Tour SelectedTour { get; set; }
        private readonly TourRepository _repository;

        public static string FindCountryAndCityNameById(int id)
        {
            LocationRepository locationRepository = new LocationRepository();
            Location location = locationRepository.GetLocationById(id);
            string countryAndCityName = location.City + " " + "(" + location.Country + ")";
            return countryAndCityName;
        }

        /*Cirkus*/
        public static void ShowCountryAndCityName()
        {
            foreach (Tour t in Tours)
            {
                t.CountryAndCityName = FindCountryAndCityNameById(t.LocationId);
            }
        }
        public ToursOverview()
        {
            InitializeComponent();
            DataContext = this;
            _repository = new TourRepository();
            Tours = new ObservableCollection<Tour>(_repository.GetAll());

            /*
             Ovo cu zbog HCI verv. izbaciti verujem da je previs dodavati citav property zarad lepog ispisa
             */
            ShowCountryAndCityName();       //Cirkus

        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            CreateTour createTour = new CreateTour();
            createTour.Show();
        }

        private void DeleteButtonClick(object sender, RoutedEventArgs e)
        {
            if(SelectedTour!= null)
            {
                MessageBoxResult result = MessageBox.Show("Da li ste sigurni", "Obrisite komentar",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    _repository.Delete(SelectedTour);
                    Tours.Remove(SelectedTour);
                }
            }
            
        }
    }
}
