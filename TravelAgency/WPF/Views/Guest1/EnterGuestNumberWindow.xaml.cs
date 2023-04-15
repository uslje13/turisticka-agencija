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
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Repositories;
using SOSTeam.TravelAgency.WPF.ViewModels.Guest1;
using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;
using SOSTeam.TravelAgency.Domain.RepositoryInterfaces;

namespace SOSTeam.TravelAgency.WPF.Views
{
    /// <summary>
    /// Interaction logic for EnterGuestNumberWindow.xaml
    /// </summary>
    public partial class EnterGuestNumberWindow : Window
    {
        
        public AccReservationViewModel forwardedItem { get; set; }
        public User LoggedInUser { get; set; }
        /*
        public AccommodationReservationRepository accommodationReservationRepository { get; set; }
        public List<AccommodationReservation> accommodationReservations { get; set; }
        */
        public AccommodationReservationService accResService { get; set; }
        public int guestNumber {  get; set; }
        public RelayCommand finishReserveCommand { get; set; }
        public ChangedReservationRequest ChangedReservationRequest { get; set; }


        public EnterGuestNumberWindow(AccReservationViewModel item, User user, bool enterOfChange, ChangedReservationRequest request)
        {
            InitializeComponent();
            DataContext = this;
            forwardedItem = item;
            LoggedInUser = user;
            ChangedReservationRequest = request;

            if (!enterOfChange)
            {
                finishReserveCommand = new RelayCommand(ExecuteReserveAccommodation);
            } 
            else
            {
                finishReserveCommand = new RelayCommand(ExecuteSendRequestForChange);
            }
        }

        private void ExecuteSendRequestForChange(object sender)
        {
            guestNumber = int.Parse(GuestNumber.Text);
            accResService = new AccommodationReservationService();
            ExecuteSendRequestForChange(forwardedItem, guestNumber);
        }

        private void ExecuteSendRequestForChange(AccReservationViewModel item, int forwadedGuestNumber)
        {
            if (forwadedGuestNumber > 0)
            {
                if (forwadedGuestNumber > item.AccommodationMaxGuests)
                {
                    MessageBox.Show("Prekoračen je maksimalni broj gostiju za ovaj smeštaj. Pokušajte ponovo.");
                }
                else
                {
                    accResService.ProcessReservation(item, LoggedInUser, ChangedReservationRequest);
                    MessageBox.Show("Zahtjev za pomjeranje rezervacije je uspješno poslat vlasniku.");
                }
            }
            else
            {
                MessageBox.Show("Ne možete izvršiti rezervaciju za 0 osoba.");
            }
        }

        private void ExecuteReserveAccommodation(object sender)
        {
            guestNumber = int.Parse(GuestNumber.Text);
            accResService = new AccommodationReservationService();
            ExecuteReserveAccommodation(forwardedItem, guestNumber);
        }

        private void ExecuteReserveAccommodation(AccReservationViewModel item, int forwadedGuestNumber)
        {
            if (forwadedGuestNumber > 0)
            {
                if (forwadedGuestNumber > item.AccommodationMaxGuests)
                {
                    MessageBox.Show("Prekoračen je maksimalni broj gostiju za ovaj smeštaj. Pokušajte ponovo.");
                }
                else
                {
                    accResService.AddReservation(item.ReservationFirstDay, item.ReservationLastDay, forwadedGuestNumber,
                                    item.ReservationDuration, item.AccommodationId, LoggedInUser);
                    MessageBox.Show("Uspješno rezervisano.");
                }
            }
            else
            {
                MessageBox.Show("Ne možete izvršiti rezervaciju za 0 osoba.");
            }
        }
    }
}
