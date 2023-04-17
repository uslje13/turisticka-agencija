using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon.Primitives;
using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Domain.RepositoryInterfaces;
using SOSTeam.TravelAgency.WPF.Views;
using static System.Net.Mime.MediaTypeNames;

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
        public RelayCommand FindNewDateCommand { get; set; }
        public RelayCommand CancelReservationCommand { get; set; }
        public CancelAndMarkResViewModel selectedCancelReservation { get; set; }
        public List<CancelAndMarkResViewModel> allReservationsInfoList { get; set; }
        public Window ThisWindow { get; set; }

        public RequestsStatusViewModel(User user, Window window) 
        { 
            LoggedInUser = user;
            ThisWindow = window;

            accommodationReservationService = new AccommodationReservationService();
            locationService = new LocationService();
            accommodationService = new AccommodationService();
            changedReservationRequestService = new ChangedReservationRequestService();
            
            allReservationsInfoList = new List<CancelAndMarkResViewModel>();
            locAccommodationViewModels = new List<LocAccommodationViewModel>();
            changedReservationRequests = new List<ChangedReservationRequest>();
            accommodationReservations = accommodationReservationService.GetAll();
            locations = locationService.GetAll();
            accommodations = accommodationService.GetAll();

            AddExistingItems();
            PrepareAccommodationReservationsList();
            PrepareReservationsForChangeRequest();
            PrepareCancelReservationList();

            FindNewDateCommand = new RelayCommand(ExecuteFindNewDate);
            CancelReservationCommand = new RelayCommand(ExecuteCancelReservation);
             
        }

        private void AddExistingItems()
        {
            List<ChangedReservationRequest> helpList = changedReservationRequestService.GetAll();
            if (helpList.Count > 0)
            {
                foreach (var item in helpList)
                {
                    if (LoggedInUser.Id == item.UserId)
                    {
                        changedReservationRequests.Add(item);
                    }
                }
            }
        }

        private void ExecuteCancelReservation(object sender)
        {
            if(selectedCancelReservation != null)
            {
                CancelReservation(selectedCancelReservation);
            }
            else
            {
                MessageBox.Show("Odaberite rezervaciju koju želite otkazati.");
            }
        }

        public void CancelReservation(CancelAndMarkResViewModel selectedReservation)
        {
            Accommodation accommodation = accommodationService.GetById(selectedReservation.AccommodationId);
            int difference = selectedReservation.FirstDay.DayOfYear - DateTime.Today.DayOfYear;

            if (difference > 1)
            {
                if (difference >= accommodation.MinDaysForCancelation)
                {
                    accommodationReservationService.CancelReservation(selectedReservation);
                    MessageBox.Show("Uspješno otkazano!");
                    ThisWindow.Close();
                }
                else
                {
                    MessageBox.Show("Odabrana rezervacija se ne može otkazati zbog postavljenog vlasnikovog ograničenja za otkazivanje od " +
                                    accommodation.MinDaysForCancelation.ToString() + " dana do početka rezervacije.");
                }
            }
            else
            {
                MessageBox.Show("Odabrana rezervacija se ne može otkazati jer počinje sutra.");
            }
        }

        private void PrepareCancelReservationList()
        {
            foreach(var reserv in accommodationReservations)
            {
                foreach(var lavm in locAccommodationViewModels)
                {
                    bool isInFuture = reserv.AccommodationId == lavm.AccommodationId && reserv.FirstDay.DayOfYear > DateTime.Today.DayOfYear && reserv.UserId == LoggedInUser.Id;
                    if (isInFuture)
                    {
                        CancelAndMarkResViewModel crModel = new CancelAndMarkResViewModel(lavm.AccommodationName, lavm.LocationCity,
                                                                                          lavm.LocationCountry, reserv.FirstDay, reserv.LastDay,
                                                                                          reserv.Id, lavm.AccommodationId);
                        allReservationsInfoList.Add(crModel);
                    }
                }
            }
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
                    if(HaveToBeDeleted(res, ch))
                    {
                        accommodationReservationService.Delete(res.Id);
                    }
                }
            }
        }

        private bool HaveToBeDeleted(AccommodationReservation res, ChangedReservationRequest ch)
        {
            return res.Id == ch.reservationId && ch.status != ChangedReservationRequest.Status.ACCEPTED && ch.status != ChangedReservationRequest.Status.REFUSED;
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
                    ThisWindow.Close();
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
                    accommodationReservationService.SaveToOtherCSV(res);
                    accommodationReservationService.Delete(res.Id);
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
                                                                                            res.FirstDay, res.LastDay, res.GuestNumber, res.UserId);
                        changedReservationRequests.Add(request);
                    }
                }
            }
        }

        private bool IsValid(AccommodationReservation res, LocAccommodationViewModel model)
        {
            foreach(var request in changedReservationRequests)
            {
                bool notValid = request.reservationId == res.Id && request.status != ChangedReservationRequest.Status.ACCEPTED && request.status != ChangedReservationRequest.Status.REFUSED;
                if (notValid)
                    return false;
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
                                                        acc.MaxGuests, acc.MinDaysStay, currentGuestNumber, false);
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
