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
        /*
        public int DaysDuration { get; set; }
        */
        public LocAccommodationViewModel DTO { get; set; }
        public User LoggedInUser { get; set; }
        
        public RelayCommand searchDatesCommand { get; set; }
        public RelayCommand cancelCommand { get; set; }

        public EnterReservationWindow(LocAccommodationViewModel dto, User user)
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
        }

        private void ExecuteSearchingDates(object sender)
        {
            AccommodationReservationService accResService = new AccommodationReservationService(DTO, LoggedInUser, FirstDate, LastDate, int.Parse(Days.Text));
            accResService.ExecuteSearchingDates();
        }

        private void ExecuteCancelingOfSearchingDates(object sender)
        {
            Close();
        }

        /*
        private void GoBackClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ReserveClick(object sender, RoutedEventArgs e)
        {
            bool validDates = CheckDates(FirstDate, LastDate);
            bool validDays = CheckDays();
            if (validDates && validDays)
            {
                ShowAvailableDatesWindow availableDates = new ShowAvailableDatesWindow(DTO, FirstDate, LastDate, DaysDuration, LoggedInUser);
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

        private bool CheckDays()
        {
            DaysDuration = int.Parse(Days.Text);
            int check = DTO.AccommodationMinDaysStay;
            if (DaysDuration >= check) return true;
            else return false;
        }

        private bool CheckDates(DateTime start, DateTime end)
        {
            if (start.Year < end.Year) return true;
            else if (start.Year == end.Year)
            {
                if (start.Month < end.Month) return true;
                else if (start.Month == end.Month)
                {
                    if (start.Day <= end.Day) return true;
                    else return false;
                }
                else return false;
            }
            else return false;
        }
        */
    }
}
