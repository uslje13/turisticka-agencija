using SOSTeam.TravelAgency.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.WPF.ViewModels.TourGuide
{
    public class GuestAttendanceCardViewModel
    {
        public string Name { get; set; }
        public string StatusImage { get; set; }
        public GuestAttendanceCardViewModel() { }
        public GuestAttendanceCardViewModel(string name, string statusImage)
        {
            Name = name;
            StatusImage = statusImage;
        }

        public void SetStatusImage(GuestAttendance guestAttendance)
        {
            if (guestAttendance.Presence == GuestPresence.YES)
            {
                StatusImage = "/Resources/Icons/checkpoint_yes.png";
            }
            else if (guestAttendance.Presence == GuestPresence.NO)
            {
                StatusImage = "/Resources/Icons/checkpoint_no.png";
            }
            else
            {
                StatusImage = "/Resources/Icons/checkpoint_unknown.png";
            }
        }

    }
}
