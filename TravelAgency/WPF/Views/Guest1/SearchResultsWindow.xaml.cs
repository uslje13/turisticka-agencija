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
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.ViewModels.Guest1;

namespace SOSTeam.TravelAgency.WPF.Views
{
    /// <summary>
    /// Interaction logic for SearchResults.xaml
    /// </summary>
    public partial class SearchResultsWindow : Window
    {
        /*
        public ObservableCollection<LocAccommodationViewModel> accommodationDTOs { get; set; }
        public LocAccommodationViewModel SelectedAccommodationDTO { get; set; }
        public User LoggedInUser { get; set; }
        */

        public SearchResultsWindow(List<LocAccommodationViewModel> Results, User user)
        {
            InitializeComponent();
            SearchResultsViewModel viewModel = new SearchResultsViewModel(Results, user);
            DataContext = viewModel;
            //DataContext = this;
            /*
            accommodationDTOs = new ObservableCollection<LocAccommodationViewModel>(Results);
            LoggedInUser = user;
            */
        }

        /*
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
        */
    }
}
