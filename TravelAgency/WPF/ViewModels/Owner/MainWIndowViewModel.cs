using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.WPF.Views;
using System.Windows;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.WPF.Views.Owner;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using SOSTeam.TravelAgency.Repositories;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Owner
{
    public class MainWindowViewModel : ViewModel
    {
        private UserService _userService;
        public string Username { get; private set; }
        public User LoggedInUser { get; private set; }
        private MainWindow _mainWindow;
        private NotificationService _notificationService;

        public static ObservableCollection<Notification> Notifications { get; set; }
        public Notification SelectedNotification { get; set; }

        public RelayCommand NavigationButtonCommand { get; private set; }
        public RelayCommand ToggleShowNotifications { get; private set; }
        public RelayCommand NotificationDoubleClick { get; private set; }



        private bool _isDropdownOpen;
        public bool IsDropdownOpen
        {
            get => _isDropdownOpen;
            set
            {
                if (_isDropdownOpen != value)
                {
                    _isDropdownOpen = value;
                    IsDropdownClosed = !IsDropdownOpen;
                    OnPropertyChanged(nameof(IsDropdownOpen));
                    
                }
            }
        }
        public bool IsDropdownClosed { get; set; }
        public MainWindowViewModel(User user,MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
            _userService = new();
            Username = user.Username;
            LoggedInUser = user;
            IsDropdownOpen = false;
            IsDropdownClosed = !IsDropdownOpen;
            NavigationButtonCommand = new RelayCommand(Execute_NavigationButtonCommand, CanExecuteMethod);
            ToggleShowNotifications = new RelayCommand(Execute_ToggleShowNotifications, CanExecuteMethod);
            NotificationDoubleClick = new RelayCommand(Execute_NotificationDoubleClick, CanExecuteMethod);


            Notifications = new ObservableCollection<Notification>();

            _notificationService = new();
            _notificationService.Refresh(LoggedInUser.Id);
            GetUserNotifications();
        }

        private void GetUserNotifications()
        {
            foreach (Notification notification in _notificationService.GetAll())
            {
                if (notification.UserId == LoggedInUser.Id)
                {
                    Notifications.Add(notification);
                }
            }
        }

        public void SetStartupPage() 
        {
            Execute_NavigationButtonCommand("Home");
        }

        internal void SetPage(object root) 
        {
            _mainWindow.MainFrame.NavigationService.Navigate(root);
        }

        public void Execute_ToggleShowNotifications(object parameter) 
        {
            IsDropdownOpen = !IsDropdownOpen;
        }

        public void Execute_NotificationDoubleClick(object parameter)
        {
            if (SelectedNotification == null) return;

            SelectedNotification.Read = true;
            _notificationService.Update(SelectedNotification);

            if (SelectedNotification.Type == Notification.NotificationType.GUESTREVIEW) 
            {
                SetPage(new GuestReviewPage(LoggedInUser,this,_userService.GetById(SelectedNotification.GuestId)));
            }
        }

        public void Execute_NavigationButtonCommand(object parameter)
        {
            string nextPage = parameter.ToString();
            var navigationService = _mainWindow.MainFrame.NavigationService;

            switch (nextPage) 
            {
                case "Home":
                    navigationService.Navigate(new HomePage(LoggedInUser));
                    break;
                case "Accommodation":
                    navigationService.Navigate(new AccommodationsPage(LoggedInUser,this));
                    break;
                case "AccommodationAdd":
                    navigationService.Navigate(new AddAccommodationPage(LoggedInUser,this));
                    break;
                
                /*
                case "Review":
                    navigationService.Navigate(new HomePage());
                    break;
                case "Renovation":
                    navigationService.Navigate(new HomePage());
                    break;
                case "Request":
                    navigationService.Navigate(new HomePage());
                    break;
                case "Suggestion":
                    navigationService.Navigate(new HomePage());
                    break;
                case "Forum":
                    navigationService.Navigate(new HomePage());
                    break;
                */

                default:
                    break;
            }
            return;

            
        }

        private bool CanExecuteMethod(object parameter)
        {
            return true;
        }

        private void FillObservableCollection()
        {
            foreach (Notification notification in _notificationService.GetAll())
            {
                if (notification.UserId == LoggedInUser.Id)
                {
                    Notifications.Add(notification);
                }
            }
        }

    }
}
