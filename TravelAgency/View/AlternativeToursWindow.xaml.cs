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
    public partial class AlternativeToursWindow : Window
    {
        public static ObservableCollection<TourDTO> TourDTOs { get; set; }
        public User LoggedInUser { get; set; }
        public TourDTO Selected { get; set; }
        public AlternativeToursWindow(TourDTO tourDTO, User loggedInUser, ObservableCollection<TourDTO> tourDTOs)
        {
            InitializeComponent();
            DataContext = this;
            TourDTOs = new ObservableCollection<TourDTO>();
            LoggedInUser = loggedInUser;
            FillDTOList(tourDTO, tourDTOs);
        }

        private static void FillDTOList(TourDTO tourDTO, ObservableCollection<TourDTO> tourDTOs)
        {
            foreach (TourDTO t in tourDTOs)
            {
                if (t.City == tourDTO.City && t.Country == tourDTO.Country && t.Ocupancy != t.MaxNumOfGuests)
                {
                    TourDTOs.Add(t);
                }
            }
        }

        private void ReserveClick(object sender, RoutedEventArgs e)
        {
            if(Selected == null)
            {
                MessageBox.Show("Izaberi turu za rezervaciju");
            }
            else
            {
                BookTourWindow bookTourWindow = new BookTourWindow(Selected, LoggedInUser);
                bookTourWindow.Show();
                this.Close();
            }
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            ToursOverview overview = new ToursOverview(LoggedInUser);
            overview.Show();
            this.Close();
        }
    }
}
