using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest1
{
    public class EnterGuestNumberViewModel
    {
        public AccReservationViewModel forwardedItem { get; set; }
        public User LoggedInUser { get; set; }
        public AccommodationReservationService accResService { get; set; }
        public int guestNumber { get; set; }
        public RelayCommand finishReserveCommand { get; set; }
        public ChangedReservationRequest ChangedReservationRequest { get; set; }
        public TextBox GuestNumber { get; set; }
        public Window ThisWindow { get; set; }

        public EnterGuestNumberViewModel(AccReservationViewModel item, User user, bool enterOfChange, ChangedReservationRequest request, TextBox tb, Window thisWindow)
        {
            forwardedItem = item;
            LoggedInUser = user;
            ChangedReservationRequest = request;
            GuestNumber = tb;
            ThisWindow = thisWindow;

            accResService = new AccommodationReservationService();

            if (!enterOfChange)
                finishReserveCommand = new RelayCommand(ExecuteReserveAccommodation);
            else
                finishReserveCommand = new RelayCommand(ExecuteSendRequestForChange);
        }

        private void ExecuteSendRequestForChange(object sender)
        {
            guestNumber = int.Parse(GuestNumber.Text);
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
                    ThisWindow.Close();
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
                    ThisWindow.Close();
                }
            }
            else
            {
                MessageBox.Show("Ne možete izvršiti rezervaciju za 0 osoba.");
            }
        }
    }
}
