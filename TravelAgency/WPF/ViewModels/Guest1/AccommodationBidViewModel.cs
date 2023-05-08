using LiveCharts;
using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using SOSTeam.TravelAgency.WPF.Views.Guest1;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest1
{
    public class AccommodationBidViewModel
    {
        public User LoggedInUser { get; set; }
        public Frame ThisFrame { get; set; }
        public ObservableCollection<LocAccommodationViewModel> _accommDTOsCollection { get; set; }
        public LocAccommodationViewModel SelectedAccommodationDTO { get; set; }
        public RelayCommand ReserveCommand { get; set; }
        
        public AccommodationBidViewModel(User user, Frame frame)
        {
            LoggedInUser = user;
            ThisFrame = frame;

            AccommodationService accommodationService = new AccommodationService();
            _accommDTOsCollection = accommodationService.CreateAllDTOForms();

            ReserveCommand = new RelayCommand(Execute_ReserveAccommodation);
        }
        public void Execute_ReserveAccommodation(object sender)
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
    }
}
