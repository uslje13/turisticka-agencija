using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest1
{
    public class EnterReservationViewModel
    {
        public DateTime FirstDate { get; set; }
        public DateTime LastDate { get; set; }
        public LocAccommodationViewModel DTO { get; set; }
        public User LoggedInUser { get; set; }
        public RelayCommand searchDatesCommand { get; set; }
        public RelayCommand cancelCommand { get; set; }
        public bool IsEnterOfGhange { get; set; }
        public ChangedReservationRequest SelectedReservation { get; set; }
        public AccommodationReservationService accResService { get; set; }
        public TextBox Days { get; set; }
        public DatePicker FirstDay { get; set; }
        public DatePicker LastDay { get; set; }
        public Window ThisWindow { get; set; }

        public EnterReservationViewModel(LocAccommodationViewModel dto, User user, bool enter, TextBox tb, DatePicker fDay, DatePicker lDay, Window window)
        {
            DTO = dto;
            FirstDate = DateTime.Now;
            LastDate = DateTime.Now;
            FirstDay = fDay;
            LastDay = lDay;
            ThisWindow = window;
            LoggedInUser = user;
            IsEnterOfGhange = enter;
            Days = tb;

            FirstDay.BlackoutDates.AddDatesInPast();
            LastDay.BlackoutDates.AddDatesInPast();

            searchDatesCommand = new RelayCommand(ExecuteSearchingDates);
            cancelCommand = new RelayCommand(ExecuteCancel);
        }

        public EnterReservationViewModel(LocAccommodationViewModel dto, User user, bool enter, TextBox tb, DatePicker fDay, DatePicker lDay, Window window, ChangedReservationRequest request)
        {
            DTO = dto;
            FirstDate = DateTime.Now;
            LastDate = DateTime.Now;
            FirstDay = fDay;
            LastDay = lDay;
            ThisWindow = window;
            LoggedInUser = user;
            IsEnterOfGhange = enter;
            Days = tb;
            SelectedReservation = request;

            FirstDay.BlackoutDates.AddDatesInPast();
            LastDay.BlackoutDates.AddDatesInPast();

            searchDatesCommand = new RelayCommand(ExecuteSearchingDates);
            cancelCommand = new RelayCommand(ExecuteCancel);
        }

        private void ExecuteSearchingDates(object sender)
        {
            accResService = new AccommodationReservationService();
            ExecuteSearchingDates(DTO, LoggedInUser, FirstDate, LastDate, int.Parse(Days.Text), IsEnterOfGhange, SelectedReservation);
        }

        private void ExecuteCancel(object sender)
        {
            ThisWindow.Close();
        }

        public void ExecuteSearchingDates(LocAccommodationViewModel dto, User user, DateTime fDay, DateTime lDay, int days, bool isEnteredOfChange, ChangedReservationRequest request)
        {
            bool validDates = accResService.CheckDates(fDay, lDay);
            bool validDays = accResService.CheckDays(dto, days);
            if (validDates && validDays)
            {
                ShowAvailableDatesWindow availableDates = new ShowAvailableDatesWindow(dto, fDay, lDay, days, user, isEnteredOfChange, request);
                availableDates.Show();
                ThisWindow.Close();
            }
            else if (!validDates)
            {
                MessageBox.Show("Nevalidan odabir datuma. Pokušajte ponovo.");
            }
            else if (!validDays)
            {
                MessageBox.Show("Unešeni broj dana boravka je manji od minimalnog za izabrani smeštaj.");
            }
        }
    }
}
