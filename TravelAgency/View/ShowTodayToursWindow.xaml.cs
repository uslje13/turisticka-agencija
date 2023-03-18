using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using TravelAgency.Converter;
using TravelAgency.Model;
using TravelAgency.Repository;

namespace TravelAgency.View
{
    /// <summary>
    /// Interaction logic for ShowTodayToursWindow.xaml
    /// </summary>
    public partial class ShowTodayToursWindow : Window
    {
        public List<Tour> Tours { get; set; }
        public List<Tour> TodayTours { get; set; }
        public ObservableCollection<Appointment> Appointments { get; set; }
        public ObservableCollection<Appointment> TodayAppointments { get; set; }
        public List<Checkpoint> Checkpoints { get; set; }

        public ObservableCollection<TourDTO> TodayToursDTO { get; set; }
        private LocationConverter _locationConverter;

        private readonly AppointmentRepository _appointmentRepository;

        private readonly CheckpointRepository _checkpointRepository;

        public AppointmentRepository AppointmentRepository => _appointmentRepository;

        public CheckpointRepository CheckpointRepository => _checkpointRepository;

        public TourDTO SelectedTourDTO { get; set; }

        private void FillObservableCollection()
        {
            foreach (Tour tour in TodayTours)
            {
                string location = _locationConverter.GetFullNameById(tour.LocationId);
                TodayToursDTO.Add(new TourDTO(tour.Id, tour.Name, tour.Language, location, tour.MaxNumOfGuests, tour.Duration));
            }
        }

        public void UpdateObservableCollection()
        {
            TodayToursDTO.Clear();
            FillObservableCollection();
        }

        public ShowTodayToursWindow(ObservableCollection<Tour> tours)
        {
            InitializeComponent();
            DataContext = this;

            _appointmentRepository = new AppointmentRepository();
            _checkpointRepository = new CheckpointRepository();

            Appointments = new ObservableCollection<Appointment>(_appointmentRepository.GetAll());
            TodayToursDTO = new ObservableCollection<TourDTO>();
            Tours = new List<Tour>(tours);

            _locationConverter = new();
            TodayTours = FindTodayTours();
            FillObservableCollection();
        }

        private List<Tour> FindTodayTours()
        {
            List<Tour> todayTours = new List<Tour>();
            DateOnly today = DateOnly.FromDateTime(DateTime.Today);
            foreach (Appointment appointment in Appointments)
            {
                foreach (Tour tour in Tours)
                {
                    if (tour.Id == appointment.TourId && appointment.Date.Equals(today))
                    {
                        todayTours.Add(tour);
                    }
                }
            }
            todayTours = new List<Tour>(todayTours.Distinct());
            return todayTours;
        }

        private void PickTimeButtonClick(object sender, RoutedEventArgs e)
        {
            if (SelectedTourDTO == null)
            {
                MessageBox.Show("Morate odabrati turu!");
            }
            else
            {
                Tour selectedTour = TodayTours.Find(t => t.Id == SelectedTourDTO.TourId) ?? throw new ArgumentException();
                StartTourWindow startTourWindow = new StartTourWindow(selectedTour, Tours, AppointmentRepository, CheckpointRepository);
                startTourWindow.Owner = Window.GetWindow(this);
                startTourWindow.ShowDialog();
            }
        }
    }
}
