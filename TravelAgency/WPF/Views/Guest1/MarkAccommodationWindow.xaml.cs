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
    /// Interaction logic for MarkAccommodationWindow.xaml
    /// </summary>
    public partial class MarkAccommodationWindow : Window
    {
        public int cleanMark { get; set; }
        public int ownerMark { get; set; }


        public MarkAccommodationWindow(User user, CancelAndMarkResViewModel accommodation)
        {
            InitializeComponent();
            FillTextBox(accommodation);
            MarkAccommodationViewModel viewModel = new MarkAccommodationViewModel(cleanMark, ownerMark, GuestComment.Text, GuestURLs.Text, user, accommodation);
            DataContext = viewModel; 
        }

        private void FillTextBox(CancelAndMarkResViewModel acc)
        {
            Binding binding = new Binding();
            AccommodationService service = new AccommodationService();
            Accommodation accommodation = service.GetById(acc.AccommodationId);
            binding.Source = accommodation.Name;
            AccommodationNameTb.SetBinding(TextBlock.TextProperty, binding);
        }

        private void c1_Checked(object sender, RoutedEventArgs e)
        {
            cleanMark = 1;
        }

        private void c2_Checked(object sender, RoutedEventArgs e)
        {
            cleanMark = 2;
        }

        private void c3_Checked(object sender, RoutedEventArgs e)
        {
            cleanMark = 3;
        }

        private void c4_Checked(object sender, RoutedEventArgs e)
        {
            cleanMark = 4;
        }

        private void c5_Checked(object sender, RoutedEventArgs e)
        {
            cleanMark = 5;
        }

        private void v1_Checked(object sender, RoutedEventArgs e)
        {
            ownerMark = 1;
        }

        private void v2_Checked(object sender, RoutedEventArgs e)
        {
            ownerMark = 2;
        }

        private void v3_Checked(object sender, RoutedEventArgs e)
        {
            ownerMark = 3;
        }

        private void v4_Checked(object sender, RoutedEventArgs e)
        {
            ownerMark = 4;
        }

        private void v5_Checked(object sender, RoutedEventArgs e)
        {
            ownerMark = 5;
        }
    }
}
