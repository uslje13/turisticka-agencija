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
    /// Interaction logic for AlternativeTours.xaml
    /// </summary>
    public partial class AlternativeTours : Window
    {
        public static ObservableCollection<Tour> Tours { get; set; }
        private readonly TourRepository _repository;
        public Tour SelectedTour { get; set; }
        public AlternativeTours(Tour tour)
        {
            InitializeComponent();
            DataContext= this;
            _repository = new TourRepository();
            Tours = new ObservableCollection<Tour>();
            
                foreach (Tour t in _repository.GetAll())
                {
                    if (t.LocationId == tour.LocationId && t.NumOfGuests != t.MaxNumOfGuests)
                    {
                        Tours.Add(t);
                    }
                }
           
        }
        private void ReserveClick(object sender, RoutedEventArgs e)
        {
            if(SelectedTour == null)
            {
                MessageBox.Show("Izaberi turu za rezervaciju");
            }
            else
            {
                this.Close();
                BookTour bookTourWindow = new BookTour(SelectedTour);
                bookTourWindow.Show();
            }
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
