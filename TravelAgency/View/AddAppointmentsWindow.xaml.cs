﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using TravelAgency.Model;

namespace TravelAgency.View
{
    /// <summary>
    /// Interaction logic for AddAppointments.xaml
    /// </summary>
    public partial class AddAppointmentsWindow : Window
    {
        private DateTime _start;
        private string _hour;
        private string _minute;
        private ObservableCollection<Appointment> _appointments;
        public ReadOnlyObservableCollection<string> HoursList { get; set; }
        public ReadOnlyObservableCollection<string> MinutesList { get; set; }

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

        public AddAppointmentsWindow(ObservableCollection<Appointment> appointments)
        {
            InitializeComponent();
            DataContext = this;
            Start = DateTime.UtcNow;
            HoursList = new ReadOnlyObservableCollection<string>(CreateHoursList());
            MinutesList = new ReadOnlyObservableCollection<string>(CreateMinutesList());
            CreateHoursList();
            CreateMinutesList();
            _appointments = appointments;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ObservableCollection<string> CreateHoursList()
        {
            ObservableCollection<string> tempList = new ObservableCollection<string>();
            for (int i = 0; i < 24; i++)
            {
                if (i < 10)
                {
                    tempList.Add($"0{i}");
                }
                else
                {
                    tempList.Add($"{i}");
                }
            }
            return tempList;
        }
        private ObservableCollection<string> CreateMinutesList()
        {
            ObservableCollection<string> tempList = new ObservableCollection<string>();
            for (int i = 0; i < 60; i += 15)
            {
                if (i == 0)
                {
                    tempList.Add($"0{i}");
                }
                else
                {
                    tempList.Add($"{i}");
                }
            }
            return tempList;
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
