using SOSTeam.TravelAgency.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Owner
{
    internal class HomePageViewModel
    {
        public string Username { get; private set; }
        public User LoggedInUser { get; private set; }
        public HomePageViewModel(User user)
        {
            LoggedInUser = user;
            Username = user.Username;
        }
    }
}
