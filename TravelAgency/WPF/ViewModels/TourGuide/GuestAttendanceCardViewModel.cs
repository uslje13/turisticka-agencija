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
    }
}
