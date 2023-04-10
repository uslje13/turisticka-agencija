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
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Repositories;
using SOSTeam.TravelAgency.WPF.ViewModels.Guest2;

namespace SOSTeam.TravelAgency.WPF.Views
{
    /// <summary>
    /// Interaction logic for AlternativeTours.xaml
    /// </summary>
    public partial class AlternativeToursWindow : Window
    {
        public static ObservableCollection<TourViewModel> TourDTOs { get; set; }
        public User LoggedInUser { get; set; }
        public TourViewModel Selected { get; set; }
        public AlternativeToursWindow(TourViewModel tourDTO, User loggedInUser, ObservableCollection<TourViewModel> tourDTOs)
        {
            InitializeComponent();
            AlternativeToursViewModel viewModel = new AlternativeToursViewModel(tourDTO, loggedInUser, tourDTOs,this);
            DataContext = viewModel;
            /* TourDTOs = new ObservableCollection<TourViewModel>();
             LoggedInUser = loggedInUser;
             FillDTOList(tourDTO, tourDTOs);*/
        }

        private static void FillDTOList(TourViewModel tourDTO, ObservableCollection<TourViewModel> tourDTOs)
        {
            foreach (TourViewModel t in tourDTOs)
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
            ToursOverviewWindow overview = new ToursOverviewWindow(LoggedInUser);
            overview.Show();
            this.Close();
        }
    }
}
