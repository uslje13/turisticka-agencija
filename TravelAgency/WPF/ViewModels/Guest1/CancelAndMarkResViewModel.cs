using SOSTeam.TravelAgency.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

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
        public LocAccommodationViewModel.AccommType AccommodationType { get; set; }
        public int DaysDuration { get; set; }
        public string NotificationShape { get; set; }

        public CancelAndMarkResViewModel() 
        {
            //NotificationShape = "Nemate rezervacija za ocjenjivanje smještaja.";
        }

        public CancelAndMarkResViewModel(string accommodationName, string accommodationCity, string accommodationCountry, DateTime firstDay, DateTime lastDay, int reservationId, int accommodationId, string daysForMarking = "", int daysDuration = -1, LocAccommodationViewModel.AccommType type = LocAccommodationViewModel.AccommType.NOTYPE)
        {
            AccommodationName = accommodationName;
            AccommodationCity = accommodationCity;
            AccommodationCountry = accommodationCountry;
            FirstDay = firstDay;
            LastDay = lastDay;
            ReservationId = reservationId;
            AccommodationId = accommodationId;
            DaysForMarking = daysForMarking;
            DaysDuration = daysDuration;
            AccommodationType = type;
            NotificationShape = "Vaša rezervacija u smještaju " + AccommodationName + " (" + AccommodationCity + ", " +
                                AccommodationCountry + ") za period " + FirstDay.ToShortDateString() + " - " + LastDay.ToShortDateString() +
                                " je završena. Za eventualno ocjenjivanje ovog smještaja Vam je ostalo još ";
        }
    }
}
