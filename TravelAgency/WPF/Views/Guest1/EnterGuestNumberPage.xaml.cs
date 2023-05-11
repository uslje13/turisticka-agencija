using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.ViewModels.Guest1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace SOSTeam.TravelAgency.WPF.Views.Guest1
{
    /// <summary>
    /// Interaction logic for EnterGuestNumberPage.xaml
    /// </summary>
    public partial class EnterGuestNumberPage : Page
    {
        public EnterGuestNumberPage(AccReservationViewModel item, User user, bool enterOfChange, ChangedReservationRequest request, Frame frame)
        {
            InitializeComponent();
            EnterGuestNumberViewModel viewModel = new EnterGuestNumberViewModel(item, user, enterOfChange, request, frame);
            DataContext = viewModel;
        }

        private void TestEnteredText(object sender, RoutedEventArgs e)
        {
            if (GuestNumber.Text.Equals(""))
            {
                MessageBox.Show("Unesite broj gostiju.", " ", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (!Regex.IsMatch(GuestNumber.Text, @"^[0-9]+$"))
            {
                MessageBox.Show("Broj gostiju se mora sastojati od cifara!", " ", MessageBoxButton.OK, MessageBoxImage.Error);
                GuestNumber.Focus();
                return;
            }
        }
    }
}
