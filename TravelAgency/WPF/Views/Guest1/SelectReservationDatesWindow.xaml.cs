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
using TravelAgency.DTO;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Repositories;
using SOSTeam.TravelAgency.WPF.ViewModels.Guest1;

namespace SOSTeam.TravelAgency.WPF.Views
{
    /// <summary>
    /// Interaction logic for SelectReservationDates.xaml
    /// </summary>
    public partial class SelectReservationDatesWindow : Window
    {
        public SelectReservationDatesWindow(List<AccReservationViewModel> list, User user, bool enter, ChangedReservationRequest request)
        {
            InitializeComponent();
            SelectResDatesViewModel viewModel = new SelectResDatesViewModel(list, user, enter, request, this);
            DataContext = viewModel;
        }
    }
}
