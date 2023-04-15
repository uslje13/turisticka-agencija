using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOSTeam.TravelAgency.Repositories.Serializer;
using SOSTeam.TravelAgency.WPF.ViewModels;
using SOSTeam.TravelAgency.WPF.ViewModels.Guest1;
using static SOSTeam.TravelAgency.Domain.Models.ChangedReservationRequest;

namespace SOSTeam.TravelAgency.Domain.Models
{
    public class WantedNewDate : ISerializable
    {
        public int Id { get; set; }
        public AccReservationViewModel wantedDate { get; set; }
        public int UserId { get; set; }
        public int OldReservationId { get; set; }

        public WantedNewDate(int id, string name, int days, DateTime fDay, DateTime lDay, int duration, int maxGuests, int currGuests, int userId, int oldReservationId)
        {
            wantedDate = new AccReservationViewModel(id, name, days, fDay, lDay, duration, maxGuests, currGuests);
            UserId = userId;
            OldReservationId = oldReservationId;
        }

        public WantedNewDate()
        {
            wantedDate = new AccReservationViewModel();
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), wantedDate.AccommodationId.ToString(), wantedDate.AccommodationName,  wantedDate.AccommodationMinDaysStay.ToString(), 
                                    wantedDate.ReservationFirstDay.ToString(), wantedDate.ReservationLastDay.ToString(), 
                                    wantedDate.ReservationDuration.ToString(), wantedDate.AccommodationMaxGuests.ToString(), wantedDate.CurrentGuestNumber.ToString(), 
                                    UserId.ToString(), OldReservationId.ToString()};
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            int i = 0;
            Id = Convert.ToInt32(values[i++]);
            wantedDate.AccommodationId = Convert.ToInt32(values[i++]);
            wantedDate.AccommodationName = values[i++];
            wantedDate.AccommodationMinDaysStay = Convert.ToInt32(values[i++]);
            wantedDate.ReservationFirstDay = Convert.ToDateTime(values[i++]);
            wantedDate.ReservationLastDay = Convert.ToDateTime(values[i++]);
            wantedDate.ReservationDuration = Convert.ToInt32(values[i++]);
            wantedDate.AccommodationMaxGuests = Convert.ToInt32(values[i++]);
            wantedDate.CurrentGuestNumber = Convert.ToInt32(values[i++]);
            UserId = Convert.ToInt32(values[i++]);
            OldReservationId = Convert.ToInt32(values[i++]);
        }
    }
}
