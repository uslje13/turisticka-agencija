﻿using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using System;
using System.Collections.ObjectModel;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Owner
{
    public class RenovationAddPageViewModel : ViewModel
    {
        public User LoggedInUser { get; private set; }

        public ObservableCollection<Accommodation> Accommodations { get; private set; }

        public Accommodation SelectedAccommodation { get; set; }

        public RelayCommand Cancel { get; private set; }
        public RelayCommand SearchDates { get; private set; }
        public RelayCommand ResetCalendar { get; private set; }

        public string StartDateString
        {
            get { return _startDateString; }
            set
            {
                if (_startDateString != value)
                {
                    _startDateString = value;
                    OnPropertyChanged(nameof(StartDateString));
                }
            }
        }

        public string EndDateString
        {
            get { return _endDateString; }
            set
            {
                if (_endDateString != value)
                {
                    _endDateString = value;
                    OnPropertyChanged(nameof(EndDateString));
                }
            }
        }
        
        public int RenovationDuration
        {
            get { return _renovationDuration; }
            set
            {
                if (_renovationDuration != value)
                {
                    _renovationDuration = value;
                    OnPropertyChanged(nameof(RenovationDuration));
                }
            }
        }

        public DateTime StartDate
        {
            get { return _startDate; }
            set
            {
                if (_startDate != value)
                {
                    _startDate = value;
                    OnPropertyChanged(nameof(StartDate));
                }
            }
        }

        public DateTime EndDate
        {
            get { return _endDate; }
            set
            {
                if (_endDate != value)
                {
                    _endDate = value;
                    OnPropertyChanged(nameof(EndDate));
                }
            }
        }

        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                if (_selectedDate != value)
                {
                    _selectedDate = value;
                    SelectedDateChange(_selectedDate);
                    OnPropertyChanged(nameof(SelectedDate));
                }
            }
        }
        public bool IsCalendarEnabled
        {
            get { return _isCalendarEnabled; }
            set
            {
                if (_isCalendarEnabled != value)
                {
                    _isCalendarEnabled = value;
                    OnPropertyChanged(nameof(IsCalendarEnabled));
                }
            }
        }




        private DateTime _startDate;
        private DateTime _endDate;
        private DateTime _selectedDate;
        private string _startDateString;
        private string _endDateString;
        private bool _isCalendarEnabled;
        private int _datePickCounter;
        private int _renovationDuration;

        private MainWindowViewModel _mainwindowVM;

        private AccommodationService _accommodationService;
        private AccommodationRenovationService _accommodationRenovationService;






        public RenovationAddPageViewModel(User user, MainWindowViewModel mainWindowVM)
        {
            LoggedInUser = user;
            _mainwindowVM = mainWindowVM;
            _accommodationService = new();
            _accommodationRenovationService = new();
            Accommodations = new ObservableCollection<Accommodation>(_accommodationService.GetAllByUserId(user.Id));
            RefreshCalendar();

            Cancel = new RelayCommand(Execute_Cancel, CanExecuteCancel);
            SearchDates = new RelayCommand(Execute_SearchDates, CanExecuteSearchDates);
            ResetCalendar = new RelayCommand(Execute_SetupCalendar, CanExecuteSetupCalendar);

        }

        private bool CanExecuteSearchDates(object obj)
        {
            return _datePickCounter >= 2 && SelectedAccommodation != null;
        }

        public void RefreshCalendar() 
        {
            Execute_SetupCalendar(null);
        }

        private void Execute_Cancel(object obj)
        {
            _mainwindowVM.Execute_NavigationButtonCommand("Renovation");
        }

        private bool CanExecuteCancel(object obj)
        {
            return true;
        }

        private void Execute_SetupCalendar(object obj)
        {
            StartDate = DateTime.Now;
            SelectedDate = DateTime.Now;
            EndDate = DateTime.Now.AddYears(5);
            _datePickCounter = 0;
            IsCalendarEnabled = true;
            StartDateString = "";
            EndDateString = "";
        }

        private void SelectedDateChange(DateTime selectedDate)
        {
            _datePickCounter++;
            if (_datePickCounter == 2)
            {
                EndDate = selectedDate;
                EndDateString = EndDate.ToString("dd/MM/yyyy");
            }
            if (_datePickCounter == 1)
            {
                StartDate = selectedDate;
                StartDateString = StartDate.ToString("dd/MM/yyyy");
            }

        }


        private void Execute_SearchDates(object obj)
        {

        }

        private bool CanExecuteSetupCalendar(object obj)
        {
            return true;
        }

    }

}
