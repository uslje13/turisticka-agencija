using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using static System.Net.Mime.MediaTypeNames;
using SOSTeam.TravelAgency.WPF.ViewModels.Guest2;

namespace SOSTeam.TravelAgency.WPF.Views
{
    /// <summary>
    /// Interaction logic for ToursOverview.xaml
    /// </summary>
    public partial class ToursOverviewWindow : Window
    {
        public ToursOverviewWindow(User loggedInUser)
        {
            InitializeComponent();
            ToursOverviewViewModel viewModel = new ToursOverviewViewModel(loggedInUser,this);
            this.DataContext = viewModel;
            viewModel.GetAttendanceMessage();
        }

    }
}