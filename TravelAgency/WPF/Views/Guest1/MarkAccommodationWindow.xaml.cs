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
        public MarkAccommodationWindow(User user, CancelAndMarkResViewModel accommodation)
        {
            InitializeComponent();
            List<RadioButton> cleanButtons = new List<RadioButton> { c1, c2, c3, c4, c5 };
            List<RadioButton> ownerButtons = new List<RadioButton> { v1, v2, v3, v4, v5 };
            MarkAccommodationViewModel viewModel = new MarkAccommodationViewModel(AccommodationNameTb, cleanButtons, ownerButtons, GuestComment, GuestURLs, user, accommodation, this);
            DataContext = viewModel; 
        }
    }
}
