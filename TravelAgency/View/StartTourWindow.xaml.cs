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

        private readonly CheckpointActivityRepository _checkpointActivityRepository;

        public List<Tour> UserTours { get; set; }
        public Tour SelectedTour { get; set; }

        public StartTourWindow(Tour selectedTour, List<Tour> userTours, AppointmentRepository appointmentRepository, CheckpointRepository checkpointRepository)
        {
            InitializeComponent();
            DataContext = this;

            _appointmentRepository = appointmentRepository;
            _checkpointRepository = checkpointRepository;
            _checkpointActivityRepository = new CheckpointActivityRepository();

            UserTours = userTours;
            SelectedTour = selectedTour;
            TodayAppointmentsByTour = new ObservableCollection<Appointment>(FindAppointmentsByTour());
            ExistsActiveAppointment();
        }
        private void ExistsActiveAppointment()
        {
            List<Appointment> appointments = FindAllAppointmentsByTours();
            Appointment activeAppointment = appointments.Find(a => a.Started == true && a.Finished == false);
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

        private List<Appointment> FindAllAppointmentsByTours()
        {
            List<Appointment> appointments = new List<Appointment>(_appointmentRepository.GetAll());
            List<Appointment> todayAppointments = new List<Appointment>();
            foreach (Tour tour in UserTours)
            {
                foreach (Appointment appointment in appointments)
                {
                    if (appointment.TourId == tour.Id)
                    {
                        todayAppointments.Add(appointment);
                    }
                }
            }
            return todayAppointments;
        }

        private List<Appointment> FindAppointmentsByTour()
        {
            List<Appointment> todayAppointments = new List<Appointment>(_appointmentRepository.GetTodayAppointments());

            return todayAppointments.FindAll(a => a.TourId == SelectedTour.Id);
        }

        private void StartButtonClick(object sender, RoutedEventArgs e)
        {
            if (SelectedAppointment == null)
            {
                MessageBox.Show("Morate odabrati satnicu!");
            }
            else if (SelectedAppointment.Finished == true)
            {
                MessageBox.Show("TURA JE ZAVŠENA!\nNe možete započeti turu.");
            }
            else
            {
                ActivateAppointment();
                SaveCheckointsActivity();
                MessageBox.Show("Tura je uspešno startovana!");
                DisableStartTour();
                Close();
                ShowTourCheckpointsWindow showTourCheckpoints = new ShowTourCheckpointsWindow(UserTours);
                showTourCheckpoints.ShowDialog();
            }
        }
        private void ActivateAppointment()
        {
            SelectedAppointment.Started = true;
            _appointmentRepository.Update(SelectedAppointment);
        }

        private void SaveCheckointsActivity()
        {
            List<Checkpoint> checkpoints = _checkpointRepository.GetAllByTourId(SelectedTour);
            List<CheckpointActivity> checkpointActivities = new List<CheckpointActivity>();
            foreach (Checkpoint checkpoint in checkpoints) 
            {
                checkpointActivities.Add(CreateCheckpointActivity(checkpoint));
            }
            _checkpointActivityRepository.SaveAll(checkpointActivities);
        }

        private CheckpointActivity CreateCheckpointActivity(Checkpoint checkpoint)
        {
            CheckpointActivity checkpointActivity = new CheckpointActivity();
            checkpointActivity.AppointmentId = SelectedAppointment.Id;
            checkpointActivity.CheckpointId = checkpoint.Id;
            if (checkpoint.Type.Equals(CheckpointType.START))
            {
                checkpointActivity.Activated = true;
            }
            return checkpointActivity;
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
