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
        private readonly ReservationService _reservationService;
        private readonly AppointmentService _appointmentSevice;
        private readonly TourService _tourService;

        public static ObservableCollection<FinishedTourViewModel> FinishedTours { get;  set; }
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
        public NotificationsWindowViewModel(User loggedInUser, NotificationsWindow window) 
        {
            _window = window;
            LoggedInUser = loggedInUser;
            BackCommand = new RelayCommand(Execute_CancelCommand, CanExecuteMethod);
            HelpCommand = new RelayCommand(Execute_HelpCommand, CanExecuteMethod);
            _reservationService = new ReservationService();
            _appointmentSevice = new AppointmentService();
            _tourService = new TourService();
            FinishedTours = new ObservableCollection<FinishedTourViewModel>();
            FillFinishedToursList();
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

        private void FillFinishedToursList()
        {
            foreach(Reservation reservation in _reservationService.GetAll())
            {
                if(reservation.Presence && _appointmentSevice.GetById(reservation.AppointmentId).Finished && reservation.Reviewed == false)
                {
                    FinishedTours.Add(new FinishedTourViewModel(_window,reservation.Id, reservation.AppointmentId, LoggedInUser, _tourService.GetTourName(_appointmentSevice.GetById(reservation.AppointmentId).TourId)));
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
