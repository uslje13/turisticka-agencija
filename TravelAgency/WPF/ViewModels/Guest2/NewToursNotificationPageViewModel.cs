using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest2
{
    public class NewToursNotificationPageViewModel
    {
        public static User LoggedInUser { get; set; }
        public static ObservableCollection<NewToursViewModel> NewTours { get; set; }

        private readonly TourService _tourService;
        private readonly TourRequestService _tourRequestService;

        public NewToursNotificationPageViewModel(User loggedInUser)
        {
            _tourService = new TourService();
            _tourRequestService= new TourRequestService();
            LoggedInUser= loggedInUser;
            NewTours = new ObservableCollection<NewToursViewModel>();
            FillNewToursList();
        }

        private void FillNewToursList()
        {
            foreach(var tour in _tourService.GetAll())
            {
                if(tour.LocationId == 17)
                {
                    NewTours.Add(new NewToursViewModel(LoggedInUser, tour.Id, tour.Name));
                }
            }
        }
    }
}
