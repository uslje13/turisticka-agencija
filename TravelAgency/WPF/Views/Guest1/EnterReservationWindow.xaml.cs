using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Repositories;
using SOSTeam.TravelAgency.WPF.ViewModels.Guest1;
using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;
using System.Runtime.InteropServices.ObjectiveC;
using SOSTeam.TravelAgency.WPF.ViewModels;

namespace SOSTeam.TravelAgency.WPF.Views
{
    /// <summary>
    /// Interaction logic for EnterReservation.xaml
    /// </summary>
    public partial class EnterReservationWindow : Window
    {
        public EnterReservationWindow(LocAccommodationViewModel dto, User user, bool enter)
        {
            InitializeComponent();
            EnterReservationViewModel viewModel = new EnterReservationViewModel(dto, user, enter, Days, FirstDay, LastDay, this);
            DataContext = viewModel;
        }

        public EnterReservationWindow(LocAccommodationViewModel dto, User user, bool enter, ChangedReservationRequest request)
        {
            InitializeComponent();
            EnterReservationViewModel viewModel = new EnterReservationViewModel(dto, user, enter, Days, FirstDay, LastDay, this, request);
            DataContext = viewModel;
        }
    }
}
