using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest1
{
    public class AccReservationViewModel
    {
        public int AccommodationId { get; set; }
        public string AccommodationName { get; set; }
        public int AccommodationMinDaysStay { get; set; }
        public DateTime ReservationFirstDay { get; set; }
        public DateTime ReservationLastDay { get; set; }
        public int ReservationDuration { get; set; }
        public int AccommodationMaxGuests { get; set; }
        public int CurrentGuestNumber { get; set; }

        public AccReservationViewModel(int id, string name, int days, DateTime firstDay, DateTime lastDay, int duration, int guests, int current)
        {
            AccommodationId = id;
            AccommodationName = name;
            AccommodationMinDaysStay = days;
            ReservationFirstDay = firstDay;
            ReservationLastDay = lastDay;
            ReservationDuration = duration;
            AccommodationMaxGuests = guests;
            CurrentGuestNumber = current;
        }

        public AccReservationViewModel()
        {

        }
    }
}
