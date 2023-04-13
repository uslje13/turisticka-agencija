using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Owner
{
    public class AccommodationInfoPageViewModel
    {
        public User LoggedInUser { get; private set; }
        public Accommodation Accommodation { get; private set; }
        public string Location { get; private set; }
        private LocationService _locationService;
        public AccommodationInfoPageViewModel(User user, Accommodation accommodation)
        {
            LoggedInUser = user;
            Accommodation = accommodation;
            _locationService = new();
            Location = _locationService.GetFullName(_locationService.GetById(Accommodation.LocationId));
        }

    }
}
