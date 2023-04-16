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
using SOSTeam.TravelAgency.WPF.ViewModels;
using SOSTeam.TravelAgency.WPF.ViewModels.Guest1;
using SOSTeam.TravelAgency.Domain.Models;

namespace SOSTeam.TravelAgency.WPF.Views.Guest1
{
    /// <summary>
    /// Interaction logic for GuestInboxWindow.xaml
    /// </summary>
    public partial class GuestInboxWindow : Window
    {
        public GuestInboxWindow(User user)
        {
            InitializeComponent();
            GuestInboxViewModel viewModel = new GuestInboxViewModel(user, this);
            DataContext = viewModel;
        }
    }
}
