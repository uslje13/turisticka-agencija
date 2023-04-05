using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Repositories;

namespace SOSTeam.TravelAgency.WPF.Views
{
    /// <summary>
    /// Interaction logic for CreateTourWindow.xaml
    /// </summary>
    public partial class CreateTourWindow : Window
    {
        public List<Tour> Tours { get; set; }
        public ObservableCollection<Checkpoint> Checkpoints { get; set; }
        public ObservableCollection<Appointment> Appointments { get; set; }
        public ObservableCollection<Image> Images { get; set; }
        
        public List<Location> Locations { get; set; }
        public ReadOnlyObservableCollection<string> Countries { get; set; }

        public ObservableCollection<string> Cities { get; set; }

        public User LoggedInUser { get; set; }

        private readonly TourRepository _tourRepository;

        private readonly LocationRepository _locationRepository;

        private readonly CheckpointRepository _checkpointRepository;

        private readonly AppointmentRepository _appointmentRepository;

        private readonly ImageRepository _imageRepository;

        private string _name;
        public string TourName
        {
            get => _name;
            set
            {
                if (!value.Equals(_name))
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _description;
        public string Description
        {
            get => _description;
            set
            {
                if (!value.Equals(_description))
                {
                    _description = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _language;
        public string TourLanguage
        {
            get => _language;
            set
            {
                if (!value.Equals(_language))
                {
                    _language = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _maxNumOfGuests;
        public int MaxNumOfGuests
        {
            get => _maxNumOfGuests;
            set
            {
                if (value != _maxNumOfGuests)
                {
                    _maxNumOfGuests = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _duration;
        public int Duration
        {
            get => _duration;
            set
            {
                if (value != _duration)
                {
                    _duration = value;
                }
            }
        }

        private string _city;
        public string City
        {
            get => _city;
            set
            {
                if (!value.Equals(_city))
                {
                    _city = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _country;
        public string Country
        {
            get => _country;
            set
            {
                if (!value.Equals(_country))
                {
                    _country = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public CreateTourWindow(User user, List<Tour> tours)
        {
            InitializeComponent();
            DataContext = this;
            LoggedInUser = user;
            Tours = tours;

            _tourRepository = new TourRepository();
            _locationRepository = new LocationRepository();
            _checkpointRepository = new CheckpointRepository();
            _appointmentRepository = new AppointmentRepository();
            _imageRepository = new ImageRepository();

            Locations = new List<Location>(_locationRepository.GetAll());
            Countries = new ReadOnlyObservableCollection<string>(GetCountries());
            Cities = new ObservableCollection<string>();

            Checkpoints = new ObservableCollection<Checkpoint>();
            Appointments = new ObservableCollection<Appointment>();
            Images = new ObservableCollection<Image>();
        }

        public ObservableCollection<string> GetCountries()
        {
            ObservableCollection<string> coutries = new ObservableCollection<string>();
            foreach (Location location in Locations)
            {
                coutries.Add(location.Country);
            }
            coutries = new ObservableCollection<string>(coutries.Distinct());
            return coutries;
        }

        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
            if (IsValid())
            {
                int locationId = FindLocationId();
                int tourId = _tourRepository.NextId();
                Tour newTour = new Tour(tourId, TourName, locationId, Description, TourLanguage, MaxNumOfGuests, Duration, LoggedInUser.Id);
                _tourRepository.Save(newTour);
                SetEntitesTourId(newTour);
                SaveEntites();
                Tours.Add(newTour);

                Close();
            }
            else
            {
                MessageBox.Show("Uneti podaci nisu ispravni!");
            }
        }

        private int FindLocationId()
        {
            List <Location> locations = new List<Location>(_locationRepository.GetAll());
            Location location = locations.Find(l => l.Country.Equals(Country) && l.City.Equals(City));
            return location.Id;
        }

        private bool IsValid()
        {
            if (Checkpoints.Count < 2 || Appointments.Count < 1 || Images.Count < 1)
            {
                return false;
            }
            return true;
        }

        private void SetEntitesTourId(Tour savedTour)
        {
            SetCheckpointsTourId(savedTour);
            SetAppointmentsTourId(savedTour);
            SetImagesTourId(savedTour);
        }

        private void SetCheckpointsTourId(Tour tour)
        {
            foreach (Checkpoint checkpoint in Checkpoints)
            {
                checkpoint.TourId = tour.Id;
            }
        }

        private void SetAppointmentsTourId(Tour tour)
        {
            foreach (Appointment appointment in Appointments)
            {
                appointment.TourId = tour.Id;
            }
        }

        private void SetImagesTourId(Tour tour)
        {
            foreach (Image image in Images)
            {
                image.EntityId = tour.Id;
            }
        }

        private void SaveEntites()
        {
            _checkpointRepository.SaveAll(new List<Checkpoint>());
            _appointmentRepository.SaveAll(new List<Appointment>());
            _imageRepository.SaveAll(new List<Image>(Images));
        }

        private void AddCheckpointButtonClick(object sender, RoutedEventArgs e)
        {
            AddCheckpointsWindow addCheckpointsWindow = new AddCheckpointsWindow(Checkpoints);
            addCheckpointsWindow.Owner = Window.GetWindow(this);
            addCheckpointsWindow.ShowDialog();
        }

        private void AddDatesButtonClick(object sender, RoutedEventArgs e)
        {
            AddAppointmentsWindow addApointemntsWindow = new AddAppointmentsWindow(Appointments);
            addApointemntsWindow.Owner = Window.GetWindow(this);
            addApointemntsWindow.ShowDialog();
        }

        private void AddImagesButtonClick(object sender, RoutedEventArgs e)
        {
            AddImagesWindow addImagesWindow = new AddImagesWindow(Images,Image.ImageType.TOUR);
            addImagesWindow.Owner = Window.GetWindow(this);
            addImagesWindow.ShowDialog();
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void CountryComboBoxSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            countryComboBox.IsEnabled = false;
            Cities.Clear();
            GetCities();
        }

        public void GetCities()
        {
            foreach (Location location in Locations)
            {
                if (location.Country.Equals(Country))
                {
                    Cities.Add(location.City);
                }
            }
        }
    }
}
