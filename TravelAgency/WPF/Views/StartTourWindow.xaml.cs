using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Repositories;

namespace SOSTeam.TravelAgency.WPF.Views
{
    /// <summary>
    /// Interaction logic for StartTourWindow.xaml
    /// </summary>
    public partial class StartTourWindow : Window
    {
        public ObservableCollection<Appointment> TodayAppointments { get; set; }
        public Appointment SelectedAppointment { get; set; }

        private readonly AppointmentRepository _appointmentRepository;

        private readonly CheckpointRepository _checkpointRepository;

        private readonly CheckpointActivityRepository _checkpointActivityRepository;

        public List<Tour> Tours { get; set; }
        public Tour SelectedTour { get; set; }

        public StartTourWindow(Tour selectedTour, List<Tour> tours)
        {
            InitializeComponent();
            DataContext = this;

            _appointmentRepository = new AppointmentRepository();
            _checkpointRepository = new CheckpointRepository();
            _checkpointActivityRepository = new CheckpointActivityRepository();

            Tours = tours;
            SelectedTour = selectedTour;
            selectedTourLabel.Content = "Tura: " + SelectedTour.Name;
            TodayAppointments = new ObservableCollection<Appointment>(FindTodayAppointmentsByTour());
            ExistsActiveAppointment();
        }

        private void ExistsActiveAppointment()
        {
            List<Appointment> appointments = _appointmentRepository.GetAllByTours(Tours);
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

        private List<Appointment> FindTodayAppointmentsByTour()
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
                SaveCheckpointsActivity();
                MessageBox.Show("Tura je uspešno startovana!");
                DisableStartTour();
                Close();
                ShowTourCheckpointsWindow showTourCheckpoints = new ShowTourCheckpointsWindow(Tours);
                showTourCheckpoints.ShowDialog();
            }
        }
        private void ActivateAppointment()
        {
            SelectedAppointment.Started = true;
            _appointmentRepository.Update(SelectedAppointment);
        }

        private void SaveCheckpointsActivity()
        {
            List<Checkpoint> checkpoints = _checkpointRepository.GetAllByTourId(SelectedTour.Id);
            List<CheckpointActivity> activities = new List<CheckpointActivity>();
            foreach (Checkpoint checkpoint in checkpoints)
            {
                activities.Add(CreateCheckpointActivity(checkpoint));
            }
            _checkpointActivityRepository.SaveAll(activities);
        }

        private CheckpointActivity CreateCheckpointActivity(Checkpoint checkpoint)
        {
            CheckpointActivity activity = new CheckpointActivity();
            activity.AppointmentId = SelectedAppointment.Id;
            activity.CheckpointId = checkpoint.Id;

            if (checkpoint.Type.Equals(CheckpointType.START))
            {
                activity.Status = CheckpointStatus.ACTIVE;
            }

            return activity;
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
