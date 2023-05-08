using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.Views;
using SOSTeam.TravelAgency.WPF.Views.Guest1;
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
        public RelayCommand GoBackCommand { get; set; }
        public bool IsEnterOfGhange { get; set; }
        public ChangedReservationRequest SelectedReservation { get; set; }
        public AccommodationReservationService accResService { get; set; }
        public TextBox Days { get; set; }
        public DatePicker FirstDay { get; set; }
        public DatePicker LastDay { get; set; }
        public Frame ThisFrame { get; set; }

        public EnterReservationViewModel(LocAccommodationViewModel dto, User user, bool enter, TextBox tb, DatePicker fDay, DatePicker lDay, Frame frame)
        {
            DTO = dto;
            FirstDate = DateTime.Now;
            LastDate = DateTime.Now;
            FirstDay = fDay;
            LastDay = lDay;
            LoggedInUser = user;
            IsEnterOfGhange = enter;
            Days = tb;
            ThisFrame = frame;

            FirstDay.BlackoutDates.AddDatesInPast();
            LastDay.BlackoutDates.AddDatesInPast();

            accResService = new AccommodationReservationService();  

            searchDatesCommand = new RelayCommand(ExecuteSearchingDates);
            GoBackCommand = new RelayCommand(Execute_GoBack);
        }

        public EnterReservationViewModel(LocAccommodationViewModel dto, User user, bool enter, TextBox tb, DatePicker fDay, DatePicker lDay, ChangedReservationRequest request, Frame frame)
        {
            DTO = dto;
            FirstDate = DateTime.Now;
            LastDate = DateTime.Now;
            FirstDay = fDay;
            LastDay = lDay;
            LoggedInUser = user;
            IsEnterOfGhange = enter;
            Days = tb;
            SelectedReservation = request;
            ThisFrame = frame;

            FirstDay.BlackoutDates.AddDatesInPast();
            LastDay.BlackoutDates.AddDatesInPast();

            accResService = new AccommodationReservationService();

            searchDatesCommand = new RelayCommand(ExecuteSearchingDates);
            GoBackCommand = new RelayCommand(Execute_GoBack);
        }

        private void ExecuteSearchingDates(object sender)
        {
            accResService = new AccommodationReservationService();
            ExecuteSearchingDates(DTO, LoggedInUser, FirstDate, LastDate, int.Parse(Days.Text), IsEnterOfGhange, SelectedReservation);
        }

        public void Execute_GoBack(object sender)
        {
            if(IsEnterOfGhange)
            {
                GetOldCSVLists();
                var navigationService = ThisFrame.NavigationService;
                navigationService.GoBack();
            } 
            else
            {
                var navigationService = ThisFrame.NavigationService;
                navigationService.GoBack();
            }
        }

        private void GetOldCSVLists()
        {
            List<AccommodationReservation> shortDeletedList = accResService.LoadFromOtherCSV();
            foreach (var item in shortDeletedList)
            {
                if (item.Id == SelectedReservation.reservationId && !item.DefinitlyChanged)
                {
                    accResService.DeleteFromOtherCSV(item);
                    accResService.SaveOld(item);
                    break;
                }
            }
        }

        public void ExecuteSearchingDates(LocAccommodationViewModel dto, User user, DateTime fDay, DateTime lDay, int days, bool isEnteredOfChange, ChangedReservationRequest request)
        {
            bool validDates = accResService.CheckDates(fDay, lDay);
            bool validDays = accResService.CheckDays(dto, days);
            if (validDates && validDays)
            {
                var navigationService = ThisFrame.NavigationService;
                navigationService.Navigate(new ShowAvailableDatesPage(dto, fDay, lDay, days, user, isEnteredOfChange, request, ThisFrame));
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
