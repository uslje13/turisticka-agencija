using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Owner
{

    internal class OwnerReviewPageViewModel : ViewModel
    {
        private GuestAccMarkService _guestAccMarkService;
        private GuestReviewService _guestReviewService;
        private AccommodationService _accommodationService;
        private UserService _userService;
        private MainWindowViewModel _mainwindowVM;

        public User LoggedInUser { get; private set; }
        public double AveridgeGrade { get; set; }
        public ObservableCollection<ReviewViewModel> Reviews { get; set; }
        public ReviewViewModel SelectedReview { get; set; }

        public OwnerReviewPageViewModel(User user, MainWindowViewModel mainWindowVM)
        {
            _guestAccMarkService = new();
            _guestReviewService = new();
            _accommodationService = new();
            _userService = new();
            _mainwindowVM = mainWindowVM;
            LoggedInUser = user;
            Reviews = new();
            FillObservableCollection();
            AveridgeGrade = GetAveridgeGrade();

        }

        private void FillObservableCollection()
        {

            foreach (var mark in GetOwnerMarks())
            {
                if(_guestReviewService.ReviewExists(LoggedInUser.Id,mark.GuestId))
                {
                    Reviews.Add(new ReviewViewModel(
                        mark.Id
                        ,_accommodationService.GetById(mark.AccommodationId).Name
                        ,_userService.GetById(mark.GuestId).Username
                        ,mark.GuestComment
                        ,GetImageUrl(mark.UrlGuestImage)
                        , mark.CleanMark
                        ,mark.OwnerMark
                        ));
                }
                
            }
        }

        private string GetImageUrl(string urls) 
        {
            if(urls.Equals("Nema priloženih slika.")) return "/Resources/Images/UnknownPhoto.png";
            return urls.Split(',')[0] ?? "/Resources/Images/UnknownPhoto.png";
        }

        private List<GuestAccommodationMark> GetOwnerMarks()
        {
            List<GuestAccommodationMark> guestAccommodationMarks = new List<GuestAccommodationMark>();
            foreach (var mark in _guestAccMarkService.GetAll())
            {
                var accommodation = _accommodationService.GetById(mark.AccommodationId);
                
                if (LoggedInUser.Id == accommodation.OwnerId)
                {
                    guestAccommodationMarks.Add(mark);
                }

            }
            return guestAccommodationMarks;
        }

        private double GetAveridgeGrade()
        {
            if(Reviews == null || Reviews.Count < 1) return 0;
            return Math.Round( Reviews.Average(r => r.OwnerMark + r.CleanMark),2);
        }
    }

    class ReviewViewModel
    {
        public int Id { get; set; }
        public string AccommodationName { get; set; }
        public string GuestUsername { get; set; }
        public string GuestComment { get; set; }
        public string PictureUrl { get; set; }
        public int CleanMark { get; set; }
        public int OwnerMark { get; set; }
        public ReviewViewModel(int id, string accommodationName, string guestUsername, string guestComment, string pictureUrl, int cleanMark, int ownerMark)
        {
            Id = id;
            AccommodationName = accommodationName;
            GuestUsername = guestUsername;
            GuestComment = guestComment;
            PictureUrl = pictureUrl;
            CleanMark = cleanMark;
            OwnerMark = ownerMark;
        }
    }
}
