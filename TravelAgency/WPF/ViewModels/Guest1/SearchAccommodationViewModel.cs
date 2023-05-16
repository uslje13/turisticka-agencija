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
        public Window ThisWindow { get; set; }
        public Window InboxWindow { get; set; }
        public Window UserProfilleWindow { get; set; }
        public Frame ThisFrame { get; set; }
        public int Notifications { get; set; }
        public string UserNameTextBlock { get; set; }
        public RelayCommand NavigationButtonCommand { get; set; }

        public SearchAccommodationViewModel(User user, Window window, Frame frame, Window inbox, Window profille, int notifications)
        {
            LoggedInUser = user;
            ThisWindow = window;
            ThisFrame = frame;
            UserNameTextBlock = user.Username;
            InboxWindow = inbox;
            UserProfilleWindow = profille;
            Notifications = notifications;

            NavigationButtonCommand = new RelayCommand(Execute_NavigationButtonCommand);
        }

        public void SetStartupPage()
        {
            Execute_NavigationButtonCommand("Bid");
        }

        public void Execute_NavigationButtonCommand(object parameter)
        {
            string nextPage = parameter.ToString();
            var navigationService = ThisFrame.NavigationService;

            switch (nextPage)
            {
                case "Profille":
                    NotificationFromOwnerService service = new NotificationFromOwnerService();
                    UserProfilleWindow newWindow = new UserProfilleWindow(LoggedInUser, service.TestInboxCharge(LoggedInUser.Id));
                    ThisWindow.Close();
                    newWindow.ShowDialog();
                    break;
                case "Search":
                    navigationService.Navigate(new SearchPage(LoggedInUser, ThisFrame));
                    break;
                case "Bid":
                    navigationService.Navigate(new AccommodationBidPage(LoggedInUser, ThisFrame));
                    break;
                case "Whatever":
                    
                    break;
                case "LogOut":
                    SignInForm form = new SignInForm();
                    ThisWindow.Close();
                    UserProfilleWindow.Close();
                    InboxWindow.Close();
                    form.ShowDialog();
                    break;
                default:
                    break;
            }
            return;
        }
    }
}
