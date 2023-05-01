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
using SOSTeam.TravelAgency.WPF.ViewModels;
using SOSTeam.TravelAgency.WPF.ViewModels.Guest1;

namespace SOSTeam.TravelAgency.WPF.Views.Guest1
{
    /// <summary>
    /// Interaction logic for RequestsStatus.xaml
    /// </summary>
    public partial class RequestsStatusWindow : Window
    {
        public RequestsStatusWindow(User user, Window profille, int notifications)
        {
            InitializeComponent();
            RequestsStatusViewModel viewModel = new RequestsStatusViewModel(user, this, StatusFrame, UsersName, profille, notifications);
            DataContext = viewModel;
            viewModel.SetStartupPage();
        }
    }
}
