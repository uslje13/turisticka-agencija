using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using SOSTeam.TravelAgency.Domain.Models;

namespace SOSTeam.TravelAgency.WPF.Views
{
    /// <summary>
    /// Interaction logic for AddAppointments.xaml
    /// </summary>
    public partial class AddAppointmentsWindow : Window
    {
        public ReadOnlyObservableCollection<string> Hours { get; set; }
        public ReadOnlyObservableCollection<string> Minutes { get; set; }

        private ObservableCollection<Appointment> _appointments;
        public ObservableCollection<Appointment> Appointments
        {
            get => _appointments;
            set
            {
                if (value != _appointments)
                {
                    _appointments = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateTime _start;
        public DateTime Start
        {
            get => _start;
            set
            {
                if (value != _start)
                {
                    _start = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _hour;
        public string Hour
        {
            get => _hour;
            set
            {
                if (!value.Equals(_hour))
                {
                    _hour = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _minute;
        public string Minute
        {
            get => _minute;
            set
            {
                if (!value.Equals(_minute))
                {
                    _minute = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public AddAppointmentsWindow(ObservableCollection<Appointment> appointments)
        {
            InitializeComponent();
            DataContext = this;
            Start = DateTime.UtcNow;

            Hours = new ReadOnlyObservableCollection<string>(CreateHoursList());
            Minutes = new ReadOnlyObservableCollection<string>(CreateMinutesList());

            _appointments = appointments;
        }

        private ObservableCollection<string> CreateHoursList()
        {
            ObservableCollection<string> hours = new ObservableCollection<string>();
            for (int i = 0; i < 24; i++)
            {
                if (i < 10)
                {
                    hours.Add($"0{i}");
                }
                else
                {
                    hours.Add($"{i}");
                }
            }
            return hours;
        }
        private ObservableCollection<string> CreateMinutesList()
        {
            ObservableCollection<string> minutes = new ObservableCollection<string>();
            for (int i = 0; i < 60; i += 15)
            {
                if (i == 0)
                {
                    minutes.Add($"0{i}");
                }
                else
                {
                    minutes.Add($"{i}");
                }
            }
            return minutes;
        }

        private void AddDateAndTimeButtonClick(object sender, RoutedEventArgs e)
        {
            Appointment appointment = new Appointment();
            appointment.Date = DateOnly.FromDateTime(Start);
            appointment.Time = new TimeOnly(int.Parse(Hour), int.Parse(Minute));
            appointment.Occupancy = 0;

            Appointments.Add(appointment);
        }


        private void ConfirmButtonClick(object sender, RoutedEventArgs e)
        {
            if (Appointments.Count < 1)
            {
                MessageBox.Show("Ne možete kreirati turu!\nMorate uneti makar jedan datum.");
            }
            else
            {
                Close();
            }
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            Appointments.Clear();
            Close();
        }
    }
}
