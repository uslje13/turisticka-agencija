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
        public static ObservableCollection<TourDTO> TourDTOs { get; set; }
        private readonly TourRepository _repository;

        public User LoggedInUser { get; set; }
        public TourDTO SelectedTourDTO { get; set; }
        public AlternativeTours(TourDTO tourDTO, User user, ObservableCollection<TourDTO> tourDTOs)
        {
            InitializeComponent();
            DataContext= this;
            _repository = new TourRepository();
            TourDTOs = new ObservableCollection<TourDTO>();

            foreach(TourDTO t in tourDTOs)
            {
                if(t.City == tourDTO.City && t.Country == tourDTO.Country && t.Ocupancy != t.MaxNumOfGuests)
                {
                    TourDTOs.Add(t);
                }
            }
        }
        private void ReserveClick(object sender, RoutedEventArgs e)
        {
            if(SelectedTourDTO == null)
            {
                MessageBox.Show("Izaberi turu za rezervaciju");
            }
            else
            {
                this.Close();
                BookTour bookTourWindow = new BookTour(SelectedTourDTO, LoggedInUser);
                bookTourWindow.Show();
            }
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
