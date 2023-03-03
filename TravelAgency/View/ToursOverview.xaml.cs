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

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            CreateTour createTour = new CreateTour();
            createTour.Show();
        }
    }
}
