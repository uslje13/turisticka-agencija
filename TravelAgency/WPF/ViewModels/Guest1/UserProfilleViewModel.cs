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
        //public bool IsEnterFromChange { get; set; }

        public UserProfilleViewModel(User user, TextBlock uName) 
        {
            LoggedInUser = user;
            userName = uName;
            //IsEnterFromChange = false;
            FillTextBlock(LoggedInUser);
            goToSearchCommand = new RelayCommand(ExecuteGoToSearch);
            goToRequestsStatus = new RelayCommand(ExecuteGoToStatuses);
        }

        private void ExecuteGoToStatuses(object sender)
        {
            //IsEnterFromChange = true;
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
