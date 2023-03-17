using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;
using TravelAgency.Model;
using TravelAgency.Repository;
using static System.Net.Mime.MediaTypeNames;

namespace TravelAgency.View
{
    /// <summary>
    /// Interaction logic for TourOverview.xaml
    /// </summary>
    public partial class ShowToursWindow : Window
    {
        public User LoggedInUser { get; set; }
        public static ObservableCollection<Tour> Tours { get; set; }
        public List<Appointment> Appointments { get; set; }
        public Tour SelectedTour { get; set; }
        
        private readonly TourRepository _tourReository;
        private readonly AppointmentRepository _appointmentReository;
        private readonly CheckpointRepository _checkpointRepository;

        public ShowToursWindow(User user)
        {
            InitializeComponent();
            DataContext = this;
            LoggedInUser = user;
            _tourReository = new TourRepository();
            _appointmentReository = new AppointmentRepository();
            _checkpointRepository = new CheckpointRepository();
            Tours = new ObservableCollection<Tour>(_tourReository.GetByUser(user));
            Appointments = new List<Appointment>(_appointmentReository.GetAll());
            AppointmentRegularEnd();
        }

        private void AppointmentRegularEnd()
        {
            foreach(Tour tour in Tours)
            {
                foreach(Appointment appointment in Appointments)
                {
                    if(appointment.TourId == tour.Id && appointment.Started == true)
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

            if(endDate.CompareTo(DateTime.Now) < 0)
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
                if(checkpoint.TourId == tour.Id)
                {
                    checkpoint.Active = false;
                    _checkpointRepository.Update(checkpoint);
                }
            }
        }

        private void DeleteButtonClick(object sender, RoutedEventArgs e)
        {
            if(SelectedTour!= null)
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

        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
            CreateTour createTour = new CreateTour(LoggedInUser);
            createTour.Owner = Window.GetWindow(this);
            createTour.Show();
        }

        private void TodayToursButtonClick(object sender, RoutedEventArgs e)
        {
            ObservableCollection<Tour> userTours = new ObservableCollection<Tour>(_tourReository.GetByUser(LoggedInUser));
            TodayTourView todayTourView = new TodayTourView(userTours);
            todayTourView.Owner = Window.GetWindow(this);
            todayTourView.Show(); 
        }
    }
}
