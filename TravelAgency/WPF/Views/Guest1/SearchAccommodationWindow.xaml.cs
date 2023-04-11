﻿using System;
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
using TravelAgency.DTO;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Repositories;
using SOSTeam.TravelAgency.WPF.ViewModels.Guest1;



namespace SOSTeam.TravelAgency.WPF.Views
{
    /// <summary>
    /// Interaction logic for SearchAccommodation.xaml
    /// </summary>
    public partial class SearchAccommodationWindow : Window
    {
        /*
        public User LoggedInUser { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public List<Accommodation> accommodations { get; set; }
        public List<Location> locations { get; set; }
        public ObservableCollection<LocAccommodationViewModel> AccommDTOsCollection { get; set; }

        public AccommodationRepository accommodationRepository { get; set; }
        public LocationRepository locationRepository { get; set; }
        public LocAccommodationViewModel SelectedAccommodationDTO { get; set; }
        public AccommodationReservationRepository accommodationReservationRepository { get; set; }
        public List<AccommodationReservation> accommodationReservations { get; set; }
        */

        /*
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        */

        public SearchAccommodationWindow(User user)
        {
            InitializeComponent();
            //DataContext = this;
            SearchAccommodationViewModel viewModel = new SearchAccommodationViewModel(user);
            DataContext = viewModel;
            /*
            AccommDTOsCollection = new ObservableCollection<LocAccommodationViewModel>();


            accommodationRepository = new AccommodationRepository();
            locationRepository = new LocationRepository();
            accommodationReservationRepository = new AccommodationReservationRepository();

            accommodations = accommodationRepository.GetAll();
            locations = locationRepository.GetAll();
            accommodationReservations = accommodationReservationRepository.GetAll();
            LoggedInUser = user;

            CreateAllDTOForms();
            */
        }

        /*
        private void SearchAccommodationClick(object sender, RoutedEventArgs e)
        {
            SearchWindow searchWindow = new SearchWindow(LoggedInUser);
            searchWindow.ShowDialog();
            Close();
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
            foreach(var item in accommodationReservations)
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

        private void ReserveAccommodationClick(object sender, RoutedEventArgs e)
        {
            if(SelectedAccommodationDTO != null)
            {
                EnterReservationWindow newWindow = new EnterReservationWindow(SelectedAccommodationDTO, LoggedInUser);
                newWindow.ShowDialog();
                Close();
            } else
            {
                MessageBox.Show("Morate da odaberete smeštaj za rezervaciju.");
            }
        }
        */
    }
}