using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.WPF.Views.TourGuide;

namespace SOSTeam.TravelAgency.WPF.ViewModels.TourGuide
{
    public class TodayToursViewModel : ViewModel
    {
        private readonly TourService _tourService;
        private readonly LocationService _locationService;
        private readonly AppointmentService _appointmentService;
        private readonly ImageService _imageService;
        private readonly CheckpointService _checkpointService;
        private readonly CheckpointActivityService _checkpointActivityService;
        private readonly GuestAttendanceService _guestAttendanceService;
        private readonly ReservationService _reservationService;
        

        private ObservableCollection<TourCardViewModel> _todayToursCards;

        public ObservableCollection<TourCardViewModel> TodayToursCards
        {
            get => _todayToursCards;
            set
            {
                if (_todayToursCards != value)
                {
                    _todayToursCards = value;
                    OnPropertyChanged("TodayTours");
                }
            }
        }

        private DateOnly _todayDate;

        public DateOnly TodayDate
        {
            get => _todayDate;
            set
            {
                if (_todayDate != value)
                {
                    _todayDate = value;
                    OnPropertyChanged("TodayDate");
                }
            }
        }

        public User LoggedUser { get; set; }

        public RelayCommand StartTourCommand { get; set; }

        public TodayToursViewModel(User loggedUser)
        {
            _tourService = new TourService();
            _locationService = new LocationService();
            _appointmentService = new AppointmentService();
            _imageService = new ImageService();
            _appointmentService = new AppointmentService();
            _checkpointService = new CheckpointService();
            _checkpointActivityService = new CheckpointActivityService();
            _reservationService = new ReservationService();
            _guestAttendanceService = new GuestAttendanceService();

            TodayToursCards = new ObservableCollection<TourCardViewModel>();
            StartTourCommand = new RelayCommand(StartTour, CanExecuteMethod);
            LoggedUser = loggedUser;
            TodayDate = DateOnly.FromDateTime(DateTime.Now);
            FillObservableCollection(loggedUser);
        }


        public void UpdateObservableCollection()
        {
            TodayToursCards.Clear();
            FillObservableCollection(LoggedUser);
        }

        public void FillObservableCollection(User loggedUser)
        {
            foreach (var appointment in _appointmentService.GetTodayAppointmentsByUserId(loggedUser.Id))
            {
                foreach (var tour in _tourService.GetAll())
                {
                    if (appointment.TourId == tour.Id)
                    {
                        TourCardViewModel viewModel = new TourCardViewModel();

                        SetTourAndAppointmentFields(viewModel, appointment, tour);

                        SetLocationField(tour, viewModel);

                        SetImageField(tour, viewModel);

                        CanStartAppointment(loggedUser, viewModel);

                        TodayToursCards.Add(viewModel);
                    }
                }
            }
        }
        private bool CanExecuteMethod(object parameter)
        {
            return true;
        }

        private void SetImageField(Tour tour, TourCardViewModel viewModel)
        {
            foreach (var image in _imageService.GetAllForTours())
            {
                if (image.Cover && image.EntityId == tour.Id)
                {
                    viewModel.CoverImagePath = image.Path;
                    break;
                }
            }
        }

        private void SetLocationField(Tour tour, TourCardViewModel viewModel)
        {
            foreach (var location in _locationService.GetAll())
            {
                if (location.Id == tour.LocationId)
                {
                    viewModel.Location = location.City + ", " + location.Country;
                    viewModel.LocationId = location.Id;
                    break;
                }
            }
        }

        private static void SetTourAndAppointmentFields(TourCardViewModel viewModel, Appointment appointment, Tour tour)
        {
            viewModel.AppointmentId = appointment.Id;
            viewModel.Time = appointment.Time;
            viewModel.Name = tour.Name;
            viewModel.TourId = tour.Id;
            SetAppointmentStatus(viewModel, appointment);
        }

        private static void SetAppointmentStatus(TourCardViewModel viewModel, Appointment appointment)
        {
            if (!appointment.Started && !appointment.Finished)
            {
                viewModel.Status = "Not started";
            }
            else if (appointment.Started && !appointment.Finished)
            {
                viewModel.Status = "Active";
            }
            else if (appointment.Started && appointment.Finished)
            {
                viewModel.Status = "Finished";
            }
        }

        public void StartTour(object sender)
        {
            var selectedAppointment = sender as TourCardViewModel;
            Appointment appointment = _appointmentService.GetById(selectedAppointment.AppointmentId);
            appointment.Started = true;
            _appointmentService.Update(appointment);
            CreateCheckpointActivities(selectedAppointment);
            UpdateObservableCollection();

            LiveTourPage liveTourPage = new LiveTourPage(LoggedUser);
            System.Windows.Application.Current.Windows.OfType<MainWindow>().FirstOrDefault().ToursOverviewFrame.Content = liveTourPage;
        }

        public void CanStartAppointment(User loggedUser, TourCardViewModel viewModel)
        {
            Appointment activeAppointment = _appointmentService.GetAllByUserId(loggedUser.Id)
                .Find(a => a.Started == true && a.Finished == false);
            if (activeAppointment == null)
            {
                viewModel.CanStart = true;
            }
            else
            {
                viewModel.CanStart = false;
            }
            if (viewModel.Status == "Finished")
            {
                viewModel.CanStart = false;
            }
        }

        private void CreateCheckpointActivities(TourCardViewModel startedAppointment)
        {
            List<CheckpointActivity> checkpointActivities = new List<CheckpointActivity>();
            List<GuestAttendance> guestAttendances = new List<GuestAttendance>();
            foreach (var checkpoint in _checkpointService.GetAllByTourId(startedAppointment.TourId))
            {
                CheckpointActivity checkpointActivity = new CheckpointActivity();
                checkpointActivity.AppointmentId = startedAppointment.AppointmentId;
                checkpointActivity.CheckpointId = checkpoint.Id;
                if (checkpoint.Type == CheckpointType.START)
                {
                    checkpointActivity.Status = CheckpointStatus.ACTIVE;
                }
                else
                {
                    checkpointActivity.Status = CheckpointStatus.NOT_STARTED;
                }
                checkpointActivities.Add(checkpointActivity);
            }
            _checkpointActivityService.SaveAll(checkpointActivities);
            CheckpointActivity activeCheckpoint = _checkpointActivityService.GetAllByAppointmentId(startedAppointment.AppointmentId)
                                                  .Find(a => a.Status == CheckpointStatus.ACTIVE);

            guestAttendances = CreateGuestAttendances(activeCheckpoint);
            if (guestAttendances.Count > 0)
            {
                _guestAttendanceService.SaveAll(guestAttendances);
            }
        }

        private List<GuestAttendance> CreateGuestAttendances(CheckpointActivity checkpointActivity)
        {
            var guestAttendances = new List<GuestAttendance>();
            foreach (var reservation in _reservationService.GetAllByAppointmentId(checkpointActivity.AppointmentId))
            {
                guestAttendances.Add(CreateGuestAttendance(reservation, checkpointActivity)); 
            }
            return guestAttendances;
        }

        private GuestAttendance CreateGuestAttendance(Reservation reservation, CheckpointActivity checkpointActivity)
        {
            var guestAttendance = new GuestAttendance
            {
                UserId = reservation.UserId,
                CheckpointActivityId = checkpointActivity.Id,
                Presence = GuestPresence.UNKNOWN,
                Message = "Da li ste bili prisutni na čekpointu: " +
                          _checkpointService.GetById(checkpointActivity.CheckpointId).Name +
                          " ?"
            };
            return guestAttendance;
        }

    }
}
