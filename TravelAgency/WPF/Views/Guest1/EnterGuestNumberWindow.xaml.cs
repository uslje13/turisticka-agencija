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
            //accommodationReservationRepository = new AccommodationReservationRepository();
            //accommodationReservations = accommodationReservationRepository.GetAll();
        }

        private void ExecuteSendRequestForChange(object sender)
        {
            guestNumber = int.Parse(GuestNumber.Text);
            accResService = new AccommodationReservationService(forwardedItem, LoggedInUser, guestNumber, ChangedReservationRequest);
            accResService.ExecuteSendRequestForChange();
        } 

        private void ExecuteReserveAccommodation(object sender)
        {
            guestNumber = int.Parse(GuestNumber.Text);
            accResService = new AccommodationReservationService(forwardedItem, LoggedInUser, guestNumber);
            accResService.ExecuteReserveAccommodation();
        }

        /*
        private void AddReservation(DateTime start, DateTime end, int guests, int days, int accId)
        {
            AccommodationReservation reservation = new AccommodationReservation(start, end, days, guests, accId, LoggedInUser.Id);
            AccommodationReservationRepository reservationRepository = new AccommodationReservationRepository();
            reservationRepository.Save(reservation);
            MessageBox.Show("Uspešno rezervisano.");
            Close();
        }

        private void Reserve(object sender, RoutedEventArgs e)
        {
            int appropriateGuestNumber = FindAppropriateGuestsNumber();

            int guestNumber = int.Parse(GuestNumber.Text);
            if(guestNumber > 0)
            {
                int helpVar = appropriateGuestNumber + guestNumber;
                if (helpVar > forwardedItem.AccommodationMaxGuests)
                {
                    MessageBox.Show("Prekoračen je maksimalni broj gostiju za ovaj smeštaj. Pokušajte ponovo.");
                }
                else
                {
                    AddReservation(forwardedItem.ReservationFirstDay, forwardedItem.ReservationLastDay, guestNumber,
                                    forwardedItem.ReservationDuration, forwardedItem.AccommodationId);
                }
            }
            else
            {
                MessageBox.Show("Ne možete izvršiti rezervaciju za 0 osoba.");
            }
        }

        private int FindAppropriateGuestsNumber()
        {
            int appropriateGuestNumber = 0;
            foreach (var item in accommodationReservations)
            {
                if (item.AccommodationId == forwardedItem.AccommodationId)
                {
                    DateTime today = DateTime.Today;
                    int helpVar1 = today.DayOfYear - forwardedItem.ReservationFirstDay.DayOfYear;
                    int helpVar2 = today.DayOfYear - forwardedItem.ReservationLastDay.DayOfYear;
                    if (helpVar1 >= 0 && helpVar2 <= 0)
                    {
                        appropriateGuestNumber += item.GuestNumber;
                    }
                }
            }

            return appropriateGuestNumber;
        }
        */
    }
}
