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
using TravelAgency.Model;
using TravelAgency.Repository;

namespace TravelAgency.View
{
    /// <summary>
    /// Interaction logic for CreateAccommodation.xaml
    /// </summary>
    public partial class CreateAccommodationWindow : Window
    {
        public User LoggedInUser { get; set; }
        public string AName { get; set; }
        public string Type { get; set; }
        public string LocationId { get; set; }
        public string maxGuests { get; set; }
        public string MinDaysStay { get; set; }

        private AccommodationRepository _accommodationRepository;
        public CreateAccommodationWindow(AccommodationRepository accommodationRepository, User user)
        {
            DataContext = this;
            _accommodationRepository = accommodationRepository;
            LoggedInUser = user;
            InitializeComponent();
        }

        private void ButtonClickCancel(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ButtonClickAdd(object sender, RoutedEventArgs e)
        {
            Accommodation accommodation = new Accommodation(AName, (Accommodation.AccommodationType)Convert.ToInt32(Type) , Convert.ToInt32(LocationId), Convert.ToInt32(maxGuests), Convert.ToInt32(MinDaysStay), "URL", LoggedInUser.Id);
            _accommodationRepository.Save(accommodation);
            Close();
        }
    }
}
