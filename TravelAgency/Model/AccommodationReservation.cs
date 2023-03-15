using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TravelAgency.Model.Accommodation;
using System.Xml.Linq;
using TravelAgency.Serializer;

namespace TravelAgency.Model
{
    public class AccommodationReservation : ISerializable
    {
        public int Id { get; set; }
        public DateOnly FirstDay { get; set; }
        public DateOnly LastDay { get; set; }
        public int ReservationDuration { get; set; }
        public int AccommodationId { get; set; }

        public AccommodationReservation() 
        {
            Id = -1;
            FirstDay = new DateOnly();
            LastDay = new DateOnly();
            ReservationDuration = -1;
            AccommodationId = -1;
        }

        public AccommodationReservation(int id, DateOnly firstDay, DateOnly lastDay, int reservationDuration, int accommodationId)
        {
            Id = id;
            FirstDay = firstDay;
            LastDay = lastDay;
            ReservationDuration = reservationDuration;
            AccommodationId = accommodationId;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), FirstDay.ToString(), LastDay.ToString(), ReservationDuration.ToString(), AccommodationId.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            int i = 0;
            Id = Convert.ToInt32(values[i++]);
            FirstDay = DateOnly.Parse(values[i++]);
            LastDay = DateOnly.Parse(values[i++]);
            ReservationDuration = Convert.ToInt32(values[i++]);
            AccommodationId = Convert.ToInt32(values[i++]);
        }
    }
}
