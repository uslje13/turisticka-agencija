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
        

        private ObservableCollection<TourCardViewModel> _todayTourCards;

        public ObservableCollection<TourCardViewModel> TodayTourCards
        {
            get => _todayTourCards;
            set
            {
                if (_todayTourCards != value)
                {
                    _todayTourCards = value;
                    OnPropertyChanged("TodayTourCards");
                }
            }
        }

        public DateOnly TodayDate { get; set; }

        public User LoggedUser { get; set; }

        public RelayCommand StartTourCommand { get; set; }

        public TodayToursViewModel(User loggedUser)
        {
            LoggedUser = loggedUser;
            TodayDate = DateOnly.FromDateTime(DateTime.Now);
            TodayTourCards = new ObservableCollection<TourCardViewModel>();

            _tourService = new TourService();
            _locationService = new LocationService();
            _appointmentService = new AppointmentService();
            _imageService = new ImageService();
            _appointmentService = new AppointmentService();
            _checkpointService = new CheckpointService();
            _checkpointActivityService = new CheckpointActivityService();
            _reservationService = new ReservationService();
            _guestAttendanceService = new GuestAttendanceService();

            StartTourCommand = new RelayCommand(StartTour, CanExecuteMethod);
            
            
            FillObservableCollection();
        }

        public void FillObservableCollection()
        {
            TodayTourCards.Clear();
            foreach (var appointment in _appointmentService.GetTodayAppointmentsByUserId(LoggedUser.Id))
            {
                foreach (var tour in _tourService.GetAll())
                {
                    if (appointment.TourId == tour.Id)
                    {
                        TourCardViewModel viewModel = new TourCardViewModel();

                        SetTourAndAppointmentFields(viewModel, appointment, tour);

                        SetLocationField(tour, viewModel);

                        SetImageField(tour, viewModel);

                        CanStartAppointment(LoggedUser, viewModel);

                        TodayTourCards.Add(viewModel);
                    }
                }
            }
        }
        private bool CanExecuteMethod(object parameter)
        {
            return true;
        }

        private void SetImageField(Tour tour, TourCardViewModel tourCard)
        {
            Image? image = _imageService.GetTourCover(tour.Id);
            tourCard.SetCoverImage(image);
        }

        private void SetLocationField(Tour tour, TourCardViewModel tourCard)
        {
            var location = _locationService.GetById(tour.LocationId);
            tourCard.SetLocation(location);
        }

        private void SetTourAndAppointmentFields(TourCardViewModel tourCard, Appointment appointment, Tour tour)
        {
            tourCard.AppointmentId = appointment.Id;
            tourCard.Date = appointment.Date;
            tourCard.Time = appointment.Time;
            tourCard.TourId = tour.Id;
            tourCard.Name = tour.Name;
            tourCard.SetAppointmentStatus(appointment);
        }

        public void CanStartAppointment(User loggedUser, TourCardViewModel viewModel)
        {
            Appointment? activeAppointment = _appointmentService.GetAllByUserId(loggedUser.Id).Find(a => a.Started && !a.Finished);
            viewModel.SetCanStart(activeAppointment);
        }

        public void StartTour(object sender)
        {
            var selectedTourCard = sender as TourCardViewModel;
            _appointmentService.ActivateAppointment(selectedTourCard.AppointmentId);

            _checkpointActivityService.CreateActivities(_checkpointService.GetAllByTourId(selectedTourCard.TourId), selectedTourCard.AppointmentId);
            CreateGuestAttendances(selectedTourCard);

            LiveTourPage liveTourPage = new LiveTourPage(LoggedUser);
            System.Windows.Application.Current.Windows.OfType<MainWindow>().FirstOrDefault().ToursOverviewFrame.Content = liveTourPage;
        }

        private void CreateGuestAttendances(TourCardViewModel startedAppointment)
        {
            var activeCheckpoint = _checkpointActivityService.GetAllByAppointmentId(startedAppointment.AppointmentId)
                .Find(a => a.Status == CheckpointStatus.ACTIVE);
            var activeCheckpointName = _checkpointService.GetById(activeCheckpoint.CheckpointId).Name;
            var reservationList = _reservationService.GetAllByAppointmentId(startedAppointment.AppointmentId);

            _guestAttendanceService.CreateAttendanceQueries(reservationList, activeCheckpoint, activeCheckpointName);
        }

    }
}
