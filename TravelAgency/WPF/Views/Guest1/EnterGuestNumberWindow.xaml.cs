using System;
using System.Collections.Generic;
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
using SOSTeam.TravelAgency.WPF.ViewModels.Guest1;
using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;
using SOSTeam.TravelAgency.Domain.RepositoryInterfaces;
using SOSTeam.TravelAgency.WPF.ViewModels;

namespace SOSTeam.TravelAgency.WPF.Views
{
    /// <summary>
    /// Interaction logic for EnterGuestNumberWindow.xaml
    /// </summary>
    public partial class EnterGuestNumberWindow : Window
    {
        public EnterGuestNumberWindow(AccReservationViewModel item, User user, bool enterOfChange, ChangedReservationRequest request)
        {
            InitializeComponent();
            EnterGuestNumberViewModel viewModel = new EnterGuestNumberViewModel(item, user, enterOfChange, request, GuestNumber, this);
            DataContext = viewModel;
        }
    }
}
