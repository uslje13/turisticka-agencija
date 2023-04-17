using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Repositories;
using SOSTeam.TravelAgency.WPF.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SOSTeam.TravelAgency.Application.Services;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest1
{
    public class SearchAccommodationViewModel
    {
        public User LoggedInUser { get; set; }
        public List<Accommodation> accommodations { get; set; }
        public List<Location> locations { get; set; }
        public ObservableCollection<LocAccommodationViewModel> AccommDTOsCollection { get; set; }
        public AccommodationService accommodationService { get; set; }
        public LocationService locationService { get; set; }
        public LocAccommodationViewModel SelectedAccommodationDTO { get; set; }
        public AccommodationReservationService accommodationReservationService { get; set; }
        private SuperOwnerService _superOwnerService;
        public List<AccommodationReservation> accommodationReservations { get; set; }
        public RelayCommand searchCommand { get; set; }
        public RelayCommand reserveCommand { get; set; }
        public Window ThisWindow { get; set; }

        public SearchAccommodationViewModel(User user, Window window)
        {
            AccommDTOsCollection = new ObservableCollection<LocAccommodationViewModel>();

            accommodationService = new AccommodationService();
            _superOwnerService = new();
            locationService = new LocationService();
            accommodationReservationService = new AccommodationReservationService();

            accommodations = accommodationService.GetAll();
            locations = locationService.GetAll();
            accommodationReservations = accommodationReservationService.GetAll();
            LoggedInUser = user;
            ThisWindow = window;

            searchCommand = new RelayCommand(ExecuteSearchAccommodation);
            reserveCommand = new RelayCommand(ExecuteReserveAccommodation);

            CreateAllDTOForms();
        }

        public void ExecuteSearchAccommodation(object sender)
        {
            SearchWindow searchWindow = new SearchWindow(LoggedInUser);
            searchWindow.ShowDialog();
            ThisWindow.Close();
        }


        private void CreateAllDTOForms()
        {
            List<LocAccommodationViewModel> locAccommodationViewModels = new List<LocAccommodationViewModel>();
            AccommDTOsCollection.Clear();
            foreach (var accommodation in accommodations)
            {
                foreach (var location in locations)
                {
                    if (accommodation.LocationId == location.Id)
                    {
                        LocAccommodationViewModel dto = CreateDTOForm(accommodation, location);

                        locAccommodationViewModels.Add(dto);
                    }
                }
            }

            locAccommodationViewModels = locAccommodationViewModels.OrderByDescending(a => a.IsSuperOwned).ToList();

            locAccommodationViewModels.ForEach(item => AccommDTOsCollection.Add(item));
        }

        private LocAccommodationViewModel CreateDTOForm(Accommodation acc, Location loc)
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
                                                        acc.MaxGuests, acc.MinDaysStay, currentGuestNumber, _superOwnerService.IsSuperOwnerAccommodation(acc.Id));
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

        public void ExecuteReserveAccommodation(object sender)
        {
            if (SelectedAccommodationDTO != null)
            {
                EnterReservationWindow newWindow = new EnterReservationWindow(SelectedAccommodationDTO, LoggedInUser, false);
                newWindow.ShowDialog();
                ThisWindow.Close();
            }
            else
            {
                MessageBox.Show("Morate da odaberete smeštaj za rezervaciju.");
            }
        }
    }
}
