using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using static TravelAgency.Model.AccommodationDTO;

namespace TravelAgency.View
{
    /// <summary>
    /// Interaction logic for SearchAccommodation.xaml
    /// </summary>
    public partial class SearchAccommodation : Window
    {
        //public User LoggedInUser { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public List<Accommodation> accommodations { get; set; }
        public List<Location> locations { get; set; }
        public ObservableCollection<AccommodationDTO> AccommDTOsCollection { get; set; }
        public AccommodationRepository accommodationRepository { get; set; }
        public LocationRepository locationRepository { get; set; }
        public AccommodationDTO SelectedAccommodationDTO { get; set; }


        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public SearchAccommodation(/*User user*/)
        {
            InitializeComponent();
            DataContext = this;
            AccommDTOsCollection = new ObservableCollection<AccommodationDTO>();

            accommodationRepository = new AccommodationRepository();
            locationRepository = new LocationRepository();

            accommodations = accommodationRepository.GetAll();
            locations = locationRepository.GetAll();

            CreateAllDTOForms();
            //LoggedInUser = user;
        }

        private void SearchAccommodationClick(object sender, RoutedEventArgs e)
        {
            SearchWindow searchWindow = new SearchWindow();
            searchWindow.ShowDialog();
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
                        AccommodationDTO dto = CreateDTOForm(accommodation, location);
                        AccommDTOsCollection.Add(dto);
                    }
                }
            }
        }

        private AccommodationDTO CreateDTOForm(Accommodation acc, Location loc)
        {
            AccommodationDTO dto = new AccommodationDTO(acc.Id, acc.Name, loc.City, loc.Country, FindAccommodationType(acc),
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

        private void ReserveAccommodationClick(object sender, RoutedEventArgs e)
        {
            if(SelectedAccommodationDTO != null)
            {
                EnterReservation newWindow = new EnterReservation(SelectedAccommodationDTO);
                newWindow.ShowDialog();
            } else
            {
                MessageBox.Show("Morate da odaberete smeštaj za rezervaciju.");
            }
        }
    }
}
