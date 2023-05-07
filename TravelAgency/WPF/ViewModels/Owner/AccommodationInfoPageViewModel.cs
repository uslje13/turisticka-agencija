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
        public Image Image { get; private set; }
        private LocationService _locationService;
        private ImageService _imageService;

        public AccommodationInfoPageViewModel(User user, Accommodation accommodation)
        {
            LoggedInUser = user;
            Accommodation = accommodation;
            _locationService = new();
            _imageService = new();
            Image = _imageService.GetAccommodationCover(accommodation.Id);
            if (Image == null) 
            {
                Image = new Image();
                Image.Path = "/Resources/Images/UnknownPhoto.png";
            } 


            Location = _locationService.GetFullName(_locationService.GetById(Accommodation.LocationId));
        }

    }
}
