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

        public ShowTodayToursWindow(List<Tour> tours)
        {
            InitializeComponent();
            DataContext = this;

            _appointmentRepository = new AppointmentRepository();
            _checkpointRepository = new CheckpointRepository();

            Tours = tours;
            Appointments = new ObservableCollection<Appointment>(_appointmentRepository.GetAppointmentsByTours(tours));
            TodayToursDTO = new ObservableCollection<TourDTO>();
            

            _locationConverter = new();
            TodayTours = GetTodayTours();
            FillObservableCollection();
        }

        private List<Tour> GetTodayTours()
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Today);
            
            List<Tour> tours = new List<Tour>();
            List<Appointment> todayAppointments = _appointmentRepository.GetTodayAppointments();

            foreach (Appointment appointment in todayAppointments)
            {
                foreach (Tour tour in Tours)
                {
                    if (tour.Id == appointment.TourId)
                    {
                        tours.Add(tour);
                    }
                }
            }
            tours = new List<Tour>(tours.Distinct());
            return tours;
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
                StartTourWindow startTourWindow = new StartTourWindow(selectedTour, Tours);
                startTourWindow.Owner = Window.GetWindow(this);
                startTourWindow.ShowDialog();
            }
        }
    }
}
