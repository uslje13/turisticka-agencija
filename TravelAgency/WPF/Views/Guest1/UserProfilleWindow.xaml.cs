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
using SOSTeam.TravelAgency.Application.Services;

namespace SOSTeam.TravelAgency.WPF.Views.Guest1
{
    /// <summary>
    /// Interaction logic for UserProfille.xaml
    /// </summary>
    public partial class UserProfilleWindow : Window
    {
        public UserProfilleWindow(User user, int notifications)
        {
            InitializeComponent();
            UserProfilleViewModel viewModel = new UserProfilleViewModel(user, UsersName, InboxButton, notifications);
            DataContext = viewModel;
        }

        private void TestOpeningInbox(object sender, RoutedEventArgs e)
        {
            Button? targetButton = (sender as Button);
            if (targetButton != null)
            {
                InboxButton.Content = "Obavještenja - 0";
                InboxButton.Background = new SolidColorBrush(Colors.AntiqueWhite);
            }
        }
    }
}
