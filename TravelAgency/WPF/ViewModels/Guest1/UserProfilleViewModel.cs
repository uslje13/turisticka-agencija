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
using System.Windows.Media;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest1
{
    public class UserProfilleViewModel
    {
        public User LoggedInUser { get; set; }
        public TextBlock userName { get; set; }
        public RelayCommand goToSearchCommand { get; set; }
        public RelayCommand goToRequestsStatus { get; set; }
        public RelayCommand goToInboxCommand { get; set; }
        public Button InboxButton { get; set; }


        public UserProfilleViewModel(User user, TextBlock uName, Button button, int notifications) 
        {
            LoggedInUser = user;
            userName = uName;
            InboxButton = button;

            ControlInboxButton(notifications);
            FillTextBlock(LoggedInUser);
            CollectFinishedReservations();
            
            goToSearchCommand = new RelayCommand(ExecuteGoToSearch);
            goToRequestsStatus = new RelayCommand(ExecuteGoToStatuses);
            goToInboxCommand = new RelayCommand(ExecuteInboxShowing);
        }

        private void ControlInboxButton(int notifications)
        {
            InboxButton.Content = "Obavještenja - " + notifications.ToString();
            if (notifications > 0)
            {
                InboxButton.Background = new SolidColorBrush(Colors.OrangeRed);
            }
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
            GuestInboxWindow newWindow = new GuestInboxWindow(LoggedInUser);
            newWindow.ShowDialog();
        }

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
