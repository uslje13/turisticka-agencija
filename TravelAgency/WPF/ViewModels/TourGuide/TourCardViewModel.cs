using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.WPF.ViewModels.TourGuide
{
    public class TourCardViewModel
    {
        public int TourId { get; set; }
        public int LocationId { get; set; }
        public int AppointmentId { get; set; }
        public string CoverImagePath { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly Time { get; set; }
        public string Status { get; set; }
        public string CancelImage { get; set; }
        public bool CanCancel { get; set; }
        public bool CanStart { get; set; }

        public TourCardViewModel() { }

        public TourCardViewModel(int tourId, int locationId, int appointmentId, string coverImagePath, string name, string location, DateOnly date, string status, string cancelImage, bool canCancel)
        {
            TourId = tourId;
            LocationId = locationId;
            AppointmentId = appointmentId;
            CoverImagePath = coverImagePath;
            Name = name;
            Location = location;
            Date = date;
            Status = status;
            CancelImage = cancelImage;
            CanCancel = canCancel;
        }
    }
}
