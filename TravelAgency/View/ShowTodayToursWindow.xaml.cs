using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using TravelAgency.Model;
using TravelAgency.Repository;

namespace TravelAgency.View
{
    /// <summary>
    /// Interaction logic for ShowTodayToursWindow.xaml
    /// </summary>
    public partial class ShowTodayToursWindow : Window
    {
        public ObservableCollection<Tour> Tours { get; set; }
        public ObservableCollection<Tour> TodayTours { get; set; }
        public ObservableCollection<Appointment> Appointments { get; set; }
        public ObservableCollection<Appointment> TodayAppointments { get; set; }
        public List<Checkpoint> Checkpoints { get; set; }

        private readonly AppointmentRepository _appointmentRepository;

        private readonly CheckpointRepository _checkpointRepository;

        public AppointmentRepository AppointmentRepository => _appointmentRepository;

        public CheckpointRepository CheckpointRepository => _checkpointRepository;

        public Tour SelectedTour { get; set; }

        public ShowTodayToursWindow(ObservableCollection<Tour> tours)
        {
            InitializeComponent();
            DataContext = this;

            _appointmentRepository = new AppointmentRepository();
            _checkpointRepository = new CheckpointRepository();

            Appointments = new ObservableCollection<Appointment>(_appointmentRepository.GetAll());
            Tours = tours;

            TodayTours = FindTodayTours();
        }

        private ObservableCollection<Tour> FindTodayTours()
        {
            ObservableCollection<Tour> todayTours = new ObservableCollection<Tour>();
            DateOnly today = DateOnly.FromDateTime(DateTime.Today);
            foreach (Appointment appointment in Appointments)
            {
                foreach (Tour tour in Tours)
                {
                    if (tour.Id == appointment.TourId && appointment.Date.Equals(today))
                    {
                        todayTours.Add(tour);
                    }
                }
            }
            todayTours = new ObservableCollection<Tour>(todayTours.Distinct());
            return todayTours;
        }

        private void PickTimeButtonClick(object sender, RoutedEventArgs e)
        {
            if (SelectedTour == null)
            {
                MessageBox.Show("Morate odabrati turu!");
            }
            else
            {
                StartTourWindow startTourWindow = new StartTourWindow(SelectedTour, TodayTours, AppointmentRepository, CheckpointRepository);
                startTourWindow.Owner = Window.GetWindow(this);
                startTourWindow.ShowDialog();
            }
        }
    }
}
