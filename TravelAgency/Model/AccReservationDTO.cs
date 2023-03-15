using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelAgency.Model
{
    public class AccReservationDTO
    {
        public enum AccType
        {
            APARTMENT, HOUSE, HUT, NOTYPE
        }
        //public int AccReservationDTOId { get; set; }
        public string AccommodationName { get; set; }
        public AccType AccommodationType { get; set; }
        public int AccommodationMaxGuests { get; set; }
        public int AccommodationMinDaysStay { get; set; }
        public DateOnly ReservationFirstDay { get; set; }
        public DateOnly ReservationLastDay { get; set; }
        public int ReservationDuration { get; set; }

        public AccReservationDTO(string name, AccType type, int guests, int days, DateOnly firstDay, DateOnly lastDay, int duration) 
        { 
            AccommodationName = name;
            AccommodationType = type;
            AccommodationMaxGuests = guests;
            AccommodationMinDaysStay = days;
            ReservationFirstDay = firstDay;
            ReservationLastDay = lastDay;
            ReservationDuration = duration;
        }

        public AccReservationDTO() 
        { 
        
        }
    }
}
