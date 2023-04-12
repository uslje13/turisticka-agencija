using SOSTeam.TravelAgency.Repositories.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.Domain.Models
{
    public class ChangedReservationRequest : ISerializable
    {
        public enum Status { ACCEPTED, REFUSED, ON_HOLD, NOT_REQUIRED }
        public int Id { get; set; }
        public int reservationId { get; set; }
        public int AccommodationId { get; set; }
        public string AccommodationName { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public DateTime FirstDay { get; set; }
        public DateTime LastDay { get; set; }
        public int ReservationDuration { get; set; }
        public int GuestNumber { get; set; }
        public int UserId { get; set; }
        public Status status { get; set; }
        public string ownerComment { get; set; }

        public ChangedReservationRequest()
        {

        }

        public ChangedReservationRequest(int resId, int accId, string accName, string city, string country, DateTime fDay, DateTime lDay, int duration, int guests, int userId)
        {
            reservationId = resId;
            AccommodationId = accId;
            AccommodationName = accName;
            City = city;
            Country = country;
            FirstDay = fDay;
            LastDay = lDay;
            ReservationDuration = duration;
            GuestNumber = guests;
            UserId = userId;
            status = Status.NOT_REQUIRED;
            ownerComment = "Nema komentara";
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), reservationId.ToString(), AccommodationId.ToString(), AccommodationName, City, Country, 
                                    FirstDay.ToString(), LastDay.ToString(), ReservationDuration.ToString(), GuestNumber.ToString(), UserId.ToString(), status.ToString(), ownerComment };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            int i = 0;
            Id = Convert.ToInt32(values[i++]);
            reservationId = Convert.ToInt32(values[i++]);
            AccommodationId = Convert.ToInt32(values[i++]);
            AccommodationName = values[i++];
            City = values[i++];
            Country = values[i++];
            FirstDay = Convert.ToDateTime(values[i++]);
            LastDay = Convert.ToDateTime(values[i++]);
            ReservationDuration = Convert.ToInt32(values[i++]);
            GuestNumber = Convert.ToInt32(values[i++]);
            UserId = Convert.ToInt32(values[i++]);
            status = FindStatus(values[i++]);
            ownerComment = values[i++];
        }

        private Status FindStatus(string str)
        {
            if (str.Equals("ACCEPTED"))
                return Status.ACCEPTED;
            else if (str.Equals("REFUSED"))
                return Status.REFUSED;
            else if (str.Equals("ON_HOLD"))
                return Status.ON_HOLD;
            else
                return Status.NOT_REQUIRED;
        }
    }
}
