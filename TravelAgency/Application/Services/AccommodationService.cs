using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Xml.Linq;
using SOSTeam.TravelAgency.Domain;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Domain.RepositoryInterfaces;
using SOSTeam.TravelAgency.WPF.ViewModels.Guest1;
using SOSTeam.TravelAgency.WPF.Views;
using SOSTeam.TravelAgency.Repositories;
using System.Collections.ObjectModel;

namespace SOSTeam.TravelAgency.Application.Services
{
    public class AccommodationService
    {
        private readonly IAccommodationRepository _accommodationRepository = Injector.CreateInstance<IAccommodationRepository>();
        private readonly ILocationRepository _locationRepository = Injector.CreateInstance<ILocationRepository>();
        private readonly IAccReservationRepository _accReservationRepository = Injector.CreateInstance<IAccReservationRepository>();

        public AccommodationService() { }

        public void Delete(int id)
        {
            _accommodationRepository.Delete(id);
        }

        public List<Accommodation> GetAll()
        {
            return _accommodationRepository.GetAll();
        }

        public List<Accommodation> GetAllByUserId(int ownerId)
        { 
            return _accommodationRepository.GetAll().Where(p => p.OwnerId == ownerId).ToList();
        }

        public Accommodation GetById(int id)
        {
            return _accommodationRepository.GetById(id);
        }

        public void Save(Accommodation accommodation)
        {
            _accommodationRepository.Save(accommodation);
        }

        public void Update(Accommodation accommodation)
        {
            _accommodationRepository.Update(accommodation);
        }

        public List<LocAccommodationViewModel> ExecuteAccommodationSearch(string name, string city, string country, LocAccommodationViewModel.AccommType type, int guestNumber, int daysNumber)
        {
            ObservableCollection<LocAccommodationViewModel> AccommDTOsCollection = CreateAllDTOForms();
            LocAccommodationViewModel dtoRequest = CreateDTORequest(name, city, country, type, guestNumber, daysNumber);
            return Search(dtoRequest, AccommDTOsCollection);
        }

        private ObservableCollection<LocAccommodationViewModel> CreateAllDTOForms()
        {
            List<Accommodation> accommodations = _accommodationRepository.GetAll();
            List<Location> locations = _locationRepository.GetAll();
            ObservableCollection<LocAccommodationViewModel>  AccommDTOsCollection = new ObservableCollection<LocAccommodationViewModel>();

            AccommDTOsCollection.Clear();
            foreach (var accommodation in accommodations)
            {
                foreach (var location in locations)
                {
                    if (accommodation.LocationId == location.Id)
                    {
                        LocAccommodationViewModel dto = CreateDTOForm(accommodation, location);
                        AccommDTOsCollection.Add(dto);
                    }
                }
            }

            return AccommDTOsCollection;
        }

        private LocAccommodationViewModel CreateDTOForm(Accommodation acc, Location loc)
        {
            List<AccommodationReservation> accommodationReservations = _accReservationRepository.GetAll();

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
                                                        acc.MaxGuests, acc.MinDaysStay, currentGuestNumber,false);
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

        private LocAccommodationViewModel CreateDTORequest(string searchedAccName, string searchedAccCity, string searchedAccCountry, LocAccommodationViewModel.AccommType searchedAccType, int searchedAccGuestsNumber, int searchedAccDaysNumber)
        {
            LocAccommodationViewModel acDTO = new LocAccommodationViewModel(searchedAccName, searchedAccCity, searchedAccCountry, searchedAccType,
                                                            searchedAccGuestsNumber, searchedAccDaysNumber,false);
            return acDTO;
        }

        private List<LocAccommodationViewModel> Search(LocAccommodationViewModel request, ObservableCollection<LocAccommodationViewModel> AccommDTOsCollection)
        {
            List<LocAccommodationViewModel> SearchResult = new List<LocAccommodationViewModel>();
            foreach (var item in AccommDTOsCollection)
            {
                bool isCorrect = IsAppropriate(item, request);
                if (isCorrect)
                {
                    SearchResult.Add(item);
                }
            }
            return SearchResult;
        }

        private bool IsAppropriate(LocAccommodationViewModel item, LocAccommodationViewModel request)
        {
            bool checkName = item.AccommodationName.ToLower().Contains(request.AccommodationName.ToLower()) || request.AccommodationName.Equals(string.Empty);
            bool checkCity = item.LocationCity.ToLower().Contains(request.LocationCity.ToLower()) || request.LocationCity.Equals(string.Empty);
            bool checkCountry = item.LocationCountry.ToLower().Contains(request.LocationCountry.ToLower()) || request.LocationCountry.Equals(string.Empty);
            bool checkType = item.AccommodationType == request.AccommodationType || request.AccommodationType == LocAccommodationViewModel.AccommType.NOTYPE;
            bool checkMaxGuests = request.GuestNumber <= item.AccommodationMaxGuests;
            bool checkDaysStay = request.AccommodationMinDaysStay >= item.AccommodationMinDaysStay;

            return checkName && checkCity && checkCountry && checkType && checkMaxGuests && checkDaysStay;
        }
    }
}
