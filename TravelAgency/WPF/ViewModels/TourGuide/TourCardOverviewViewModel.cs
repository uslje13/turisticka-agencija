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
        public string ImageUrl { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public DateOnly Date { get; set; }
        public bool Started { get; set; }
        public bool Finished { get; set; }

        public TourCardOverviewViewModel() { }

        public TourCardOverviewViewModel(int tourId, int locationId, int appointmentId, string imageUrl, string name, string location, DateOnly date, bool started, bool finished)
        {
            TourId = tourId;
            LocationId = locationId;
            AppointmentId = appointmentId;
            ImageUrl = imageUrl;
            Name = name;
            Location = location;
            Date = date;
            Started = started;
            Finished = finished;
        }
    }
}
