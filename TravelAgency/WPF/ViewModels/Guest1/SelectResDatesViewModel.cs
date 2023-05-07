using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.Views;
using SOSTeam.TravelAgency.WPF.Views.Guest1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest1
{
    public class SelectResDatesViewModel
    {
        public List<AccReservationViewModel> suggestCatalog { get; set; }
        public AccReservationViewModel selectedCatalogItem { get; set; }
        public User LoggedInUser { get; set; }
        public RelayCommand continueReserveCommand { get; set; }
        public RelayCommand GoBackCommand { get; set; }
        public bool IsEnterOfChange { get; set; }
        public ChangedReservationRequest ChangedReservationRequest { get; set; }
        public Frame ThisFrame { get; set; }
        public TextBlock AccNameTextBlock { get; set; }

        public SelectResDatesViewModel(List<AccReservationViewModel> list, User user, bool isEnterOfChange, ChangedReservationRequest request, Frame frame, TextBlock tb)
        {
            suggestCatalog = list;
            LoggedInUser = user;
            IsEnterOfChange = isEnterOfChange;
            ChangedReservationRequest = request;
            ThisFrame = frame;
            AccNameTextBlock = tb;

            FillTextBlock(list);

            continueReserveCommand = new RelayCommand(ExecuteCountinueReservation);
            GoBackCommand = new RelayCommand(Execute_GoBack);
        }

        private void Execute_GoBack(object sender)
        {
            var navigationService = ThisFrame.NavigationService;
            navigationService.GoBack();
        }

        private void FillTextBlock(List<AccReservationViewModel> list)
        {
            foreach (var item in list)
            {
                Binding binding = new Binding();
                binding.Source = "Pronađeni termini u smještaju " + item.AccommodationName;
                AccNameTextBlock.SetBinding(TextBlock.TextProperty, binding);
                break;
            }
        }

        public void ExecuteCountinueReservation(object sender)
        {
            if (selectedCatalogItem == null)
            {
                MessageBox.Show("Odaberite smeštaj koji želite rezervisati.");
            }
            else
            {
                var navigationService = ThisFrame.NavigationService;
                navigationService.Navigate(new EnterGuestNumberPage(selectedCatalogItem, LoggedInUser, IsEnterOfChange, ChangedReservationRequest, ThisFrame));
            }
        }
    }
}
