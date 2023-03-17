using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using TravelAgency.Model;
using TravelAgency.Repository;

namespace TravelAgency.View
{
    /// <summary>
    /// Interaction logic for StartTourWindow.xaml
    /// </summary>
    public partial class StartTourWindow : Window
    {
        public ObservableCollection<Appointment> TodayAppointmentsByTour { get; set; }
        public Appointment SelectedAppointment { get; set; }

        private readonly AppointmentRepository _appointmentRepository;

        private readonly CheckpointRepository _checkpointRepository;

        public Tour SelectedTour { get; set; }

        public StartTourWindow(Tour selectedTour, AppointmentRepository appointmentRepository, CheckpointRepository checkpointRepository)
        {
            InitializeComponent();
            DataContext = this;
            SelectedTour = selectedTour;
            _appointmentRepository = appointmentRepository;
            _checkpointRepository = checkpointRepository;
            TodayAppointmentsByTour = new ObservableCollection<Appointment>(FindAppointmentsByTour());
            ExistsActiveAppointment();
        }
        private void ExistsActiveAppointment()
        {
            List<Appointment> todayAppointments = FindTodayAppointments();
            Appointment activeAppointment = todayAppointments.Find(a => a.Started == true && a.Finished == false);
            if (activeAppointment != null)
            {
                DisableStartTour();
            }
        }

        private void DisableStartTour()
        {
            startButton.IsEnabled = false;
            label.Content = "Ne možete startovati turu (već postoji aktivna tura)!";
            label.Foreground = new SolidColorBrush(Colors.Red);
        }
        private List<Appointment> FindTodayAppointments()
        {
            List<Appointment> appointments = new List<Appointment>(_appointmentRepository.GetAll());
            DateOnly today = DateOnly.FromDateTime(DateTime.Today);

            return appointments.FindAll(a => a.Date.Equals(today));
        }

        private List<Appointment> FindAppointmentsByTour()
        {
            List<Appointment> todayAppointments = FindTodayAppointments();

            return todayAppointments.FindAll(a => a.TourId == SelectedTour.Id);
        }

        private void StartButtonClick(object sender, RoutedEventArgs e)
        {
            if (SelectedAppointment == null)
            {
                MessageBox.Show("Morate odabrati satnicu!");
            }
            else
            {
                ActivateAppointment();
                ActivateStartCheckpoint();
                MessageBox.Show("Tura je uspešno startovana!");
                DisableStartTour();
                Close();
            }
        }

        private void ActivateAppointment()
        {
            SelectedAppointment.Started = true;
            _appointmentRepository.Update(SelectedAppointment);
        }
        private void ActivateStartCheckpoint()
        {
            Checkpoint startCheckpoint = FindStartCheckpoint();
            startCheckpoint.Active = true;
            _checkpointRepository.Update(startCheckpoint);
        }

        private Checkpoint FindStartCheckpoint()
        {
            List<Checkpoint> checkpoints = new List<Checkpoint>(_checkpointRepository.GetAll());
            return checkpoints.Find(c => c.TourId == SelectedTour.Id && c.Type == CheckpointType.START) ?? throw new ArgumentException();
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
