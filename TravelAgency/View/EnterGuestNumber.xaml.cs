﻿using System;
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
    /// Interaction logic for EnterGuestNumber.xaml
    /// </summary>
    public partial class EnterGuestNumber : Window
    {
        public AccReservationDTO forwardedItem { get; set; }

        public EnterGuestNumber()
        {
            InitializeComponent();
        }

        public EnterGuestNumber(AccReservationDTO item)
        {
            InitializeComponent();
            DataContext = this;
            forwardedItem = item;
        }

        private void AddReservation(DateTime start, DateTime end, int days, int accId)
        {
            AccommodationReservation reservation = new AccommodationReservation(start, end, days, accId);
            AccommodationReservationRepository reservationRepository = new AccommodationReservationRepository();
            reservationRepository.Save(reservation);
            MessageBox.Show("Uspešno rezervisano.");
        }

        private void Reserve(object sender, RoutedEventArgs e)
        {
            int guestNumber = int.Parse(GuestNumber.Text);
            if(guestNumber > forwardedItem.AccommodationMaxGuests) 
            {
                MessageBox.Show("Prekoračen je maksimalni broj gostiju za ovaj smeštaj. Pokušajte ponovo.");
            } 
            else
            {
                AddReservation(forwardedItem.ReservationFirstDay, forwardedItem.ReservationLastDay,
                                forwardedItem.AccommodationMinDaysStay, forwardedItem.AccommodationId);
            }
        }
    }
}
