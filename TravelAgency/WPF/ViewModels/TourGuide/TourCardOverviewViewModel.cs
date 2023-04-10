using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.WPF.ViewModels.TourGuide
{
    public class TourCardOverviewViewModel
    {
        public int TourId { get; set; }
        public int LocationId { get; set; }
        public int AppointmentId { get; set; }
        public string ImageLocation { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public DateOnly Date { get; set; }
        public bool Started { get; set; }
        public bool Ended { get; set; }

        public TourCardOverviewViewModel() { }

        public TourCardOverviewViewModel(int tourId, int locationId, int appointmentId, string imageLocation, string name, string city, string country, DateOnly date, bool started, bool ended)
        {
            TourId = tourId;
            LocationId = locationId;
            AppointmentId = appointmentId;
            ImageLocation = imageLocation;
            Name = name;
            City = city;
            Country = country;
            Date = date;
            Started = started;
            Ended = ended;
        }
    }
}
