using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Repositories;
using SOSTeam.TravelAgency.WPF.Views.TourGuide;

namespace SOSTeam.TravelAgency.WPF.ViewModels.TourGuide
{
    public class LiveTourViewModel : ViewModel
    {
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

        private string _tourName;

        public string TourName
        {
            get => _tourName;
            set
            {
                if (_tourName != value)
                {
                    _tourName = value;
                    OnPropertyChanged("TourName");
                }
            }

        }

        private DateTime? _date;

        public DateTime? Date
        {
            get => _date;
            set
            {
                if (_date != value)
                {
                    _date = value;
                    OnPropertyChanged("Date");
                }
            }
        }
        
        public Appointment? ActiveAppointment { get; set; }
        

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

        public User LoggedUser { get; set; }

        private readonly CheckpointActivityService _checkpointActivityService;
        private readonly CheckpointService _checkpointService;
        private readonly AppointmentService _appointmentService;
        private readonly TourService _tourService;
        private readonly ReservationService _reservationService;
        private readonly GuestAttendanceService _guestAttendanceService;

        public RelayCommand ViewGuestAttendanceCommand { get; set; }
        public RelayCommand ActivateCheckpointCommand { get; set; }
        public RelayCommand FinishCheckpointCommand { get; set; }
        public RelayCommand FinishAppointmentCommand { get; set; }

        public LiveTourViewModel(User loggedUser)
        {
            LoggedUser = loggedUser;
            CheckpointCards = new ObservableCollection<CheckpointCardViewModel>();
            _checkpointActivityService = new CheckpointActivityService();
            _checkpointService = new CheckpointService();
            _appointmentService = new AppointmentService();
            _tourService = new TourService();
            _reservationService = new ReservationService();
            _guestAttendanceService = new GuestAttendanceService();

            CanActivateCheckpoint = false;
            ActiveAppointment = _appointmentService.GetActiveByUserId(loggedUser.Id);
            FillObservableCollection();
            CheckCanActivateCheckpoint();
            SetTourNameAndDate();

            ViewGuestAttendanceCommand = new RelayCommand(ViewGuestAttendance, CanExecuteMethod);
            ActivateCheckpointCommand = new RelayCommand(ActivateCheckpoint, CanExecuteMethod);
            FinishCheckpointCommand = new RelayCommand(FinishCheckpoint, CanExecuteMethod);
            FinishAppointmentCommand = new RelayCommand(FinishAppointment, CanExecuteMethod);
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

        public void FillObservableCollection()
        {
            CheckpointCards.Clear();
            if (ActiveAppointment == null)
            {
               return;
            }
            foreach (var checkpointActivity in _checkpointActivityService.GetAllByAppointmentId(ActiveAppointment.Id))
            {
                var checkpoint = _checkpointService.GetById(checkpointActivity.CheckpointId);
                var viewModel = CreateCheckpointCard(checkpointActivity, checkpoint);
                CheckpointCards.Add(viewModel);
            }
        }

        private CheckpointCardViewModel CreateCheckpointCard(CheckpointActivity checkpointActivity, Checkpoint checkpoint)
        {
            var checkpointCard = new CheckpointCardViewModel
            {
                CheckpointId = checkpointActivity.CheckpointId,
                ActivityId = checkpointActivity.Id,
                Name = checkpoint.Name,
                Type = checkpoint.Type,
                Status = checkpointActivity.Status,
                CanShowAttendance = true,
            };

            checkpointCard.SetCanShowAttendance();

            return checkpointCard;
        }

        private void CreateQueryForGuests(CheckpointCardViewModel selectedCheckpointCard)
        {
            if (ActiveAppointment == null)
            {
                return;
            }

            var reservations = _reservationService.GetAllByAppointmentId(ActiveAppointment.Id);
            var activatedCheckpoint = _checkpointActivityService.GetById(selectedCheckpointCard.ActivityId);
            var checkpointName = _checkpointService.GetById(activatedCheckpoint.CheckpointId).Name;
            _guestAttendanceService.CreateAttendanceQueries(reservations, activatedCheckpoint, checkpointName);
        }

        private void ActivateCheckpoint(object sender)
        {
            if (SelectedCheckpointCard == null)
            {
                return;
            }
            _checkpointActivityService.ActivateCheckpoint(SelectedCheckpointCard.ActivityId);
            CreateQueryForGuests(SelectedCheckpointCard);

            FillObservableCollection();
            CheckCanActivateCheckpoint();
        }

        private void FinishCheckpoint(object sender)
        {
            if (SelectedCheckpointCard == null || SelectedCheckpointCard.Status != CheckpointStatus.ACTIVE)
            {
                return;
            }

            if (SelectedCheckpointCard.Type == CheckpointType.END)
            {
                _appointmentService.FinishAppointment(ActiveAppointment.Id);
                ActiveAppointment = null;
                SetTourNameAndDate();
            }

            _checkpointActivityService.FinishCheckpoint(SelectedCheckpointCard.ActivityId);
            FillObservableCollection();

            CheckCanActivateCheckpoint();
        }

        private void CheckCanActivateCheckpoint()
        {
            var canActivate = CheckpointCards.Any(c => c.Status == CheckpointStatus.ACTIVE);
            var existNoFinished = CheckpointCards.Any(c => c.Status != CheckpointStatus.FINISHED);

            CanActivateCheckpoint = !(canActivate || !existNoFinished);
        }

        private void FinishAppointment(object sender)
        {
            if (ActiveAppointment == null)
            {
                return;
            }
            _appointmentService.FinishAppointment(ActiveAppointment.Id);
            ActiveAppointment = null;
            SetTourNameAndDate();
            FillObservableCollection();
        }

    }
}
