using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using TravelAgency.DTO;
using TravelAgency.Model;
using TravelAgency.Repository;

namespace TravelAgency.View
{
    /// <summary>
    /// Interaction logic for ShowTourCheckpoints.xaml
    /// </summary>
    public partial class ShowTourCheckpointsWindow : Window
    {
        public List<CheckpointActivity> CheckpointActivities { get; set; }
        public ObservableCollection<CheckpointActivityDTO> CheckpointActivitiesDTO { get; set; }
        public List<Tour> Tours { get; set; }
        public CheckpointActivityDTO SelectedCheckpointActivityDTO { get; set; }
        public CheckpointActivity SelectedCheckpointActivity { get; set; }
        private readonly CheckpointActivityRepository _checkpointActivityRepository;
        private readonly AppointmentRepository _appointmentRepository;
        private readonly CheckpointRepository _checkpointRepository;

        private void FillObservableCollection()
        {
            FindCheckpointActivites();
            foreach (CheckpointActivity activity in CheckpointActivities)
            {
                Checkpoint checkpoint = _checkpointRepository.GetById(activity.CheckpointId);
                CheckpointActivitiesDTO.Add(new CheckpointActivityDTO(activity.Id, checkpoint.Name, checkpoint.Type, activity.Activated, checkpoint.Id));
            }
        }

        public void UpdateObservableCollection()
        {
            CheckpointActivitiesDTO.Clear();
            FillObservableCollection();
        }


        public ShowTourCheckpointsWindow(List<Tour> tours)
        {
            InitializeComponent();
            DataContext = this;

            _checkpointActivityRepository = new CheckpointActivityRepository();
            _appointmentRepository = new AppointmentRepository();
            _checkpointRepository = new CheckpointRepository();

            Tours = tours;
            CheckpointActivities = new List<CheckpointActivity>();
            CheckpointActivitiesDTO = new ObservableCollection<CheckpointActivityDTO>();
            FindCheckpointActivites();
            FillObservableCollection();
        }

        private void FindCheckpointActivites()
        {
            List<CheckpointActivity> activities = _checkpointActivityRepository.GetAll();
            Appointment activeAppointment = FindActiveAppointment();
            CheckpointActivities.Clear();

            if (activeAppointment != null)
            {
                foreach (CheckpointActivity activity in activities)
                {
                    if (activity.AppointmentId == activeAppointment.Id)
                    {
                        CheckpointActivities.Add(activity);
                    }

                }
            }
        }

        private Appointment FindActiveAppointment()
        {
            List<Appointment> appointments = _appointmentRepository.GetAppointmentsByTours(Tours);
            return appointments.Find(a => a.Started == true && a.Finished == false);

            /*
            List<Appointment> appointments = new List<Appointment>(_appointmentRepository.GetAll());
            List<Appointment> activeAppontementsByTours = new List<Appointment>();
            foreach (Tour tour in Tours)
            {
                foreach (Appointment appointment in appointments)
                {
                    bool active = appointment.Started == true && appointment.Finished == false;
                    if (appointment.TourId == tour.Id && active)
                    {
                        activeAppontementsByTours.Add(appointment);
                    }
                }
            }
            return activeAppontementsByTours;
            */
        }

        private void SetSelectedCheckpointActivity()
        {
            SelectedCheckpointActivity = _checkpointActivityRepository.GetById(SelectedCheckpointActivityDTO.Id);
        }

        private void ActivateCheckpointButtonClick(object sender, RoutedEventArgs e)
        {
            SetSelectedCheckpointActivity();
            if (SelectedCheckpointActivity.Activated == false)
            {
                SelectedCheckpointActivity.Activated = true;
                _checkpointActivityRepository.Update(SelectedCheckpointActivity);
                UpdateObservableCollection();
                MessageBox.Show("Uspešno ste aktivirali čekpoint!");

                Checkpoint checkpoint = _checkpointRepository.GetById(SelectedCheckpointActivity.CheckpointId);
                if (checkpoint.Type == CheckpointType.END)
                {
                    Appointment appointment = _appointmentRepository.GetById(SelectedCheckpointActivity.AppointmentId);
                    appointment.Finished = true;
                    _appointmentRepository.Update(appointment);
                }
            }
        }

        private void FinishTourButtonClick(object sender, RoutedEventArgs e)
        {
            Appointment activeAppointment = FindActiveAppointment();
            if (activeAppointment != null)
            {
                activeAppointment.Finished = true;
                _appointmentRepository.Update(activeAppointment);
                MessageBox.Show("Završili ste turu!");
                Close();
            }
            

            /*
            List<CheckpointActivity> checkpointActivities = new List<CheckpointActivity>(CheckpointActivities);
            CheckpointActivity checkpointActivity = checkpointActivities[0];
            Appointment appointment = _appointmentRepository.GetById(checkpointActivity.AppointmentId);
            appointment.Finished = true;
            _appointmentRepository.Update(appointment);
            */
        }

        private void GuestsAttendanceButtonClick(object sender, RoutedEventArgs e)
        {
            if (SelectedCheckpointActivityDTO == null || SelectedCheckpointActivityDTO.Activated == false)
            {
                MessageBox.Show("Morate izabrati AKTIVNU ključnu tačku!");
            }
            else
            {
                SetSelectedCheckpointActivity();
                ShowGuestsAttendanceWindow showGuestsAttendanceWindow = new ShowGuestsAttendanceWindow(SelectedCheckpointActivity);
                showGuestsAttendanceWindow.ShowDialog();
            }
        }
    }
}
