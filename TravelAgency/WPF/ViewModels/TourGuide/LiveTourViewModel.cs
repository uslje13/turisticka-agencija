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
                    _checkpointCards =  value;
                    OnPropertyChanged("CheckpointCards");
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

        public User LoggedUser { get; set; }

        private readonly CheckpointActivityService _checkpointActivityService;
        private readonly CheckpointService _checkpointService;
        private readonly AppointmentService _appointmentService;
        private readonly TourService _tourService;
        private readonly ReservationService _reservationService; 
        private readonly GuestAttendanceService _guestAttendanceService;

        public RelayCommand ViewGuestAttendanceCommand { get; set; }

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

            ActiveAppointment = _appointmentService.GetActiveByUserId(loggedUser.Id);
            FillObservableCollection();
            SetTourNameAndDate();

            ViewGuestAttendanceCommand = new RelayCommand(ViewGuestAttendance, CanExecuteMethod);
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
                Date = new DateTime(ActiveAppointment.Date.Year, ActiveAppointment.Date.Month, ActiveAppointment.Date.Day,
                    ActiveAppointment.Time.Hour, ActiveAppointment.Time.Minute, ActiveAppointment.Time.Second);
            }
        }

        public void FillObservableCollection()
        {
            if (ActiveAppointment != null)
            {
                foreach (var checkpointActivity in _checkpointActivityService.GetAll())
                {
                    if (checkpointActivity.AppointmentId == ActiveAppointment.Id)
                    {
                        var checkpoint = _checkpointService.GetById(checkpointActivity.CheckpointId);
                        var viewModel = CreateCheckpointCard(checkpointActivity, checkpoint);
                        CheckpointCards.Add(viewModel);
                    }
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
                GuestsCalled = checkpointActivity.GuestsCalled
            };
            return viewModel;
        }

        public void ViewGuestAttendance(object sender)
        {
            var selectedGuestAttendance = sender as CheckpointCardViewModel;
            if (!selectedGuestAttendance.GuestsCalled)
            {
                CreateQueryForGuests(selectedGuestAttendance);
            }
        }

        private void CreateQueryForGuests(CheckpointCardViewModel selectedGuestAttendance)
        {
            var guestAttendances = new List<GuestAttendance>();
            foreach (var reservation in _reservationService.GetAll())
            {
                if (reservation.AppointmentId == ActiveAppointment.Id)
                {
                    foreach (var checkpointActivity in _checkpointActivityService.GetAll())
                    {
                        if (checkpointActivity.AppointmentId == ActiveAppointment.Id && checkpointActivity.CheckpointId == selectedGuestAttendance.CheckpointId)
                        {
                            var guestAttendance = CreateGuestAttendance(reservation, checkpointActivity);
                            guestAttendances.Add(guestAttendance);

                            checkpointActivity.GuestsCalled = true;
                        }
                    }
                }
            }

            if (guestAttendances.Count > 0)
            {
                _guestAttendanceService.SaveAll(guestAttendances);
            }
        }

        private GuestAttendance CreateGuestAttendance(Reservation reservation, CheckpointActivity checkpointActivity)
        {
            var guestAttendance = new GuestAttendance
            {
                UserId = reservation.UserId,
                CheckpointActivityId = checkpointActivity.AppointmentId,
                Presence = GuestPresence.UNKNOWN,
                Message = "Da li ste bili prisutni na čekpointu: " +
                          _checkpointService.GetById(checkpointActivity.CheckpointId).Name +
                          " ?"
            };
            return guestAttendance;
        }
    }
}
