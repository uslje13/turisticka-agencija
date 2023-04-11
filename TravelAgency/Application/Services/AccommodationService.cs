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
        public string searchedAccName { get; set; }
        public string searchedAccCity { get; set; }
        public string searchedAccCountry { get; set; }
        public LocAccommodationViewModel.AccommType searchedAccType { get; set; }
        public int searchedAccGuestsNumber { get; set; }
        public int searchedAccDaysNumber { get; set; }
        public List<Accommodation> accommodations { get; set; }
        public List<Location> locations { get; set; }
        public ObservableCollection<LocAccommodationViewModel> AccommDTOsCollection { get; set; }
        public AccommodationRepository accommodationRepository { get; set; }
        public LocationRepository locationRepository { get; set; }
        public AccommodationReservationRepository accommodationReservationRepository { get; set; }
        public List<AccommodationReservation> accommodationReservations { get; set; }
        public User LoggedInUser { get; set; }

        private readonly IAccommodationRepository _accommodationRepository = Injector.CreateInstance<IAccommodationRepository>();

        public AccommodationService()
        {

        }

        public AccommodationService(User user, string name, string city, string country, LocAccommodationViewModel.AccommType type, int guestNumber, int daysNumber)
        {
            AccommDTOsCollection = new ObservableCollection<LocAccommodationViewModel>();

            accommodationRepository = new AccommodationRepository();
            locationRepository = new LocationRepository();
            accommodationReservationRepository = new AccommodationReservationRepository();

            accommodations = accommodationRepository.GetAll();
            locations = locationRepository.GetAll();
            accommodationReservations = accommodationReservationRepository.GetAll();
            LoggedInUser = user;
            searchedAccName = name;
            searchedAccCity = city;
            searchedAccCountry = country;
            searchedAccType = type;
            searchedAccGuestsNumber = guestNumber;
            searchedAccDaysNumber = daysNumber;
        }

        public void Delete(int id)
        {
            _accommodationRepository.Delete(id);
        }

        public List<Accommodation> GetAll()
        {
            return _accommodationRepository.GetAll();
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

        public void ExecuteAccommodationSearch()
        {
            CreateAllDTOForms();
            LocAccommodationViewModel dtoRequest = CreateDTORequest();
            SearchAndShow(dtoRequest);
        }

        private void CreateAllDTOForms()
        {
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

        private LocAccommodationViewModel CreateDTORequest()
        {
            LocAccommodationViewModel acDTO = new LocAccommodationViewModel(searchedAccName, searchedAccCity, searchedAccCountry, searchedAccType,
                                                            searchedAccGuestsNumber, searchedAccDaysNumber);
            return acDTO;
        }

        private void SearchAndShow(LocAccommodationViewModel request)
        {
            List<LocAccommodationViewModel> searchResult = Search(request);
            ShowResults(searchResult);
        }

        private List<LocAccommodationViewModel> Search(LocAccommodationViewModel request)
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

        private void ShowResults(List<LocAccommodationViewModel> results)
        {
            if (results.Count > 0)
            {
                SearchResultsWindow newWindow = new SearchResultsWindow(results, LoggedInUser);
                newWindow.Show();
            }
            else
            {
                MessageBox.Show("Nepostojeća kombinacija podataka. Pokušajte ponovo.");
            }
        }
    }
}
