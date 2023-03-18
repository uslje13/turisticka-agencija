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
using TravelAgency.Model;
using TravelAgency.Repository;
using static TravelAgency.Model.AccommodationDTOStefan;

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
        public ObservableCollection<AccommodationDTOStefan> AccommDTOsCollection { get; set; }
        public AccommodationRepository accommodationRepository { get; set; }
        public LocationRepository locationRepository { get; set; }

        public SearchWindow()
        {
            InitializeComponent();
            DataContext = this;
            AccommDTOsCollection = new ObservableCollection<AccommodationDTOStefan>();
            
            accommodationRepository = new AccommodationRepository();
            locationRepository = new LocationRepository();

            accommodations = accommodationRepository.GetAll();
            locations = locationRepository.GetAll();
        }

        private void SearchAccommodationClick(object sender, RoutedEventArgs e)
        {
            CreateAllDTOForms();
            LoadEnteredRequests();
            AccommodationDTOStefan dtoRequest = CreateDTORequest();
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
                        AccommodationDTOStefan dto = CreateDTOForm(accommodation, location);
                        AccommDTOsCollection.Add(dto);
                    }
                }
            }
        }

        private AccommodationDTOStefan CreateDTOForm(Accommodation acc, Location loc)
        {
            AccommodationDTOStefan dto = new AccommodationDTOStefan(acc.Name, loc.City, loc.Country, FindAccommodationType(acc),
                                                        acc.MaxGuests, acc.MinDaysStay);
            //dto.AccommodationDTOId = NextId();
            //dto.AccommodationId = acc.Id;
            //dto.LocationId = loc.Id;

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

        private AccommodationDTOStefan CreateDTORequest()
        {
            AccommodationDTOStefan acDTO = new AccommodationDTOStefan(searchedAccName, searchedAccCity, searchedAccCountry, searchedAccType,
                                                            searchedAccGuestsNumber, searchedAccDaysNumber);
            return acDTO;
        }

        private void SearchAndShow(AccommodationDTOStefan request)
        {
            List<AccommodationDTOStefan> searchResult = Search(request);
            ShowResults(searchResult);
        }

        private List<AccommodationDTOStefan> Search(AccommodationDTOStefan request)
        {
            List<AccommodationDTOStefan> SearchResult = new List<AccommodationDTOStefan>();
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

        private bool IsAppropriate(AccommodationDTOStefan item, AccommodationDTOStefan request)
        {
            bool checkName = item.AccommodationName.Contains(request.AccommodationName) || request.AccommodationName.Equals(string.Empty);
            bool checkCity = item.LocationCity.Contains(request.LocationCity) || request.LocationCity.Equals(string.Empty);
            bool checkCountry = item.LocationCountry.Contains(request.LocationCountry) || request.LocationCountry.Equals(string.Empty);
            bool checkType = item.AccommodationType == request.AccommodationType || request.AccommodationType == AccommType.NOTYPE;
            bool checkMaxGuests = request.AccommodationMaxGuests <= item.AccommodationMaxGuests;
            bool checkDaysStay = request.AccommodationMinDaysStay >= item.AccommodationMinDaysStay;

            return checkName && checkCity && checkCountry && checkType && checkMaxGuests && checkDaysStay;
        }

        private void ShowResults(List<AccommodationDTOStefan> results)
        {
            if (results.Count > 0)
            {
                SearchResults newWindow = new SearchResults(results);
                newWindow.Show();
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

        /*
        private int NextId()
        {
            if (AccommDTOsCollection.Count < 1)
            {
                return 1;
            }
            return AccommDTOsCollection.Max(l => l.AccommodationDTOId) + 1;
        }
        
        public List<AccommodationDTO> GetAll()
        {
            return AccommDTOsCollection.ToList();
        }
        */
    }
}