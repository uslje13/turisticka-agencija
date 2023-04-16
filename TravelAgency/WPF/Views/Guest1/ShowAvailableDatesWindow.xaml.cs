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
using SOSTeam.TravelAgency.Repositories;
using SOSTeam.TravelAgency.WPF.ViewModels.Guest1;

namespace SOSTeam.TravelAgency.WPF.Views
{
    /// <summary>
    /// Interaction logic for ShowAvailableDates.xaml
    /// </summary>
    public partial class ShowAvailableDatesWindow : Window
    {
        public ShowAvailableDatesWindow(LocAccommodationViewModel dto, DateTime firstDay, DateTime lastDay, int days, User user, bool enter, ChangedReservationRequest request)
        {
            InitializeComponent();
            ShowAvailableDatesViewModel viewModel = new ShowAvailableDatesViewModel(dto, firstDay, lastDay, days, user, Calendar, enter, request, this);
            DataContext = viewModel;
        }
    }
}
