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
     
        public AlternativeToursWindow(TourViewModel tourViewModel, User loggedInUser, ObservableCollection<TourViewModel> tourViewModels)
        {
            InitializeComponent();
            AlternativeToursViewModel viewModel = new AlternativeToursViewModel(tourViewModel, loggedInUser, tourViewModels,this);
            DataContext = viewModel;
        }

    }
}
