﻿using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.Views.Guest2;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest2
{
    public class NotificationsWindowViewModel : ViewModel
    {
        public static User LoggedInUser { get; set; }
        private NotificationsWindow _window;
        private RelayCommand _backCommand;

        public RelayCommand BackCommand
        {
            get { return _backCommand; }
            set
            {
                _backCommand = value;
            }
        }

        private RelayCommand _helpCommand;

        public RelayCommand HelpCommand
        {
            get { return _helpCommand; }
            set
            {
                _helpCommand = value;
            }
        }

        private RelayCommand _navigationCommand;
        public RelayCommand NavigationCommand
        {
            get { return _navigationCommand; }
            set
            {
                _navigationCommand = value;
            }
        }
        public NotificationsWindowViewModel(User loggedInUser, NotificationsWindow window) 
        {
            _window = window;
            LoggedInUser = loggedInUser;
            BackCommand = new RelayCommand(Execute_CancelCommand, CanExecuteMethod);
            HelpCommand = new RelayCommand(Execute_HelpCommand, CanExecuteMethod);
            NavigationCommand = new RelayCommand(Execute_NavigationCommand, CanExecuteMethod);
        }

        public void SetStartupPage()
        {
            Execute_NavigationCommand("FinishedTour");
        }
        private void Execute_NavigationCommand(object obj)
        {
            string nextPage = obj.ToString();
            var navigationService = _window.MainFrame.NavigationService;

            switch (nextPage)
            {
                case "FinishedTour":
                    navigationService.Navigate(new FinishedToursNotificationPage(LoggedInUser));
                    break;
                case "NewTour":
                    navigationService.Navigate(new NewToursNotificationPage(LoggedInUser));
                    break;
            }
        }    

        private void Execute_HelpCommand(object obj)
        {
            _window.Close();

            var currentApp = System.Windows.Application.Current;

            foreach (Window window in currentApp.Windows)
            {
                if (window is ToursOverviewWindow)
                {
                    PreviousWindowOrPageName.SetPreviousWindowOrPageName(this.GetType().Name);
                    var navigationService = ((ToursOverviewWindow)window).HelpFrame.NavigationService;
                    navigationService.Navigate(new HelpPage(LoggedInUser));
                    break;
                }
            }
        }

        private bool CanExecuteMethod(object parameter)
        {
            return true;
        }

        private void Execute_CancelCommand(object sender)
        {
            _window.Close();
        }
    }
}
