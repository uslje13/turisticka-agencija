using System;
using System.Collections.Generic;
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

namespace TravelAgency.View
{
    /// <summary>
    /// Interaction logic for SearchWindow.xaml
    /// </summary>
    public partial class SearchWindow : Window
    {
        private string[] AvailableAccommodationTypes = { "APARTMAN", "KUĆA", "KOLIBA" };
        public string searchedAccName { get; set; }
        public string searchedAccCity { get; set; }
        public string searchedAccCountry { get; set; }
        public int searchedAccGuestsNumber { get; set; }
        public int searchedAccDaysNumber { get; set; }
        public List<AccommodationDTO> DTOs { get; set; }

        public SearchWindow()
        {
            DataContext = this;
            //CBTypes.ItemsSource = AvailableAccommodationTypes;
            AccommodationDTO accDTO = new AccommodationDTO();
            DTOs = accDTO.GetAll();
            InitializeComponent();
        }

        private void SearchAccommodationClick(object sender, RoutedEventArgs e)
        {
            LoadEnteredRequests();
            AccommodationDTO dtoRequest = CreateDTORequest();
            Search(dtoRequest);
        }

        private void LoadEnteredRequests()
        {
            searchedAccName = name.Text;
            searchedAccCity = city.Text;
            searchedAccCountry = country.Text;
            //string accommodationTypes;
            string loadedGuestsNumber = guestsNumber.Text;
            searchedAccGuestsNumber = int.Parse(loadedGuestsNumber);
            string loadedDaysNumber = daysNumber.Text;
            searchedAccDaysNumber = int.Parse(loadedDaysNumber);
        }

        private AccommodationDTO CreateDTORequest()
        {
            AccommodationDTO acDTO = new AccommodationDTO();
            acDTO.AccommodationName = searchedAccName;
            acDTO.LocationCity = searchedAccCity;
            acDTO.LocationCountry = searchedAccCountry;
            //acDTO.AccommodationType = searchedType;
            acDTO.AccommodationMaxGuests = searchedAccGuestsNumber;
            acDTO.AccommodationMinDaysStay = searchedAccDaysNumber;
            return acDTO;
        }

        private void Search(AccommodationDTO request)
        {
            List<AccommodationDTO> SearchResult = new List<AccommodationDTO>();
            foreach (var item in DTOs)
            {
                bool checkName = item.AccommodationName.Contains(searchedAccName) || searchedAccName.Equals(string.Empty);
                bool checkCity = item.LocationCity.Contains(searchedAccCity) || searchedAccCity.Equals(string.Empty);
                bool checkCountry = item.LocationCountry.Contains(searchedAccCountry) || searchedAccCountry.Equals(string.Empty);
                bool checkMaxGuests = searchedAccGuestsNumber <= item.AccommodationMaxGuests;
                bool checkDaysStay = searchedAccDaysNumber >= item.AccommodationMinDaysStay;
                if (checkName && checkCity && checkCountry && checkMaxGuests && checkDaysStay)
                {
                    SearchResult.Add(item);
                }
            }

            SearchResults newWindow = new SearchResults(SearchResult);
            newWindow.Show();
        }

        private void CancelAccommodationClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}