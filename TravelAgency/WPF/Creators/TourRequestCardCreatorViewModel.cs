using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.ViewModels.TourGuide;

namespace SOSTeam.TravelAgency.WPF.Creators
{
    public class TourRequestCardCreatorViewModel
    {
        private readonly TourRequestService _tourRequestService;

        public TourRequestCardCreatorViewModel()
        {
            _tourRequestService = new TourRequestService();
            _tourRequestService.UpdateInvalidRequests();
        }

        public ObservableCollection<TourRequestCardViewModel> CreateTourRequestCards()
        {
            var tourRequestCards = new ObservableCollection<TourRequestCardViewModel>();

            foreach (var tourRequest in _tourRequestService.GetAllOnHold())
            {
                var tourRequestCard = new TourRequestCardViewModel
                {
                    Language = tourRequest.Language,
                    NumOfGuests = tourRequest.MaxNumOfGuests,
                    Location = tourRequest.Country + ", " + tourRequest.City,
                    MinDate = new DateTime(tourRequest.MaintenanceStartDate.Year, tourRequest.MaintenanceStartDate.Month, tourRequest.MaintenanceStartDate.Day),
                    MaxDate = new DateTime(tourRequest.MaintenanceEndDate.Year, tourRequest.MaintenanceEndDate.Month, tourRequest.MaintenanceEndDate.Day)
                };
                tourRequestCards.Add(tourRequestCard);
            }

            return tourRequestCards;
        }

        public ObservableCollection<TourRequestCardViewModel> SearchTourRequests(string country, string city, int numOfGuests,
                                                                                 string language, DateTime minDate, DateTime maxDate)
        {
            var searchDateRange = new DateRange(minDate, maxDate);



            var tourRequestCards = new ObservableCollection<TourRequestCardViewModel>();

            foreach (var tourRequest in _tourRequestService.GetAllOnHold())
            {
                var requestDateRange = FindRequestDateRange(tourRequest);

                if (!requestDateRange.IsOutOfRange(searchDateRange))
                {
                    var tourRequestCard = new TourRequestCardViewModel
                    {
                        Language = tourRequest.Language,
                        NumOfGuests = tourRequest.MaxNumOfGuests,
                        Location = tourRequest.Country + ", " + tourRequest.City,
                        MinDate = new DateTime(tourRequest.MaintenanceStartDate.Year, tourRequest.MaintenanceStartDate.Month, tourRequest.MaintenanceStartDate.Day),
                        MaxDate = new DateTime(tourRequest.MaintenanceEndDate.Year, tourRequest.MaintenanceEndDate.Month, tourRequest.MaintenanceEndDate.Day)
                    };
                    tourRequestCards.Add(tourRequestCard);
                }
            }

            return tourRequestCards;
        }

        private DateRange FindRequestDateRange(TourRequest tourRequest)
        {
            var maintenanceStartDate = new DateTime(tourRequest.MaintenanceStartDate.Year,
                tourRequest.MaintenanceStartDate.Month, tourRequest.MaintenanceStartDate.Day);

            var maintenanceEndDate = new DateTime(tourRequest.MaintenanceEndDate.Year,
                tourRequest.MaintenanceEndDate.Month, tourRequest.MaintenanceEndDate.Day);

            var requestDateRange = new DateRange(maintenanceStartDate, maintenanceEndDate);

            return requestDateRange;
        }
    }
}
