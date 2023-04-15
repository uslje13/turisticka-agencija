using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
using System.Runtime.InteropServices.ObjectiveC;

namespace SOSTeam.TravelAgency.WPF.Views
{
    /// <summary>
    /// Interaction logic for EnterReservation.xaml
    /// </summary>
    public partial class EnterReservationWindow : Window
    {
        public DateTime FirstDate { get; set; }
        public DateTime LastDate { get; set; } 
        public LocAccommodationViewModel DTO { get; set; }
        public User LoggedInUser { get; set; }
        
        public RelayCommand searchDatesCommand { get; set; }
        public RelayCommand cancelCommand { get; set; }
        public bool IsEnterOfGhange { get; set; }
        public ChangedReservationRequest selectedReservation { get; set; }
        public AccommodationReservation selectedReservationCopy { get; set; }
        public AccommodationReservationService accResService { get; set; }

        public EnterReservationWindow(LocAccommodationViewModel dto, User user, bool enter)
        {
            InitializeComponent();
            DataContext = this;

            searchDatesCommand = new RelayCommand(ExecuteSearchingDates);
            cancelCommand = new RelayCommand(ExecuteCancelingOfSearchingDates);
            
            DTO = dto;
            FirstDate = DateTime.Now;
            LastDate = DateTime.Now;
            FirstDay.BlackoutDates.AddDatesInPast();
            LastDay.BlackoutDates.AddDatesInPast();
            LoggedInUser = user;
            IsEnterOfGhange = enter;
        }

        public EnterReservationWindow(LocAccommodationViewModel dto, User user, bool enter, ChangedReservationRequest request, AccommodationReservation reservation)
        {
            InitializeComponent();
            DataContext = this;

            searchDatesCommand = new RelayCommand(ExecuteSearchingDates);
            cancelCommand = new RelayCommand(ExecuteCancelingOfSearchingDates);

            DTO = dto;
            FirstDate = DateTime.Now;
            LastDate = DateTime.Now;
            FirstDay.BlackoutDates.AddDatesInPast();
            LastDay.BlackoutDates.AddDatesInPast();
            LoggedInUser = user;
            IsEnterOfGhange = enter;
            selectedReservation = request;
            selectedReservationCopy = reservation;
        }

        private void ExecuteSearchingDates(object sender)
        {
            accResService = new AccommodationReservationService();
            ExecuteSearchingDates(DTO, LoggedInUser, FirstDate, LastDate, int.Parse(Days.Text), IsEnterOfGhange, selectedReservation);
        }

        private void ExecuteCancelingOfSearchingDates(object sender)
        {
            Close();
        }

        public void ExecuteSearchingDates(LocAccommodationViewModel dto, User user, DateTime fDay, DateTime lDay, int days, bool isEnteredOfChange, ChangedReservationRequest request)
        {
            bool validDates = accResService.CheckDates(fDay, lDay);
            bool validDays = accResService.CheckDays(dto, days);
            if (validDates && validDays)
            {
                ShowAvailableDatesWindow availableDates = new ShowAvailableDatesWindow(dto, fDay, lDay, days, user, isEnteredOfChange, request);
                availableDates.Show();
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
