﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using TravelAgency.Converter;
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
        public ObservableCollection<CheckpointActivity> CheckpointActivities { get; set; }
        public ObservableCollection<CheckpointActivityDTO> CheckpointActivitiesDTO { get; set; }
        public List<Tour> UserTours { get; set; }
        public CheckpointActivityDTO SelectedCheckpointActivityDTO { get; set; }
        public CheckpointActivity SelectedCheckpointActivity { get; set; }
        private readonly CheckpointActivityRepository _checkpointActivityRepository;
        private readonly AppointmentRepository _appointmentRepository;
        private readonly CheckpointRepository _checkpointRepository;

        private void FillObservableCollection()
        {
            FindCheckpointActivitesByActiveAppointment();
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

            UserTours = tours;
            CheckpointActivities = new ObservableCollection<CheckpointActivity>();
            CheckpointActivitiesDTO = new ObservableCollection<CheckpointActivityDTO>();
            FindCheckpointActivitesByActiveAppointment();
            FillObservableCollection();
        }

        private void FindCheckpointActivitesByActiveAppointment()       
        {
            List<CheckpointActivity> checkpointActivities = _checkpointActivityRepository.GetAll();
            List<Appointment> activeAppointments = FindAllActiveAppointmentsByTours();
            CheckpointActivities.Clear();

            foreach (var checkpointActivity in checkpointActivities)
            {
                foreach (var appointment in activeAppointments)
                {
                    if (checkpointActivity.AppointmentId == appointment.Id)
                    {
                        CheckpointActivities.Add(checkpointActivity);
                    }
                }
            }
        }

        private List<Appointment> FindAllActiveAppointmentsByTours()  //Moze biti samo jedan ne treba mi lista
        {
            List<Appointment> appointments = new List<Appointment>(_appointmentRepository.GetAll());
            List<Appointment> activeAppontementsByTours = new List<Appointment>();
            foreach (Tour tour in UserTours)
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
                if(checkpoint.Type == CheckpointType.END)
                {
                    Appointment appointment = _appointmentRepository.GetById(SelectedCheckpointActivity.AppointmentId);
                    appointment.Finished = true;
                    _appointmentRepository.Update(appointment);
                }
            }
        }

        private void FinishTourButtonClick(object sender, RoutedEventArgs e)
        {
            List<CheckpointActivity> checkpointActivities = new List<CheckpointActivity>(CheckpointActivities);
            CheckpointActivity checkpointActivity = checkpointActivities[0];
            Appointment appointment = _appointmentRepository.GetById(checkpointActivity.AppointmentId);
            appointment.Finished = true;
            _appointmentRepository.Update(appointment);

            MessageBox.Show("Završili ste turu!");
            Close();
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
