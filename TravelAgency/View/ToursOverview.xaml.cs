using System.Collections.ObjectModel;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;
using TravelAgency.Model;
using TravelAgency.Repository;
using static System.Net.Mime.MediaTypeNames;

namespace TravelAgency.View
{
    /// <summary>
    /// Interaction logic for TourOverview.xaml
    /// </summary>
    public partial class ToursOverview : Window
    {
        public static ObservableCollection<Tour> Tours { get; set; }
        public Tour SelectedTour { get; set; }
        
        private readonly TourRepository _tourReository;

        public ToursOverview()
        {
            InitializeComponent();
            DataContext = this;
            _tourReository = new TourRepository();
            Tours = new ObservableCollection<Tour>(_tourReository.GetAll());
        }

        private void DeleteButtonClick(object sender, RoutedEventArgs e)
        {
            if(SelectedTour!= null)
            {
                MessageBoxResult result = MessageBox.Show("Da li ste sigurni", "Obrisite komentar",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    CheckpointRepository checkpointRepository = new CheckpointRepository();
                    LocationRepository locationRepository = new LocationRepository();
                    DateAndOccupancyRepository dateAndOccupancyRepository = new DateAndOccupancyRepository();
                    locationRepository.DeleteById(SelectedTour.LocationId);
                    _tourReository.Delete(SelectedTour);
                    checkpointRepository.DeleteByTourId(SelectedTour.Id);
                    dateAndOccupancyRepository.DeleteByTourId(SelectedTour.Id);
                    Tours.Remove(SelectedTour);
                }
            }
        }

        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
            CreateTour createTour = new CreateTour();
            createTour.Owner = Window.GetWindow(this);
            createTour.Show();
        }

        private void TodayToursButtonClick(object sender, RoutedEventArgs e)
        {
            TodayTourView todayTourView = new TodayTourView(null);  //prosledicu listu
            todayTourView.Show(); 
        }
    }
}
