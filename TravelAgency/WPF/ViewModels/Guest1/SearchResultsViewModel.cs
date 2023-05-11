using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.Views;
using SOSTeam.TravelAgency.WPF.Views.Guest1;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest1
{
    public class SearchResultsViewModel
    {
        public User LoggedInUser { get; set; }
        public Frame ThisFrame { get; set; }
        public ObservableCollection<LocAccommodationViewModel> accommodationDTOs { get; set; }
        public LocAccommodationViewModel SelectedAccommodationDTO { get; set; }
        public RelayCommand reserveCommand { get; set; }
        public RelayCommand GoBackCommand { get; set; }

        public SearchResultsViewModel(List<LocAccommodationViewModel> Results, User user, Frame thisFrame)
        {
            accommodationDTOs = new ObservableCollection<LocAccommodationViewModel>(Results);
            LoggedInUser = user;
            reserveCommand = new RelayCommand(ExecuteReserveAccommodation);
            GoBackCommand = new RelayCommand(Execute_GoBack);
            ThisFrame = thisFrame;
        }

        public void ExecuteReserveAccommodation(object sender)
        {
            if (SelectedAccommodationDTO != null)
            {
                var navigationService = ThisFrame.NavigationService;
                navigationService.Navigate(new EnterReservationPage(SelectedAccommodationDTO, LoggedInUser, false, ThisFrame));
            }
            else
            {
                MessageBox.Show("Morate da odaberete smeštaj za rezervaciju.");
            }
        }

        public void Execute_GoBack(object sender)
        {
            var navigationService = ThisFrame.NavigationService;
            navigationService.GoBack();
        }
    }
}
