using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.WPF.ViewModels.TourGuide
{

    public class UserAccountViewModel
    {
        public string Username { get; set; }

        public UserAccountViewModel()
        {
            Username = App.LoggedUser.Username;
        }
    }
}
