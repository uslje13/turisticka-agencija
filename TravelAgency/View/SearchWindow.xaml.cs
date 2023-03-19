using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TravelAgency.DTO;
using TravelAgency.Model;
using TravelAgency.Repository;
using static TravelAgency.DTO.LocAccommodationDTO;

namespace TravelAgency.View
{
    /// <summary>
    /// Interaction logic for SearchWindow.xaml
    /// </summary>
    public partial class SearchWindow : Window
    {
        public string searchedAccName { get; set; }
        public string searchedAccCity { get; set; }
        public string searchedAccCountry { get; set; }
        public AccommType searchedAccType { get; set; }
        public int searchedAccGuestsNumber { get; set; }
        public int searchedAccDaysNumber { get; set; }
        public List<Accommodation> accommodations { get; set; }
        public List<Location> locations { get; set; }
        public ObservableCollection<LocAccommodationDTO> AccommDTOsCollection { get; set; }
        public AccommodationRepository accommodationRepository { get; set; }
        public LocationRepository locationRepository { get; set; }
        public AccommodationReservationRepository accommodationReservationRepository { get; set; }
        public List<AccommodationReservation> accommodationReservations { get; set; }
        public User LoggedInUser { get; set; }

        public SearchWindow(User user)
        {
            InitializeComponent();
            DataContext = this;
            AccommDTOsCollection = new ObservableCollection<LocAccommodationDTO>();
            
            accommodationRepository = new AccommodationRepository();
            locationRepository = new LocationRepository();
            accommodationReservationRepository = new AccommodationReservationRepository();

            accommodations = accommodationRepository.GetAll();
            locations = locationRepository.GetAll();
            accommodationReservations = accommodationReservationRepository.GetAll();
            LoggedInUser = user; 
        }

        private void SearchAccommodationClick(object sender, RoutedEventArgs e)
        {
            CreateAllDTOForms();
            LoadEnteredRequests();
            LocAccommodationDTO dtoRequest = CreateDTORequest();
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
                        LocAccommodationDTO dto = CreateDTOForm(accommodation, location);
                        AccommDTOsCollection.Add(dto);
                    }
                }
            }
        }

        private LocAccommodationDTO CreateDTOForm(Accommodation acc, Location loc)
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
            LocAccommodationDTO dto = new LocAccommodationDTO(acc.Id, acc.Name, loc.City, loc.Country, FindAccommodationType(acc),
                                                        acc.MaxGuests, acc.MinDaysStay, currentGuestNumber);
            return dto;
        }

        private AccommType FindAccommodationType(Accommodation acc)
        {
            if (acc.Type == Accommodation.AccommodationType.APARTMENT)
                return AccommType.APARTMENT;
            else if (acc.Type == Accommodation.AccommodationType.HOUSE)
                return AccommType.HOUSE;
            else if (acc.Type == Accommodation.AccommodationType.HUT)
                return AccommType.HUT;
            else
                return AccommType.NOTYPE;
        }

        private void LoadEnteredRequests()
        {
            searchedAccName = name.Text;
            searchedAccCity = city.Text;
            searchedAccCountry = country.Text;
            searchedAccType = GetSelectedItem(CBTypes);
            string loadedGuestsNumber = guestsNumber.Text;
            searchedAccGuestsNumber = int.Parse(loadedGuestsNumber);
            string loadedDaysNumber = daysNumber.Text;
            searchedAccDaysNumber = int.Parse(loadedDaysNumber);
        }

        private AccommType GetSelectedItem(ComboBox cb)
        {
            if(cb.SelectedItem == CBItemApart) return AccommType.APARTMENT;
            else if(cb.SelectedItem == CBItemHouse) return AccommType.HOUSE;
            else if(cb.SelectedItem==CBItemHut) return AccommType.HUT;
            else return AccommType.NOTYPE;
        }

        private LocAccommodationDTO CreateDTORequest()
        {
            LocAccommodationDTO acDTO = new LocAccommodationDTO(searchedAccName, searchedAccCity, searchedAccCountry, searchedAccType,
                                                            searchedAccGuestsNumber, searchedAccDaysNumber);
            return acDTO;
        }

        private void SearchAndShow(LocAccommodationDTO request)
        {
            List<LocAccommodationDTO> searchResult = Search(request);
            ShowResults(searchResult);
        }

        private List<LocAccommodationDTO> Search(LocAccommodationDTO request)
        {
            List<LocAccommodationDTO> SearchResult = new List<LocAccommodationDTO>();
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

        private bool IsAppropriate(LocAccommodationDTO item, LocAccommodationDTO request)
        {
            bool checkName = item.AccommodationName.ToLower().Contains(request.AccommodationName.ToLower()) || request.AccommodationName.Equals(string.Empty);
            bool checkCity = item.LocationCity.ToLower().Contains(request.LocationCity.ToLower()) || request.LocationCity.Equals(string.Empty);
            bool checkCountry = item.LocationCountry.ToLower().Contains(request.LocationCountry.ToLower()) || request.LocationCountry.Equals(string.Empty);
            bool checkType = item.AccommodationType == request.AccommodationType || request.AccommodationType == AccommType.NOTYPE;
            bool checkMaxGuests = item.GuestNumber + request.GuestNumber <= item.AccommodationMaxGuests;
            bool checkDaysStay = request.AccommodationMinDaysStay >= item.AccommodationMinDaysStay;

            return checkName && checkCity && checkCountry && checkType && checkMaxGuests && checkDaysStay;
        }

        private void ShowResults(List<LocAccommodationDTO> results)
        {
            if (results.Count > 0)
            {
                SearchResultsWindow newWindow = new SearchResultsWindow(results, LoggedInUser);
                newWindow.Show();
                Close();
            }
            else
            {
                MessageBox.Show("Nepostojeća kombinacija podataka. Pokušajte ponovo.");
            }
        }

        private void CancelAccommodationClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}