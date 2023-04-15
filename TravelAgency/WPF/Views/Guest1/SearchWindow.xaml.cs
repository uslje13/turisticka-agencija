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
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Repositories;
using SOSTeam.TravelAgency.WPF.ViewModels.Guest1;
using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;

namespace SOSTeam.TravelAgency.WPF.Views
{
    /// <summary>
    /// Interaction logic for SearchWindow.xaml
    /// </summary>
    public partial class SearchWindow : Window
    {
        /*
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
        */
        public User LoggedInUser { get; set; }
        
        public RelayCommand searchCommand { get; set; }
        public RelayCommand cancelCommand { get; set; }


        public SearchWindow(User user)
        {
            InitializeComponent();
            DataContext = this;
            
            searchCommand = new RelayCommand(ExecuteAccommodationSearch);
            cancelCommand = new RelayCommand(ExecuteCancelAccommodationSearch);
            
            LoggedInUser = user;
        }

        private void ExecuteAccommodationSearch(object sender)
        {
            AccommodationService accommodationService = new AccommodationService(LoggedInUser, name.Text, city.Text, country.Text, GetSelectedItem(CBTypes), int.Parse(guestsNumber.Text), int.Parse(daysNumber.Text));
            accommodationService.ExecuteAccommodationSearch();
        }

        private void ExecuteCancelAccommodationSearch(object sender)
        {
            Close();
        }

        private LocAccommodationViewModel.AccommType GetSelectedItem(ComboBox cb)
        {
            if (cb.SelectedItem == CBItemApart) return LocAccommodationViewModel.AccommType.APARTMENT;
            else if (cb.SelectedItem == CBItemHouse) return LocAccommodationViewModel.AccommType.HOUSE;
            else if (cb.SelectedItem == CBItemHut) return LocAccommodationViewModel.AccommType.HUT;
            else return LocAccommodationViewModel.AccommType.NOTYPE;
        }
    }
}