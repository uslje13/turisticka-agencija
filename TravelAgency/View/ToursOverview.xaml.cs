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
        
        private readonly TourRepository _repository;

        public ToursOverview()
        {
            InitializeComponent();
            DataContext = this;
            _repository = new TourRepository();
            Tours = new ObservableCollection<Tour>(_repository.GetAll());
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

        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
            CreateTour createTour = new CreateTour();
            createTour.Show();
        }
    }
}
