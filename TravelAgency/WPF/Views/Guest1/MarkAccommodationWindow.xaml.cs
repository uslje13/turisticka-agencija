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
using System.Security.Cryptography.X509Certificates;
using System.Collections.ObjectModel;

namespace SOSTeam.TravelAgency.WPF.Views.Guest1
{
    /// <summary>
    /// Interaction logic for MarkAccommodationWindow.xaml
    /// </summary>
    public partial class MarkAccommodationWindow : Window
    {
        public MarkAccommodationWindow(User user, CancelAndMarkResViewModel accommodation, ObservableCollection<CancelAndMarkResViewModel> reservationsForMark, ObservableCollection<CancelAndMarkResViewModel> ratingsFromOwner)
        {
            InitializeComponent();
            List<RadioButton> cleanButtons = new List<RadioButton> { c1, c2, c3, c4, c5 };
            List<RadioButton> ownerButtons = new List<RadioButton> { v1, v2, v3, v4, v5 };
            List<RadioButton> renovationButtons = new List<RadioButton> { h1, h2, h3, h4, h5 };
            MarkAccommodationViewModel viewModel = new MarkAccommodationViewModel(cleanButtons, ownerButtons, renovationButtons, user, accommodation, this, reservationsForMark, ratingsFromOwner);
            DataContext = viewModel;
        }
    }
}
