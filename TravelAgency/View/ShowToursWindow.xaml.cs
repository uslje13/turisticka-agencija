using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using TravelAgency.Converter;
using TravelAgency.Model;
using TravelAgency.Repository;

namespace TravelAgency.View
{
    /// <summary>
    /// Interaction logic for TourOverview.xaml
    /// </summary>
    public partial class ShowToursWindow : Window
    {
        private ObservableCollection<Tour> _tours;

        public ObservableCollection<Tour> Tours
        {
            get => _tours;
            set
            {
                if (!value.Equals(_tours))
                {
                    _tours = value;
                    OnPropertyChanged();
                }
            }
        }
        public ObservableCollection<TourDTO> ToursDTO { get; set; }
        public List<Appointment> Appointments { get; set; }
        public User LoggedInUser { get; set; }

        public Tour SelectedTour { get; set; }

        private readonly TourRepository _tourReository;

        private readonly AppointmentRepository _appointmentReository;

        private LocationConverter _locationConverter;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void FillObservableCollection()
        {
            foreach(Tour tour in _tourReository.GetByUser(LoggedInUser))
            {
                string location = _locationConverter.GetFullNameById(tour.LocationId);
                ToursDTO.Add(new TourDTO(tour.Id, tour.Name, tour.Language, location, tour.MaxNumOfGuests, tour.Duration));
            }
        }

        public void UpdateObservableCollection()
        {
            ToursDTO.Clear();
            FillObservableCollection();
        }


        public ShowToursWindow(User user)
        {
            InitializeComponent();
            DataContext = this;
            LoggedInUser = user;
            deleteButton.IsEnabled = false;
            _locationConverter = new();

            _tourReository = new TourRepository();
            _appointmentReository = new AppointmentRepository();

            ToursDTO = new ObservableCollection<TourDTO>();
            FillObservableCollection();

            Tours = new ObservableCollection<Tour>(_tourReository.GetByUser(user));
            Appointments = new List<Appointment>(_appointmentReository.GetAll());

            AppointmentRegularEnd();
        }

        private void AppointmentRegularEnd()
        {
            foreach (Tour tour in Tours)
            {
                foreach (Appointment appointment in Appointments)
                {
                    if (appointment.TourId == tour.Id && appointment.Started == true)
                    {
                        CheckAppointmentEnd(tour, appointment);
                    }
                }
            }
        }

        private void CheckAppointmentEnd(Tour tour, Appointment appointment)
        {
            DateTime dateStart = appointment.Date.ToDateTime(appointment.Time);

            (int durationInDays, int durationInHours) = ConvertDuration(tour.Duration);

            DateTime endDate = dateStart.AddDays(durationInDays).AddHours(durationInHours);

            if (endDate.CompareTo(DateTime.Now) < 0)
            {
                appointment.Finished = true;
                _appointmentReository.Update(appointment);
            }
        }

        private (int, int) ConvertDuration(int duration)
        {
            int days = duration / 24;
            int hours = duration % 24;

            return (days, hours);
        }

        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
            CreateTourWindow createTourWindow = new CreateTourWindow(LoggedInUser, Tours);
            createTourWindow.Owner = Window.GetWindow(this);
            createTourWindow.ShowDialog();
            UpdateObservableCollection();
        }

        private void TodayToursButtonClick(object sender, RoutedEventArgs e)
        {
            ShowTodayToursWindow showTodayToursWindow = new ShowTodayToursWindow(Tours);
            showTodayToursWindow.Owner = Window.GetWindow(this);
            showTodayToursWindow.ShowDialog();
        }

        private void ActiveTourButtonClick(object sender, RoutedEventArgs e)
        {
            ShowTourCheckpointsWindow showTourCheckpoints = new ShowTourCheckpointsWindow(new List<Tour>(Tours));
            showTourCheckpoints.ShowDialog();
        }

    }
}
