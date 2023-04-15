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
            foreach (var checkpointActivity in _checkpointActivityService.GetAll())
            {
                if (checkpointActivity.AppointmentId == ActiveAppointment.Id)
                {
                    CheckpointCardViewModel viewModel = new CheckpointCardViewModel();
                    Checkpoint checkpoint = _checkpointService.GetById(checkpointActivity.CheckpointId);
                    viewModel.CheckpointId = checkpointActivity.CheckpointId;
                    viewModel.ActivityId = checkpointActivity.Id;
                    viewModel.Name = checkpoint.Name;
                    viewModel.Type = checkpoint.Type;
                    viewModel.Status = checkpointActivity.Status;
                    viewModel.GuestsCalled = checkpointActivity.GuestsCalled;

                    CheckpointCards.Add(viewModel);
                }
            }
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
            List<GuestAttendance> guestAttendances = new List<GuestAttendance>();
            foreach (var reservation in _reservationService.GetAll())
            {
                if (reservation.AppointmentId == ActiveAppointment.Id)
                {
                    foreach (var checkpointActivity in _checkpointActivityService.GetAll())
                    {
                        if (checkpointActivity.AppointmentId == ActiveAppointment.Id && checkpointActivity.CheckpointId == selectedGuestAttendance.CheckpointId)
                        {
                            GuestAttendance guestAttendance = new GuestAttendance();
                            guestAttendance.UserId = reservation.UserId;
                            guestAttendance.CheckpointActivityId = checkpointActivity.AppointmentId;
                            guestAttendance.Presence = GuestPresence.UNKNOWN;
                            guestAttendance.Message = "Da li ste bili prisutni na čekpointu: " +
                                                      _checkpointService.GetById(checkpointActivity.CheckpointId).Name +
                                                      " ?";
                            guestAttendances.Add(guestAttendance);

                            checkpointActivity.GuestsCalled = true;

                        }
                    }
                }
            }
            _guestAttendanceService.SaveAll(guestAttendances);
        }
    }
}
