﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.Views.TourGuide;

namespace SOSTeam.TravelAgency.WPF.ViewModels.TourGuide
{
    public class LiveTourViewModel : ViewModel
    {
        #region Services

        private readonly CheckpointActivityService _checkpointActivityService;
        private readonly CheckpointService _checkpointService;
        private readonly AppointmentService _appointmentService;
        private readonly TourService _tourService;
        private readonly ReservationService _reservationService;
        private readonly GuestAttendanceService _guestAttendanceService;

        #endregion

        #region Commands
        public RelayCommand ViewGuestAttendanceCommand { get; set; }
        public RelayCommand ActivateCheckpointCommand { get; set; }
        public RelayCommand FinishCheckpointCommand { get; set; }
        public RelayCommand FinishAppointmentCommand { get; set; }
        public RelayCommand SelectionChangedCardCommand { get; set; }
        #endregion

        #region Properties

        private ObservableCollection<CheckpointCardViewModel> _checkpointCards;

        public ObservableCollection<CheckpointCardViewModel> CheckpointCards
        {
            get => _checkpointCards;
            set
            {
                if (_checkpointCards != value)
                {
                    _checkpointCards = value;
                    OnPropertyChanged("CheckpointCards");
                }
            }
        }

        private CheckpointCardViewModel? _selectedCheckpointCard;

        public CheckpointCardViewModel? SelectedCheckpointCard
        {
            get => _selectedCheckpointCard;
            set
            {
                if (_selectedCheckpointCard != value)
                {
                    _selectedCheckpointCard = value;
                    OnPropertyChanged("SelectedCheckpointCard");
                }
            }
        }


        private bool _canActivateCheckpoint;

        public bool CanActivateCheckpoint
        {
            get => _canActivateCheckpoint;
            set
            {
                if (_canActivateCheckpoint != value)
                {
                    _canActivateCheckpoint = value;
                    OnPropertyChanged("CanActivateCheckpoint");
                }
            }
        }

        private bool _canFinishCheckpoint;

        public bool CanFinishCheckpoint
        {
            get => _canFinishCheckpoint;
            set
            {
                if (_canFinishCheckpoint != value)
                {
                    _canFinishCheckpoint = value;
                    OnPropertyChanged("CanFinishCheckpoint");
                }
            }
        }

        private Appointment? _activeAppointment;

        public Appointment? ActiveAppointment
        {
            get => _activeAppointment;
            set
            {
                if (_activeAppointment != value)
                {
                    _activeAppointment = value;
                    OnPropertyChanged("ActiveAppointment");
                }
            }
        }

        public string TourName { get; private set; }

        public DateTime? Date { get; private set; }

        #endregion

        public LiveTourViewModel(User loggedUser)
        {
            CheckpointCardCreatorViewModel checkpointCardCreator = new CheckpointCardCreatorViewModel(loggedUser);
            CheckpointCards = checkpointCardCreator.CreateCards();

            _checkpointActivityService = new CheckpointActivityService();
            _checkpointService = new CheckpointService();
            _appointmentService = new AppointmentService();
            _tourService = new TourService();
            _reservationService = new ReservationService();
            _guestAttendanceService = new GuestAttendanceService();

            
            ActiveAppointment = _appointmentService.GetActiveByUserId(loggedUser.Id);
            SetTourNameAndDate();
            CanActivateOrFinish(null);

            ViewGuestAttendanceCommand = new RelayCommand(ViewGuestAttendance, CanExecuteMethod);
            ActivateCheckpointCommand = new RelayCommand(ActivateCheckpoint, CanExecuteMethod);
            FinishCheckpointCommand = new RelayCommand(FinishCheckpoint, CanExecuteMethod);
            FinishAppointmentCommand = new RelayCommand(FinishAppointment, CanExecuteMethod);
            SelectionChangedCardCommand = new RelayCommand(CanActivateOrFinish, CanExecuteMethod);
        }

        private bool CanExecuteMethod(object parameter)
        {
            return true;
        }

        public void ViewGuestAttendance(object sender)
        {
            var selectedGuestAttendanceCard = sender as CheckpointCardViewModel;
            GuestAttendancePage guestAttendancePage = new GuestAttendancePage(selectedGuestAttendanceCard, TourName, Date);
            System.Windows.Application.Current.Windows.OfType<MainWindow>().FirstOrDefault().ToursOverviewFrame.Content = guestAttendancePage;

        }

        private void SetTourNameAndDate()
        {
            if (ActiveAppointment == null)
            {
                TourName = "Active tour doesn't exist";
                Date = null;
            }
            else
            {
                TourName = _tourService.GetById(ActiveAppointment.TourId).Name;
                Date = ActiveAppointment.Start;
            }
        }

        private void ActivateCheckpoint(object sender)
        {
            _checkpointActivityService.ActivateCheckpoint(SelectedCheckpointCard.ActivityId);
            CreateQueryForGuests(SelectedCheckpointCard);
            UpdateCard(false);
            CanActivateOrFinish(null);
        }

        private void CreateQueryForGuests(CheckpointCardViewModel selectedCheckpointCard)
        {
            var reservations = _reservationService.GetAllByAppointmentId(ActiveAppointment.Id);
            var activatedCheckpoint = _checkpointActivityService.GetById(selectedCheckpointCard.ActivityId);
            var checkpointName = _checkpointService.GetById(activatedCheckpoint.CheckpointId).Name;
            _guestAttendanceService.CreateAttendanceQueries(reservations, activatedCheckpoint, checkpointName);
        }

        private void FinishCheckpoint(object sender)
        {
            if (SelectedCheckpointCard.Type == CheckpointType.END)
            {
                FinishAppointment(null);
            }

            _checkpointActivityService.FinishCheckpoint(SelectedCheckpointCard.ActivityId);

            UpdateCard(true);
            CanActivateOrFinish(null);

        }

        private void UpdateCard(bool isFinish)
        {
            SelectedCheckpointCard.StatusEnum = isFinish ? CheckpointStatus.FINISHED : CheckpointStatus.ACTIVE;

            SelectedCheckpointCard.SetStatusAndBackground();
            SelectedCheckpointCard.SetCanShowAttendance();
        }

        private void CanActivateOrFinish(object sender)
        {
            var isExistsActiveCheckpoint = CheckpointCards.Any(c => c.StatusEnum == CheckpointStatus.ACTIVE);

            CanActivateCheckpoint = (SelectedCheckpointCard?.StatusEnum == CheckpointStatus.NOT_STARTED &&
                                     !isExistsActiveCheckpoint);
            CanFinishCheckpoint = (SelectedCheckpointCard?.StatusEnum == CheckpointStatus.ACTIVE);

            if (SelectedCheckpointCard == null || SelectedCheckpointCard.StatusEnum == CheckpointStatus.FINISHED)
            {
                CanActivateCheckpoint = false;
                CanFinishCheckpoint = false;

            }
        }

        private void FinishAppointment(object sender)
        {
            _appointmentService.FinishAppointment(ActiveAppointment.Id);
            ResetActiveAppointment();
        }

        private void ResetActiveAppointment()
        {
            ActiveAppointment = null;
            SetTourNameAndDate();
        }
    }
}
