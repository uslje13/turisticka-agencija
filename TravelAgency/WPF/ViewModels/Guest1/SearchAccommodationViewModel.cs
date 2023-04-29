using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Repositories;
using SOSTeam.TravelAgency.WPF.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.WPF.Views.Owner;
using System.Windows.Controls;
using SOSTeam.TravelAgency.WPF.Views.Guest1;
using SOSTeam.TravelAgency.WPF.Views.TourGuide;
using System.Windows.Data;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest1
{
    public class SearchAccommodationViewModel
    {
        public User LoggedInUser { get; set; }
        public RelayCommand searchCommand { get; set; }
        public RelayCommand reserveCommand { get; set; }
        public Window ThisWindow { get; set; }
        public RelayCommand NavigationButtonCommand { get; set; }
        public Frame ThisFrame { get; set; }
        public TextBlock TextBlockUsername { get; set; }

        public SearchAccommodationViewModel(User user, Window window, Frame frame, TextBlock textBlock)
        {
            LoggedInUser = user;
            ThisWindow = window;
            ThisFrame = frame;
            TextBlockUsername = textBlock;

            FillTextBlock(LoggedInUser);

            searchCommand = new RelayCommand(ExecuteSearchAccommodation);
            reserveCommand = new RelayCommand(ExecuteReserveAccommodation);
            NavigationButtonCommand = new RelayCommand(Execute_NavigationButtonCommand);
        }

        private void FillTextBlock(User user)
        {
            Binding binding = new Binding();
            binding.Source = user.Username;
            TextBlockUsername.SetBinding(TextBlock.TextProperty, binding);
        }

        public void SetStartupPage()
        {
            var navigationService = ThisFrame.NavigationService;
            navigationService.Navigate(new AccommodationBidPage(LoggedInUser));
        }

        public void ExecuteSearchAccommodation(object sender)
        {
            //SearchWindow searchWindow = new SearchWindow(LoggedInUser);
            //searchWindow.ShowDialog();
            //ThisWindow.Close();
        }

        public void ExecuteReserveAccommodation(object sender)
        {   
            /*
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
            */
        }

        public void Execute_NavigationButtonCommand(object parameter)
        {
            string nextPage = parameter.ToString();
            var navigationService = ThisFrame.NavigationService;

            switch (nextPage)
            {
                case "Profille":
                    ThisWindow.Close();
                    break;
                case "Search":
                    navigationService.Navigate(new SearchPage(LoggedInUser));
                    break;
                case "Reservation":
                    //navigationService.Navigate(new AddAccommodationPage(LoggedInUser, this));
                    break;
                case "Whatever":
                    //navigationService.Navigate(new RequestPage(LoggedInUser, this));
                    break;
                case "LogOut":
                    //navigationService.Navigate(new OwnerReviewPage(LoggedInUser, this));
                    break;
                default:
                    break;
            }
            return;


        }
    }
}
