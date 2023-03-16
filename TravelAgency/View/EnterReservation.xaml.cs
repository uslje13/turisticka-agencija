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
using TravelAgency.Model;
using TravelAgency.Repository;

namespace TravelAgency.View
{
    /// <summary>
    /// Interaction logic for EnterReservation.xaml
    /// </summary>
    public partial class EnterReservation : Window
    {
        public DateTime FirstDate { get; set; }
        public DateTime LastDate { get; set; } 
        public int DaysDuration { get; set; }
        public AccommodationDTO DTO { get; set; }


        public EnterReservation()
        {
            InitializeComponent();
        }

        public EnterReservation(AccommodationDTO dto)
        {
            InitializeComponent();
            DataContext = this;
            DTO = dto;
            FirstDate = DateTime.Now;
            LastDate = DateTime.Now;
            FirstDay.BlackoutDates.AddDatesInPast();
            LastDay.BlackoutDates.AddDatesInPast();
        }

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
                ShowAvailableDates availableDates = new ShowAvailableDates(DTO, FirstDate, LastDate.Date, DaysDuration);
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
    }
}
