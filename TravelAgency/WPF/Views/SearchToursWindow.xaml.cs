using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.ViewModels.Guest2;

namespace SOSTeam.TravelAgency.WPF.Views
{
    /// <summary>
    /// Interaction logic for SearchToursWindow.xaml
    /// </summary>
    public partial class SearchToursWindow : Window
    {
        public SearchToursWindow(ObservableCollection<TourViewModel> tourDTOs, User loggedInUser)
        {
            InitializeComponent();
            SearchToursViewModel viewModel = new SearchToursViewModel(tourDTOs, loggedInUser,this);
            DataContext = viewModel;
        }
       
    }
}
