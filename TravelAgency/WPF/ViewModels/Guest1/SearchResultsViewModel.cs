using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest1
{
    public class SearchResultsViewModel
    {
        public ObservableCollection<LocAccommodationViewModel> accommodationDTOs { get; set; }
        public LocAccommodationViewModel SelectedAccommodationDTO { get; set; }
        public User LoggedInUser { get; set; }
        public RelayCommand reserveCommand { get; set; }
        public Window ThisWindow { get; set; }

        public SearchResultsViewModel(List<LocAccommodationViewModel> Results, User user, Window window)
        {
            accommodationDTOs = new ObservableCollection<LocAccommodationViewModel>(Results);
            LoggedInUser = user;
            ThisWindow = window;
            reserveCommand = new RelayCommand(ExecuteReserveAccommodation);
        }

        public void ExecuteReserveAccommodation(object sender)
        {
            if (SelectedAccommodationDTO != null)
            {
                EnterReservationWindow newWindow = new EnterReservationWindow(SelectedAccommodationDTO, LoggedInUser, false);
                newWindow.ShowDialog();
                ThisWindow.Close();
            }
            else
            {
                MessageBox.Show("Morate da odaberete smeštaj za rezervaciju.");
            }
        }
    }
}
