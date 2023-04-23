using System;
using System.Collections.ObjectModel;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;

namespace SOSTeam.TravelAgency.WPF.ViewModels.TourGuide
{
    public class AddAppointmentsViewModel : ViewModel
    {
        private DateTime _start;

        public DateTime Start
        {
            get => _start;
            set
            {
                if (_start != value)
                {
                    _start = value;
                    OnPropertyChanged("Start");
                }
            }
        }

        private ObservableCollection<Appointment>? _appointments;

        public ObservableCollection<Appointment>? Appointments
        {
            get => _appointments;
            set
            {
                if (_appointments != value)
                {
                    _appointments = value;
                    OnPropertyChanged("Appointments");
                }
            }
        }

        public RelayCommand AddAppointmentCommand { get; set; }
        public RelayCommand DeleteAppointmentCommand { get; set; }
        public RelayCommand ClearAppointmentsCommand { get; set; }

        public AddAppointmentsViewModel(ObservableCollection<Appointment> appointments)
        {
            Appointments = appointments;
            AddAppointmentCommand = new RelayCommand(AddAppointment, CanExecuteMethod);
            DeleteAppointmentCommand = new RelayCommand(DeleteAppointment, CanExecuteMethod);
            ClearAppointmentsCommand = new RelayCommand(DeleteAllAppointments, CanExecuteMethod);
            Start = DateTime.Now;
        }

        private bool CanExecuteMethod(object parameter)
        {
            return true;
        }

        public void AddAppointment(object sender)
        {
            if (Appointments == null) { return; }
            var appointment = new Appointment
            {
                Start = DateTime.Now,
                Occupancy = 0,
                Started = false,
                Finished = false
            };
            Appointments.Add(appointment);
        }

        public void DeleteAppointment(object sender)
        {
            if (Appointments == null) { return; }

            Appointment? selectedAppointment = sender as Appointment;
            if (selectedAppointment == null) { return; }
            Appointments.Remove(selectedAppointment);
        }

        public void DeleteAllAppointments(object sender)
        {
            if (Appointments == null) { return; }
            if (Appointments.Count == 0) {  return; }
            Appointments.Clear();
        }

    }
}
