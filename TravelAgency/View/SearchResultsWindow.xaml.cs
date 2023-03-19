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
using TravelAgency.DTO;
using TravelAgency.Model;

namespace TravelAgency.View
{
    /// <summary>
    /// Interaction logic for SearchResults.xaml
    /// </summary>
    public partial class SearchResultsWindow : Window
    {
        public ObservableCollection<LocAccommodationDTO> accommodationDTOs { get; set; }
        public LocAccommodationDTO SelectedAccommodationDTO { get; set; }
        public User LoggedInUser { get; set; }

        public SearchResultsWindow()
        {
            InitializeComponent();
        }

        public SearchResultsWindow(List<LocAccommodationDTO> Results, User user)
        {
            InitializeComponent();
            DataContext = this;
            accommodationDTOs = new ObservableCollection<LocAccommodationDTO>(Results);
            LoggedInUser = user;
        }

        private void ReserveAccommodationClick(object sender, RoutedEventArgs e)
        {
            if (SelectedAccommodationDTO != null)
            {
                EnterReservationWindow newWindow = new EnterReservationWindow(SelectedAccommodationDTO, LoggedInUser);
                newWindow.ShowDialog();
                Close();
            }
            else
            {
                MessageBox.Show("Morate da odaberete smeštaj za rezervaciju.");
            }
        }
    }
}
