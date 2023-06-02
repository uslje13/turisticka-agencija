using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Owner
{
    public class ForumInfoPageViewModel
    {
        public string OpenerUsername { get; set; }
        public string Location { get; set; }

        private Forum Forum { get; set; }
        private UserService _userService;
        private LocationService _locationService;
        public ForumInfoPageViewModel(Forum forum)
        {
            _userService = new();
            _locationService = new();

            Forum = forum;
            OpenerUsername = _userService.GetById(forum.UserId).Username;
            var location = _locationService.GetById(forum.LocationId);
            Location = _locationService.GetFullName(location);
        }
    }
}
