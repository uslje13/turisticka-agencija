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

        private CheckpointCardViewModel _selectedCheckpointCard;

        public CheckpointCardViewModel SelectedCheckpointCard
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

        private Appointment _activeAppointment;

        public Appointment ActiveAppointment
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

        private void SetTourNameAndDate()
        {
            if (ActiveAppointment == null)
            {
                TourName = "Active tour doesn't exist";
                Date = null;
            }
            else
            {
                Tour tour = _tourService.GetById(ActiveAppointment.TourId);
                TourName = tour.Name;
                Date = new DateTime(ActiveAppointment.Date.Year, ActiveAppointment.Date.Month,
                    ActiveAppointment.Date.Day,
                    ActiveAppointment.Time.Hour, ActiveAppointment.Time.Minute, ActiveAppointment.Time.Second);
            }
        }

        public void FillObservableCollection()
        {
            CheckpointCards.Clear();
            if (ActiveAppointment != null)
            {
                foreach (var checkpointActivity in _checkpointActivityService.GetAllByAppointmentId(ActiveAppointment.Id))
                {
                    var checkpoint = _checkpointService.GetById(checkpointActivity.CheckpointId);
                    var viewModel = CreateCheckpointCard(checkpointActivity, checkpoint);
                    CheckpointCards.Add(viewModel);
                }
            }
        }

        private static CheckpointCardViewModel CreateCheckpointCard(CheckpointActivity checkpointActivity,
            Checkpoint checkpoint)
        {
            var viewModel = new CheckpointCardViewModel
            {
                CheckpointId = checkpointActivity.CheckpointId,
                ActivityId = checkpointActivity.Id,
                Name = checkpoint.Name,
                Type = checkpoint.Type,
                Status = checkpointActivity.Status,
                CanShowAttendance = true,
            };

            if (viewModel.Status == CheckpointStatus.NOT_STARTED)
            {
                viewModel.CanShowAttendance = false;
            }

            return viewModel;
        }

        public void ViewGuestAttendance(object sender)
        {
            var selectedGuestAttendanceCard = sender as CheckpointCardViewModel;
            GuestAttendancePage guestAttendancePage = new GuestAttendancePage(selectedGuestAttendanceCard, TourName, Date);
            System.Windows.Application.Current.Windows.OfType<MainWindow>().FirstOrDefault().ToursOverviewFrame.Content = guestAttendancePage;

        }

        private void CreateQueryForGuests(CheckpointCardViewModel selectedCheckpointCard)
        {
            var guestAttendances = new List<GuestAttendance>();
            foreach (var reservation in _reservationService.GetAllByAppointmentId(ActiveAppointment.Id))
            {
                var guestAttendance = CreateGuestAttendance(reservation, selectedCheckpointCard);
                guestAttendances.Add(guestAttendance);
            }

            if (guestAttendances.Count > 0)
            {
                _guestAttendanceService.SaveAll(guestAttendances);
            }
        }

        private void ActivateCheckpoint(object sender)
        {
            if (SelectedCheckpointCard != null)
            {
                var checkpointActivity = _checkpointActivityService.GetById(SelectedCheckpointCard.ActivityId);
                checkpointActivity.Status = CheckpointStatus.ACTIVE;
                _checkpointActivityService.Update(checkpointActivity);
                CreateQueryForGuests(SelectedCheckpointCard);

                FillObservableCollection();
                CheckCanActivateCheckpoint();
            }
        }

        private void FinishCheckpoint(object sender)
        {
            if (SelectedCheckpointCard != null && SelectedCheckpointCard.Status == CheckpointStatus.ACTIVE)
            {
                //If end checkpoint finish active tour
                if (SelectedCheckpointCard.Type == CheckpointType.END)
                {
                    ActiveAppointment.Finished = true;
                    _appointmentService.Update(ActiveAppointment);
                    ActiveAppointment = null;
                    SetTourNameAndDate();
                }
                var checkpointActivity = _checkpointActivityService.GetById(SelectedCheckpointCard.ActivityId);
                checkpointActivity.Status = CheckpointStatus.FINISHED;
                _checkpointActivityService.Update(checkpointActivity);
                FillObservableCollection();

                CheckCanActivateCheckpoint();
            }
        }

        private GuestAttendance CreateGuestAttendance(Reservation reservation, CheckpointCardViewModel selectedCheckpointCard)
        {
            var guestAttendance = new GuestAttendance
            {
                UserId = reservation.UserId,
                CheckpointActivityId = selectedCheckpointCard.ActivityId,
                Presence = GuestPresence.UNKNOWN,
                Message = "Da li ste bili prisutni na čekpointu: " +
                          _checkpointService.GetById(selectedCheckpointCard.CheckpointId).Name +
                          " ?"
            };
            return guestAttendance;
        }

        private void CheckCanActivateCheckpoint()
        {
            bool foundedActive = false;
            bool existNoFinished = false;

            foreach (var checkpointCard in CheckpointCards)
            {
                if (checkpointCard.Status == CheckpointStatus.ACTIVE)
                {
                    foundedActive = true;
                }
                if (checkpointCard.Status != CheckpointStatus.FINISHED)
                {
                    existNoFinished = true;
                }
            }


            if (foundedActive || !existNoFinished)
            {
                CanActivateCheckpoint = false;
            }
            else
            {
                CanActivateCheckpoint = true;
            }
        }

        private void FinishAppointment(object sender)
        {
            if (ActiveAppointment != null)
            {
                ActiveAppointment.Finished = true;
                _appointmentService.Update(ActiveAppointment);
                ActiveAppointment = null;
                SetTourNameAndDate();
                FillObservableCollection();
            }
        }

    }
}
