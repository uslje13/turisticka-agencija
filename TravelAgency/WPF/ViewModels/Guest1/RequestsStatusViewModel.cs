using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.Views;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest1
{
    public class RequestsStatusViewModel
    {
        public List<ChangedReservationRequest> changedReservationRequests {  get; set; }
        public AccommodationReservationService accommodationReservationService { get; set; }
        public LocationService locationService { get; set; }
        public AccommodationService accommodationService { get; set; }
        public ChangedReservationRequestService changedReservationRequestService { get; set; }

        public List<AccommodationReservation> accommodationReservations { get; set; }
        public List<Location> locations { get; set; }
        public List<Accommodation> accommodations { get; set; }
        public List<LocAccommodationViewModel> locAccommodationViewModels { get; set; }
        public User LoggedInUser { get; set; }
        public ChangedReservationRequest selectedReservation { get; set; }
        public AccommodationReservation selectedAccReservationCopy { get; set; }
        public RelayCommand FindNewDateCommand { get; set; }


        public RequestsStatusViewModel(User user) 
        { 
            LoggedInUser = user;

            accommodationReservationService = new AccommodationReservationService();
            locationService = new LocationService();
            accommodationService = new AccommodationService();
            changedReservationRequestService = new ChangedReservationRequestService();

            accommodationReservations = accommodationReservationService.GetAll();
            locations = locationService.GetAll();
            accommodations = accommodationService.GetAll();
            locAccommodationViewModels = new List<LocAccommodationViewModel>();
            changedReservationRequests = changedReservationRequestService.GetAll();

            PrepareAccommodationReservationsList();
            PrepareReservationsForChangeRequest();

            FindNewDateCommand = new RelayCommand(ExecuteFindNewDate);
        }

        private void PrepareAccommodationReservationsList()
        {
            List<AccommodationReservation> accommodationReservationsCopy = new List<AccommodationReservation>();

            foreach(var res in accommodationReservations)
            {
                accommodationReservationsCopy.Add(res);
            }

            foreach(var res in accommodationReservationsCopy)
            {
                foreach(var ch in changedReservationRequests)
                {
                    if(res.Id == ch.reservationId)
                    {
                        accommodationReservations.Remove(res);
                    }
                }
            }
        }

        private void ExecuteFindNewDate(object sender)
        {
            if(selectedReservation != null)
            {
                if (selectedReservation.status == ChangedReservationRequest.Status.NOT_REQUIRED)
                {
                    PrepareReservationCSV();
                    LocAccommodationViewModel model = FindModel(selectedReservation);
                    EnterReservationWindow newWindow = new EnterReservationWindow(model, LoggedInUser, true, selectedReservation);
                    newWindow.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Odabrana rezervacija je već procesuirana!");
                }
            }
            else
            {
                MessageBox.Show("Odaberite rezervaciju za procesuiranje.");
            }
        }

        private void PrepareReservationCSV()
        {
            foreach(var res in accommodationReservations)
            {
                if(res.Id == selectedReservation.reservationId)
                {
                    selectedAccReservationCopy = res;
                    //accommodationReservationService.Delete(res.Id);
                    break;
                }
            }
        }

        private LocAccommodationViewModel FindModel(ChangedReservationRequest model)
        {
            foreach(var dto in locAccommodationViewModels)
            {
                if(dto.AccommodationId == model.AccommodationId)
                {
                    return dto;
                }
            }

            return null;
        }

        private void PrepareReservationsForChangeRequest()
        {
            MergeLocationsAndAccommodations();
            foreach (var res in accommodationReservations)
            {
                foreach(var model in locAccommodationViewModels)
                {
                    if(IsValid(res, model)) 
                    {
                        ChangedReservationRequest request = new ChangedReservationRequest(res.Id, res.AccommodationId, model.AccommodationName, model.LocationCity, model.LocationCountry,
                                                                                            res.FirstDay, res.LastDay, res.ReservationDuration, res.GuestNumber, res.UserId);
                        changedReservationRequests.Add(request);
                    }
                }
            }
        }

        private bool IsValid(AccommodationReservation res, LocAccommodationViewModel model)
        {
            foreach(var request in changedReservationRequests)
            {
                if(request.reservationId == res.Id)
                {
                    return false;
                }
            }

            return res.AccommodationId == model.AccommodationId && res.UserId == LoggedInUser.Id && res.FirstDay.DayOfYear > DateTime.Today.DayOfYear;
        }

        private void MergeLocationsAndAccommodations()
        {
            locAccommodationViewModels.Clear();
            foreach (var accommodation in accommodations)
            {
                foreach (var location in locations)
                {
                    if (accommodation.LocationId == location.Id)
                    {
                        LocAccommodationViewModel dto = CreateLocAccForm(accommodation, location);

                        locAccommodationViewModels.Add(dto);
                    }
                }
            }
        }

        private LocAccommodationViewModel CreateLocAccForm(Accommodation acc, Location loc)
        {
            int currentGuestNumber = 0;
            foreach (var item in accommodationReservations)
            {
                if (item.AccommodationId == acc.Id)
                {
                    DateTime today = DateTime.Today;
                    int helpVar1 = today.DayOfYear - item.FirstDay.DayOfYear;
                    int helpVar2 = today.DayOfYear - item.LastDay.DayOfYear;
                    if (helpVar1 >= 0 && helpVar2 <= 0)
                    {
                        currentGuestNumber += item.GuestNumber;
                    }
                }
            }
            LocAccommodationViewModel dto = new LocAccommodationViewModel(acc.Id, acc.Name, loc.City, loc.Country, FindAccommodationType(acc),
                                                        acc.MaxGuests, acc.MinDaysStay, currentGuestNumber);
            return dto;
        }

        private LocAccommodationViewModel.AccommType FindAccommodationType(Accommodation acc)
        {
            if (acc.Type == Accommodation.AccommodationType.APARTMENT)
                return LocAccommodationViewModel.AccommType.APARTMENT;
            else if (acc.Type == Accommodation.AccommodationType.HOUSE)
                return LocAccommodationViewModel.AccommType.HOUSE;
            else if (acc.Type == Accommodation.AccommodationType.HUT)
                return LocAccommodationViewModel.AccommType.HUT;
            else
                return LocAccommodationViewModel.AccommType.NOTYPE;
        }
    }
}
