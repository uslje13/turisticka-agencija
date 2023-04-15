using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Owner
{
    internal class RequestPageViewModel
    {
        private AccommodationReservationService _accommodationReservationService;
        private UserService _userService;
        private MainWindowViewModel _mainwindowVM;

        public User LoggedInUser { get; private set; }
        public RelayCommand Accept { get; private set; }
        public RelayCommand Deny { get; private set; }
        public RelayCommand ShowDenialPopup { get; private set; }
        public bool IsDropdownOpen { get; set; }
        public ObservableCollection<RequestViewModel> Requests { get; set; }
        public RequestViewModel SelectedRequest { get; set; }

        public RequestPageViewModel(User user, MainWindowViewModel mainWindowVM)
        {
            _accommodationReservationService = new();
            _userService = new();
            _mainwindowVM = mainWindowVM;
            LoggedInUser = user;
            Requests = new();
            IsDropdownOpen = true;
            FillObservableCollection();

            

            Accept = new RelayCommand(Execute_Accept, CanExecuteDenyorAccept);
            Deny = new RelayCommand(Execute_Deny, CanExecuteDenyorAccept);
            ShowDenialPopup = new RelayCommand(Execute_ShowDenialPopup, CanExecuteDenyorAccept);
        }

        private void FillObservableCollection()
        {
            AccommodationReservationService.ReservationsInformations reservationsInformations = _accommodationReservationService.SendRequestToOwner(LoggedInUser.Id);
            foreach (var request in reservationsInformations.requests) 
            {
                foreach (var newDates in reservationsInformations.newDates) 
                {
                    if (request.reservationId == newDates.OldReservationId) 
                    {
                        string username = _userService.GetById(request.UserId).Username;
                        Requests.Add(new RequestViewModel(request, newDates, username));
                    }
                }
            }
        }

        private void Execute_ShowDenialPopup(object obj)
        {
            IsDropdownOpen = !IsDropdownOpen;
        }
        private void Execute_Deny(object obj)
        {
            throw new NotImplementedException();
        }
        private void Execute_Accept(object obj)
        {
            _accommodationReservationService.acceptReservationChanges( SelectedRequest.NewDate,SelectedRequest.Request,LoggedInUser.Id);
        }

        private bool CanExecuteDenyorAccept(object obj)
        {
            return SelectedRequest != null;
        }
    }

    internal class RequestViewModel
    {
        public ChangedReservationRequest Request { get; private set; }
        public WantedNewDate NewDate { get; private set; }
        public string Username { get; private set; }
        public string OldReservationDates { get; private set; }
        public string NewReservationDates { get; private set; }
        public RequestViewModel( ChangedReservationRequest changedReservationRequest,WantedNewDate wantedNewDate,string username) 
        {
            Request = changedReservationRequest;
            NewDate = wantedNewDate;
            OldReservationDates = changedReservationRequest.OldFirstDay.ToShortDateString() + " - " + changedReservationRequest.OldFirstDay.ToShortDateString();
            NewReservationDates = wantedNewDate.wantedDate.ReservationFirstDay.ToShortDateString() + " - " + wantedNewDate.wantedDate.ReservationLastDay.ToShortDateString();
            Username = username;
        }
    }
}
