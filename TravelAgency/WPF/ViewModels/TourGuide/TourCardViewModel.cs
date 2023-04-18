using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOSTeam.TravelAgency.Domain.Models;

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

        public void SetCanCancel(Appointment appointment)
        {
            DateTime now = DateTime.Now;
            DateTime start = new DateTime(appointment.Date.Year, appointment.Date.Month, appointment.Date.Day,
                                          appointment.Time.Hour, appointment.Time.Minute, appointment.Time.Second);
            TimeSpan timeDifference = start - now;

            if (timeDifference.TotalHours < 48)
            {
                CancelImage = "/Resources/Icons/cancel_light.png";
                CanCancel = false;

            }
            else
            {
                CancelImage = "/Resources/Icons/cancel.png";
                CanCancel = true;
            }
        }

        public void SetCanStart(Appointment? appointment)
        {
            if (appointment != null || Status == "Finished")
            {
                CanStart = false;
            }
            else
            {
                CanStart = true;
            }
        }

        public void SetAppointmentStatus(Appointment appointment)
        {
            if (!appointment.Started && !appointment.Finished)
            {
                Status = "Not started";
            }
            else if (appointment.Started && !appointment.Finished)
            {
                Status = "Active";
            }
            else if (appointment.Started && appointment.Finished)
            {
                Status = "Finished";
            }
            else
            {
                Status = "Expired";
            }
        }

        public void SetLocation(Location location)
        {
            Location = location.City + ", " + location.Country;
            LocationId = location.Id;
        }

        public void SetCoverImage(Image? image)
        {
            CoverImagePath = image != null ? image.Path : "/Resources/Images/UnknownPhoto.png";
        }

    }
}
