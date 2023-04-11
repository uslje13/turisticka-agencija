using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest1
{
    public class SelectResDatesViewModel
    {
        public List<AccReservationViewModel> suggestCatalog { get; set; }
        public AccReservationViewModel selectedCatalogItem { get; set; }
        public User LoggedInUser { get; set; }
        public RelayCommand continueReserveCommand { get; set; }


        public SelectResDatesViewModel(List<AccReservationViewModel> list, User user)
        {
            suggestCatalog = list;
            LoggedInUser = user;
            continueReserveCommand = new RelayCommand(ExecuteCountinueReservation);
        }

        public void ExecuteCountinueReservation(object sender)
        {
            if (selectedCatalogItem == null)
            {
                MessageBox.Show("Odaberite smeštaj koji želite rezervisati.");
            }
            else
            {
                EnterGuestNumberWindow newWindow = new EnterGuestNumberWindow(selectedCatalogItem, LoggedInUser);
                newWindow.Show();
            }
        }
        /*
        private void ContinueReservation(object sender, RoutedEventArgs e)
        {
            if (selectedCatalogItem == null)
            {
                MessageBox.Show("Odaberite smeštaj koji želite rezervisati.");
            }
            else
            {
                EnterGuestNumberWindow newWindow = new EnterGuestNumberWindow(selectedCatalogItem, LoggedInUser);
                newWindow.Show();
            }
        }
        */
    }
}
