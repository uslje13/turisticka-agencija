using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.ViewModels.Guest1;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SOSTeam.TravelAgency.WPF.Views.Guest1
{
    /// <summary>
    /// Interaction logic for UserProfillePage.xaml
    /// </summary>
    public partial class UserProfillePage : Page
    {
        public UserProfillePage(User user, int notifications, NavigationService service)
        {
            InitializeComponent();
            UserProfilleViewModel viewModel = new UserProfilleViewModel(user, notifications, service);
            DataContext = viewModel;
        }

        private void TestOpeningInbox(object sender, RoutedEventArgs e)
        {
            Button? targetButton = (sender as Button);
            if (targetButton != null)
            {
                //InboxButton.Content = "Inbox";
                Binding binding = new Binding();
                binding.Source = "   ";
                Messages.SetBinding(TextBlock.TextProperty, binding);
                InboxButton.Background = new SolidColorBrush(Colors.LightGoldenrodYellow);
            }
        }
    }
}
