using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Domain.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Threading;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.WPF.Views.TourGuide;

namespace SOSTeam.TravelAgency.WPF.ViewModels.TourGuide
{
    public class TodayToursViewModel : ViewModel
    {
        #region Services

        private readonly AppointmentService _appointmentService;
        private readonly CheckpointService _checkpointService;
        private readonly CheckpointActivityService _checkpointActivityService;
        private readonly GuestAttendanceService _guestAttendanceService;
        private readonly ReservationService _reservationService;

        #endregion

        #region Commands
        public RelayCommand StartTourCommand { get; set; }

        #endregion

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

        private readonly User _loggedUser;

        public TodayToursViewModel(User loggedUser, DispatcherTimer timer)
        {
            _loggedUser = loggedUser;
            TodayDate = DateOnly.FromDateTime(DateTime.Now);
            TourCardCreatorViewModel tourCardCreator = new TourCardCreatorViewModel();
            TodayTourCards = tourCardCreator.CreateCards(loggedUser, CreationType.TODAY);

            _appointmentService = new AppointmentService();
            _checkpointService = new CheckpointService();
            _checkpointActivityService = new CheckpointActivityService();
            _reservationService = new ReservationService();
            _guestAttendanceService = new GuestAttendanceService();

            timer.Tick += UpdateTourCards;

            StartTourCommand = new RelayCommand(StartTour, CanExecuteMethod);
            
        }

        private void UpdateTourCards(object sender, EventArgs e)
        {
            var activeAppointment = _appointmentService.GetActiveByUserId(_loggedUser.Id);
            foreach (var tourCard in TodayTourCards)
            {
                var appointment = _appointmentService.GetById(tourCard.AppointmentId);
                tourCard.SetAppointmentStatusAndBackground(appointment);
                tourCard.SetCanStart(appointment, activeAppointment);
            }
        }

        private bool CanExecuteMethod(object parameter)
        {
            return true;
        }

        public void StartTour(object sender)
        {
            var selectedTourCard = sender as TourCardViewModel;
            _appointmentService.StartAppointment(selectedTourCard.AppointmentId);

            var messageBoxWindow = CreateMessageBox();

            var result = messageBoxWindow.ShowDialog();

            if (result == true)
            {
                _checkpointActivityService.CreateActivities(_checkpointService.GetAllByTourId(selectedTourCard.TourId), selectedTourCard.AppointmentId);
                CreateGuestAttendances(selectedTourCard);
                LiveTourPage liveTourPage = new LiveTourPage(_loggedUser);
                System.Windows.Application.Current.Windows.OfType<MainWindow>().FirstOrDefault().MainFrame.Content = liveTourPage;
            }
        }

        private static MessageBoxWindow CreateMessageBox()
        {
            const string message = "Are you sure you want to start the tour?\n";

            var messageBoxViewModel = new MessageBoxViewModel("Alert", "/Resources/Icons/warning.png", message);
            var messageBoxWindow = new MessageBoxWindow
            {
                DataContext = messageBoxViewModel
            };
            return messageBoxWindow;
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
