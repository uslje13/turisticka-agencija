using SOSTeam.TravelAgency.Application.Services;
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
using System.Windows.Shapes;

namespace SOSTeam.TravelAgency.WPF.Views.Guest1
{
    /// <summary>
    /// Interaction logic for AnswerToGuest.xaml
    /// </summary>
    public partial class AnswerToGuestWindow : Window
    {
        //public TextBlock UsersName { get; set; }
        //public TextBlock firstOldDay { get; set; }
        //public TextBlock lastOldDay { get; set; }
        //public TextBlock firstNewDay { get; set; }
        //public TextBlock lastNewDay { get; set; }
        //public TextBlock accommodationName { get; set; }
        //public string givenOwnerComment { get; set; }
        public WantedNewDate newReservation { get; set; }
        public ChangedReservationRequest oldReservation { get; set; }

        public AnswerToGuestWindow(WantedNewDate dto, ChangedReservationRequest request)
        {
            InitializeComponent();
            DataContext = this;

            oldReservation = request;
            newReservation = dto;

            FillTextBlocks();
        }

        private void FillTextBlocks()
        {
            FillUsername();
            FillFirstOldDay();
            FillLastOldDay();
            FillFirstNewDay();
            FillLastNewDay();
            FillAccommodationName();
        }

        private void FillUsername()
        {
            Binding binding = new Binding();
            UserService userService = new UserService();
            User user = userService.GetById(oldReservation.UserId);
            binding.Source = user.Username;
            UsersName.SetBinding(TextBlock.TextProperty, binding);
        }

        private void FillFirstOldDay()
        {
            Binding binding = new Binding();
            binding.Source = oldReservation.OldFirstDay.ToString();
            firstOldDay.SetBinding(TextBlock.TextProperty, binding);
        }

        private void FillLastOldDay()
        {
            Binding binding = new Binding();
            binding.Source = oldReservation.OldLastDay.ToString();
            lastOldDay.SetBinding(TextBlock.TextProperty, binding);
        }

        private void FillFirstNewDay()
        {
            Binding binding = new Binding();
            binding.Source = newReservation.wantedDate.ReservationFirstDay.ToString();
            firstNewDay.SetBinding(TextBlock.TextProperty, binding);
        }

        private void FillLastNewDay()
        {
            Binding binding = new Binding();
            binding.Source = newReservation.wantedDate.ReservationLastDay.ToString();
            lastNewDay.SetBinding(TextBlock.TextProperty, binding);
        }

        private void FillAccommodationName()
        {
            Binding binding = new Binding();
            binding.Source = newReservation.wantedDate.AccommodationName;
            accommodationName.SetBinding(TextBlock.TextProperty, binding);
        }

        private void PotvrdiKlik(object sender, RoutedEventArgs e)
        {
            AccommodationReservationService service = new AccommodationReservationService();
            service.acceptReservationChanges(newReservation, oldReservation);
        }

        private void odbijKlik(object sender, RoutedEventArgs e)
        {
            AccommodationReservationService service = new AccommodationReservationService();
            service.declineReservationChanges(givenOwnerComment.Text, newReservation, oldReservation);
        }
    }
}
