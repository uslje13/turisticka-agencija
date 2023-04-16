using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;

namespace SOSTeam.TravelAgency.WPF.ViewModels.TourGuide
{
    public class AddAppointmentsViewModel : ViewModel
    {
        private DateTime _date;

        public DateTime Date
        {
            get => _date;
            set
            {
                if (_date != value)
                {
                    _date = value;
                    OnPropertyChanged("Date");
                }
            }
        }

        private DateTime _time;

        public DateTime Time
        {
            get => _time;
            set
            {
                if (_time != value)
                {
                    _time = value;
                    OnPropertyChanged("Time");
                }
            }
        }

        private ObservableCollection<Appointment> _appointments;

        public ObservableCollection<Appointment> Appointments
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
            Date = DateTime.Now;
        }

        private bool CanExecuteMethod(object parameter)
        {
            return true;
        }

        public void AddAppointment(object sender)
        {
            Appointment appointment = new Appointment();
            appointment.Date = DateOnly.FromDateTime(Date);
            appointment.Time = TimeOnly.FromDateTime(Time);
            appointment.Occupancy = 0;
            appointment.Started = false;
            appointment.Finished = false;
            Appointments.Add(appointment);
        }

        public void DeleteAppointment(object sender)
        {
            var selectedAppointment = sender as Appointment;
            Appointments.Remove(selectedAppointment);
        }

        public void DeleteAllAppointments(object sender)
        {
            Appointments.Clear();
        }

    }
}
