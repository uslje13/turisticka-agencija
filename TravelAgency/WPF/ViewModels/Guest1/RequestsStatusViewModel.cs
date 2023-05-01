using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.Views.Guest1;
using SOSTeam.TravelAgency.WPF.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows;
using System.Reflection.Metadata;
using SOSTeam.TravelAgency.Application.Services;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest1
{
    public class RequestsStatusViewModel
    {
        public User LoggedInUser { get; set; }
        public RelayCommand searchCommand { get; set; }
        public RelayCommand reserveCommand { get; set; }
        public Window ThisWindow { get; set; }
        public RelayCommand NavigationButtonCommand { get; set; }
        public Frame ThisFrame { get; set; }
        public TextBlock TextBlockUsername { get; set; }
        public Window UserProfilleWindow { get; set; }
        public int Notifications { get; set; }

        public RequestsStatusViewModel(User user, Window window, Frame frame, TextBlock textBlock, Window profille, int notifications)
        {
            LoggedInUser = user;
            ThisWindow = window;
            ThisFrame = frame;
            TextBlockUsername = textBlock;
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
            var navigationService = ThisFrame.NavigationService;
            navigationService.Navigate(new AllStatusesPage(LoggedInUser, ThisFrame));
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
                    
                    break;
                case "Bid":
                    
                    break;
                case "Whatever":

                    break;
                case "LogOut":
                    SignInForm form = new SignInForm();
                    ThisWindow.Close();
                    UserProfilleWindow.Close();
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
