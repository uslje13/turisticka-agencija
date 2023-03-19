using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using TravelAgency.Converter;
using TravelAgency.Model;
using TravelAgency.Repository;

namespace TravelAgency.View
{
    /// <summary>
    /// Interaction logic for TourOverview.xaml
    /// </summary>
    public partial class ShowToursWindow : Window
    {
        public List<Tour> Tours { get; set; }

        public ObservableCollection<TourDTO> ToursDTO { get; set; }
        public List<Appointment> Appointments { get; set; }
        public User LoggedInUser { get; set; }

        public Tour SelectedTour { get; set; }

        private readonly TourRepository _tourReository;

        private readonly AppointmentRepository _appointmentReository;

        private LocationConverter _locationConverter;

        private void FillObservableCollection()
        {
            foreach(Tour tour in Tours)
            {
                string location = _locationConverter.GetFullNameById(tour.LocationId);
                ToursDTO.Add(new TourDTO(tour.Id, tour.Name, tour.Language, location, tour.MaxNumOfGuests, tour.Duration));
            }
        }

        public void UpdateObservableCollection()
        {
            ToursDTO.Clear();
            FillObservableCollection();
        }


        public ShowToursWindow(User user)
        {
            InitializeComponent();
            DataContext = this;
            LoggedInUser = user;
            deleteButton.IsEnabled = false;
            

            _tourReository = new TourRepository();
            _appointmentReository = new AppointmentRepository();

            Tours = new List<Tour>(_tourReository.GetByUser(user));
            Appointments = new List<Appointment>(_appointmentReository.GetAll());

            _locationConverter = new();
            ToursDTO = new ObservableCollection<TourDTO>();
            FillObservableCollection();

            TourDurationExpiredEnd();
        }

        private void TourDurationExpiredEnd()
        {
            List<Appointment> appointments = _appointmentReository.GetAllByTours(Tours);
            foreach (Appointment appointment in appointments)
            {
                Tour tour = _tourReository.Get(appointment.TourId);
                CheckAppointmentEnd(tour, appointment);
            }
        }

        private void CheckAppointmentEnd(Tour tour, Appointment appointment)
        {
            DateTime startDate = appointment.Date.ToDateTime(appointment.Time);

            (int durationDays, int durationHours) = ConvertDuration(tour.Duration);

            DateTime endDate = startDate.AddDays(durationDays).AddHours(durationHours);

            if (endDate.CompareTo(DateTime.Now) < 0)
            {
                appointment.Finished = true;
                _appointmentReository.Update(appointment);
            }
        }

        private (int, int) ConvertDuration(int duration)
        {
            int days = duration / 24;
            int hours = duration % 24;

            return (days, hours);
        }

        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
            CreateTourWindow createTourWindow = new CreateTourWindow(LoggedInUser, Tours);
            createTourWindow.Owner = Window.GetWindow(this);
            createTourWindow.ShowDialog();
            UpdateObservableCollection();
        }

        private void TodayToursButtonClick(object sender, RoutedEventArgs e)
        {
            ShowTodayToursWindow showTodayToursWindow = new ShowTodayToursWindow(Tours);
            showTodayToursWindow.Owner = Window.GetWindow(this);
            showTodayToursWindow.ShowDialog();
        }

        private void ActiveTourButtonClick(object sender, RoutedEventArgs e)
        {
            ShowTourCheckpointsWindow showTourCheckpoints = new ShowTourCheckpointsWindow(Tours);
            showTourCheckpoints.ShowDialog();
        }

    }
}
