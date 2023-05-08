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
        public Window InboxWindow { get; set; }
        public Window UserProfilleWindow { get; set; }
        public int Notifications { get; set; }

        public SearchAccommodationViewModel(User user, Window window, Frame frame, TextBlock textBlock, Window inbox, Window profille, int notifications)
        {
            LoggedInUser = user;
            ThisWindow = window;
            ThisFrame = frame;
            TextBlockUsername = textBlock;
            InboxWindow = inbox;
            UserProfilleWindow = profille;
            Notifications = notifications;

            FillTextBlock(LoggedInUser);

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
            Execute_NavigationButtonCommand("Bid");
        }

        public void Execute_NavigationButtonCommand(object parameter)
        {
            string nextPage = parameter.ToString();
            var navigationService = ThisFrame.NavigationService;

            switch (nextPage)
            {
                case "Profille":
                    UserProfilleWindow newWindow = new UserProfilleWindow(LoggedInUser, TestInboxCharge(LoggedInUser.Id));
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

        private int TestInboxCharge(int loggedInUserId)
        {
            int marksNotifications = TestMarkNotifications(loggedInUserId);
            int ownerNotifications = TestOwnerRequestNotifications(loggedInUserId);
            return marksNotifications + ownerNotifications;
        }

        private int TestMarkNotifications(int loggedInUserId)
        {
            AccommodationReservationService resService = new AccommodationReservationService();
            List<AccommodationReservation> allRes = resService.GetAll();
            int counter = 0;
            foreach (var res in allRes)
            {
                int diff = DateTime.Today.DayOfYear - res.LastDay.DayOfYear;
                bool fullCharge = res.UserId == loggedInUserId && !res.ReadMarkNotification && diff <= 5 && diff > 0;
                if (fullCharge)
                {
                    counter++;
                }
            }
            return counter;
        }

        private int TestOwnerRequestNotifications(int loggedInUserId)
        {
            NotificationFromOwnerService notifService = new NotificationFromOwnerService();
            List<NotificationFromOwner> notifications = notifService.GetAll();
            int counter = 0;
            foreach (var item in notifications)
            {
                if (item.GuestId == loggedInUserId)
                {
                    counter++;
                }
            }
            return counter;
        }
    }
}
