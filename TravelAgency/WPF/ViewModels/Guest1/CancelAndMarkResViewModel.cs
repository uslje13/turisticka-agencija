using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest1
{
    public class CancelAndMarkResViewModel
    {
        public string AccommodationName { get; set; }
        public string AccommodationCity { get; set; }
        public string AccommodationCountry { get; set; }
        public DateTime FirstDay { get; set; }
        public DateTime LastDay { get; set; }
        public int ReservationId { get; set; }
        public int AccommodationId { get; set; }
        public string DaysForMarking { get; set; }

        public CancelAndMarkResViewModel() { }

        public CancelAndMarkResViewModel(string accommodationName, string accommodationCity, string accommodationCountry, DateTime firstDay, DateTime lastDay, int reservationId, int accommodationId, string daysForMarking = "")
        {
            AccommodationName = accommodationName;
            AccommodationCity = accommodationCity;
            AccommodationCountry = accommodationCountry;
            FirstDay = firstDay;
            LastDay = lastDay;
            ReservationId = reservationId;
            AccommodationId = accommodationId;
            DaysForMarking = daysForMarking;
        }
    }
}
