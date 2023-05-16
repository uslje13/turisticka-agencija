using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.Views.Guest1;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest1
{
    public class EnterGuestNumberViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public User LoggedInUser { get; set; }
        public Frame ThisFrame { get; set; }
        public int guestNumber { get; set; }
        private string enteredGuestNumber { get; set; }
        public string EnteredGuestNumber
        {
            get { return enteredGuestNumber; }
            set
            {
                enteredGuestNumber = value;
                OnPropertyChaged("EnteredGuestComment");
            }
        }
        public AccReservationViewModel forwardedItem { get; set; }
        public ChangedReservationRequest ChangedReservationRequest { get; set; }
        public RelayCommand finishReserveCommand { get; set; }
        public RelayCommand GoBackCommand { get; set; }
        
        public EnterGuestNumberViewModel(AccReservationViewModel item, User user, bool enterOfChange, ChangedReservationRequest request, Frame frame)
        {
            forwardedItem = item;
            LoggedInUser = user;
            ChangedReservationRequest = request;
            ThisFrame = frame;

            if (!enterOfChange)
                finishReserveCommand = new RelayCommand(ExecuteReserveAccommodation);
            else
                finishReserveCommand = new RelayCommand(ExecuteSendRequestForChange);

            GoBackCommand = new RelayCommand(Execute_GoBack);
        }

        protected void OnPropertyChaged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Execute_GoBack(object sender)
        {
            var navigationService = ThisFrame.NavigationService;
            navigationService.GoBack();
        }

        private void ExecuteSendRequestForChange(object sender)
        {
            guestNumber = int.Parse(EnteredGuestNumber);
            ExecuteSendRequestForChange(forwardedItem, guestNumber);
        }

        private void ExecuteSendRequestForChange(AccReservationViewModel item, int forwadedGuestNumber)
        {
            AccommodationReservationService accResService = new AccommodationReservationService();
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
                    var navigationService = ThisFrame.NavigationService;
                    navigationService.Navigate(new AllStatusesPage(LoggedInUser, ThisFrame));
                }
            }
            else
            {
                MessageBox.Show("Ne možete izvršiti rezervaciju za 0 osoba.");
            }
        }

        private void ExecuteReserveAccommodation(object sender)
        {
            guestNumber = int.Parse(EnteredGuestNumber);
            ExecuteReserveAccommodation(forwardedItem, guestNumber);
        }

        private void ExecuteReserveAccommodation(AccReservationViewModel item, int forwadedGuestNumber)
        {
            AccommodationReservationService accResService = new AccommodationReservationService();
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
                    var navigationService = ThisFrame.NavigationService;
                    navigationService.Navigate(new AccommodationBidPage(LoggedInUser, ThisFrame));
                }
            }
            else
            {
                MessageBox.Show("Ne možete izvršiti rezervaciju za 0 osoba.");
            }
        }
    }
}
