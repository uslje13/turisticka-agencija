using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Owner
{
    public class AccommodationInfoPageViewModel : ViewModel
    {
        public User LoggedInUser { get; private set; }
        public Accommodation Accommodation { get; private set; }
        public string Location { get; private set; }
        public Image Image { get; private set; }
        public RelayCommand NavigatePhotos { get; private set; }
        private List<Image> _images;
        private LocationService _locationService;
        private ImageService _imageService;
        private int _imageIndex;


        public AccommodationInfoPageViewModel(User user, Accommodation accommodation)
        {
            LoggedInUser = user;
            Accommodation = accommodation;
            _locationService = new();
            _imageService = new();
            _images = _imageService.GetAllForAccommodations().Where(t => t.EntityId == accommodation.Id).ToList();
            if (_images == null) 
            {
                Image = new Image();
                Image.Path = "/Resources/Images/UnknownPhoto.png";
            }
            else 
            {
                _images = _images.OrderByDescending(t => t.Cover).ToList();
                Image = _images.First();
                _imageIndex = 0;
            }


            NavigatePhotos = new RelayCommand(Execute_NavigatePhotos, CanExecuteNavigatePhotos);
            Location = _locationService.GetFullName(_locationService.GetById(Accommodation.LocationId));
        }

        private bool CanExecuteNavigatePhotos(object obj)
        {
            return _images.Count > 1;
        }

        private void Execute_NavigatePhotos(object obj)
        {
            string direction = obj.ToString();
            if (direction.Equals("Left")) 
            {
                _imageIndex = _imageIndex == 0 ? _images.Count - 1 : _imageIndex - 1;
                Image = _images[_imageIndex];
            }
            else 
            {
                _imageIndex = _imageIndex == _images.Count - 1 ? 0 : _imageIndex + 1;
                Image = _images[_imageIndex];
            }
            OnPropertyChanged("Image");
        }
    }
}
