using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Domain.Models;
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

using SOSTeam.TravelAgency.Application.Services;

namespace SOSTeam.TravelAgency.WPF.Views.Owner
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Window
    {
        public string Username { get; set; }
        public MainPage(User user)
        {
            DataContext = this;
            Username = user.Username;
            AccommodationService accommodationService = new();
            accommodationService.GetAll();

            AccommodationReservationService service = new AccommodationReservationService();
            service.SendRequestToOwner(user.Id);

            InitializeComponent();
        }
    }
}
