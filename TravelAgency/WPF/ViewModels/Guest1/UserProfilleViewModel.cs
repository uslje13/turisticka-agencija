using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;
using System.Windows.Controls;
using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.WPF.Views;
using SOSTeam.TravelAgency.WPF.Views.Guest1;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest1
{
    public class UserProfilleViewModel
    {
        public User LoggedInUser { get; set; }
        public TextBlock userName { get; set; }
        public RelayCommand goToSearchCommand { get; set; }
        public RelayCommand goToRequestsStatus { get; set; }
        public RelayCommand goToInboxCommand { get; set; }


        public UserProfilleViewModel(User user, TextBlock uName) 
        {
            LoggedInUser = user;
            userName = uName;

            FillTextBlock(LoggedInUser);
            CollectFinishedReservations();
            //ShowNotifications();
            goToSearchCommand = new RelayCommand(ExecuteGoToSearch);
            goToRequestsStatus = new RelayCommand(ExecuteGoToStatuses);
            goToInboxCommand = new RelayCommand(ExecuteInboxShowing);
        }

        private void CollectFinishedReservations()
        {
            AccommodationReservationService service = new AccommodationReservationService();
            List<AccommodationReservation> localList = service.LoadFinishedReservations();

            if(localList.Count > 0)
            {
                foreach (var item in localList)
                {
                    service.DeleteFromFinsihedCSV(item);
                }
            }

            foreach (var item in service.GetAll())
            {
                if (DateTime.Today.DayOfYear > item.LastDay.DayOfYear)
                {
                    service.SaveFinishedReservation(item);
                }
            }
        }

        private void ExecuteInboxShowing(object sender)
        {
            //MarkAccommodationWindow markAccommodationWindow = new MarkAccommodationWindow(LoggedInUser);
            //markAccommodationWindow.Show();
            GuestInboxWindow newWindow = new GuestInboxWindow(LoggedInUser);
            newWindow.ShowDialog();
        }
        /*
        private void ShowNotifications()
        {
            NotificationFromOwnerService service = new NotificationFromOwnerService();
            UserService userService = new UserService();
            List<NotificationFromOwner> localList = service.GetAll();

            if(localList.Count > 0)
            {
                foreach (var item in localList)
                {
                    User user = userService.GetById(item.OwnerId);
                    MessageBox.Show("Vlasnik " + user.Username + " je odgovorio na Vaš zahtjev za pomjeranje rezervacije u smještaju " +
                                    item.AccommodationName + " sa: " + item.Answer + ".");
                    service.Delete(item.Id);
                }
            }
        }
        */
        private void ExecuteGoToStatuses(object sender)
        {
            RequestsStatusWindow newWindow = new RequestsStatusWindow(LoggedInUser);
            newWindow.Show();
        }

        private void ExecuteGoToSearch(object sender)
        {
            SearchAccommodationWindow newWindow = new SearchAccommodationWindow(LoggedInUser);
            newWindow.Show();
        }

        private void FillTextBlock(User user)
        {
            Binding binding = new Binding();
            binding.Source = user.Username;
            userName.SetBinding(TextBlock.TextProperty, binding);
        }
    }
}
