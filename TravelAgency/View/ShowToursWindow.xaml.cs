using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using TravelAgency.Model;
using TravelAgency.Repository;

namespace TravelAgency.View
{
    /// <summary>
    /// Interaction logic for TourOverview.xaml
    /// </summary>
    public partial class ShowToursWindow : Window
    {
        private ObservableCollection<Tour> _tours;

        public ObservableCollection<Tour> Tours
        {
            get => _tours;
            set
            {
                if (!value.Equals(_tours))
                {
                    _tours = value;
                    OnPropertyChanged();
                }
            }
        }
        public List<Appointment> Appointments { get; set; }
        public User LoggedInUser { get; set; }

        public Tour SelectedTour { get; set; }

        private readonly TourRepository _tourReository;

        private readonly AppointmentRepository _appointmentReository;

        private readonly CheckpointRepository _checkpointRepository;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public ShowToursWindow(User user)
        {
            InitializeComponent();
            DataContext = this;
            LoggedInUser = user;
            deleteButton.IsEnabled = false;

            _tourReository = new TourRepository();
            _appointmentReository = new AppointmentRepository();
            _checkpointRepository = new CheckpointRepository();

            Tours = new ObservableCollection<Tour>(_tourReository.GetByUser(user));
            Appointments = new List<Appointment>(_appointmentReository.GetAll());

            AppointmentRegularEnd();
        }

        private void AppointmentRegularEnd()
        {
            foreach (Tour tour in Tours)
            {
                foreach (Appointment appointment in Appointments)
                {
                    if (appointment.TourId == tour.Id && appointment.Started == true)
                    {
                        CheckAppointmentEnd(tour, appointment);
                    }
                }
            }
        }

        private void CheckAppointmentEnd(Tour tour, Appointment appointment)
        {
            DateTime dateStart = appointment.Date.ToDateTime(appointment.Time);

            (int durationInDays, int durationInHours) = ConvertDuration(tour.Duration);

            DateTime endDate = dateStart.AddDays(durationInDays).AddHours(durationInHours);

            if (endDate.CompareTo(DateTime.Now) < 0)
            {
                appointment.Finished = true;
                _appointmentReository.Update(appointment);
                DeactivateCheckpoints(tour);
            }

        }

        private (int, int) ConvertDuration(int duration)
        {
            int days = duration / 24;
            int hours = duration % 24;

            return (days, hours);
        }

        private void DeactivateCheckpoints(Tour tour)
        {
            List<Checkpoint> checkpoints = new List<Checkpoint>(_checkpointRepository.GetAll());
            foreach (Checkpoint checkpoint in checkpoints)
            {
                if (checkpoint.TourId == tour.Id)
                {
                    checkpoint.Active = false;
                    _checkpointRepository.Update(checkpoint);
                }
            }
        }

        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
            CreateTourWindow createTourWindow = new CreateTourWindow(LoggedInUser, Tours);
            createTourWindow.Owner = Window.GetWindow(this);
            createTourWindow.ShowDialog();
        }

        private void TodayToursButtonClick(object sender, RoutedEventArgs e)
        {
            ShowTodayToursWindow showTodayToursWindow = new ShowTodayToursWindow(Tours);
            showTodayToursWindow.Owner = Window.GetWindow(this);
            showTodayToursWindow.ShowDialog();
        }

        /*
            private void DeleteButtonClick(object sender, RoutedEventArgs e)
            {
                if (SelectedTour != null)
                {
                    MessageBoxResult result = MessageBox.Show("Da li ste sigurni", "Obrisite turu",
                        MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        CheckpointRepository checkpointRepository = new CheckpointRepository();
                        LocationRepository locationRepository = new LocationRepository();
                        AppointmentRepository appointmentRepository = new AppointmentRepository();
                        ImageRepository imageRepository = new ImageRepository();
                        locationRepository.DeleteById(SelectedTour.LocationId);
                        _tourReository.Delete(SelectedTour);
                        checkpointRepository.DeleteByTourId(SelectedTour.Id);
                        appointmentRepository.DeleteByTourId(SelectedTour.Id);
                        imageRepository.DeleteByTourId(SelectedTour.Id);
                        Tours.Remove(SelectedTour);
                    }
                }
            }
        */
    }
}
