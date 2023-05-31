using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Owner
{
    public class SuggestionPageViewModel : ViewModel
    {
        private RenovationRecommendationService _renovationRecommendationService;
        private UserService _userService;
        private AccommodationService _accommodationService;

        private ObservableCollection<RenovationRecommendationViewModel> _renovationRecommendations;

        public RenovationRecommendationViewModel SelectedRecommendation { get; set; }

        public ObservableCollection<RenovationRecommendationViewModel> RenovationRecommendations
        {
            get { return _renovationRecommendations; }
            set
            {
                _renovationRecommendations = value;
                OnPropertyChanged("RenovationRecommendations");
            }
        }

        public SuggestionPageViewModel()
        {
             _renovationRecommendationService = new RenovationRecommendationService();
            RenovationRecommendations = new();
            _userService = new();
            _accommodationService = new();

            var a = _renovationRecommendationService.GetAllForUser(App.LoggedUser.Id);
            foreach(var recommendation in a) 
            {
                RenovationRecommendations.Add(new RenovationRecommendationViewModel(
                                              recommendation.Id,
                                              _userService.GetById(recommendation.GuestId).Username,
                                              recommendation.RenovationRank,
                                              recommendation.Comment,
                                              _accommodationService.GetById(recommendation.AccommodationId).Name
                    ));
            }

            
        }


    }

    public class RenovationRecommendationViewModel
    {
        public int Id { get; set; }
        public string Accommodation { get; set; }
        public string Username { get; set; }
        public int RenovationRank { get; set; }
        public string Comment { get; set; }

        public RenovationRecommendationViewModel(int id, string username, int renovationRank, string comment, string accommodation)
        {
            Id = id;
            Username = username;
            RenovationRank = renovationRank;
            Comment = comment;
            Accommodation = accommodation;
        }
    }


}
