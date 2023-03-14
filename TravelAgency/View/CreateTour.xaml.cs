using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using TravelAgency.Model;
using TravelAgency.Repository;

namespace TravelAgency.View
{
    /// <summary>
    /// Interaction logic for CreateTour.xaml
    /// </summary>
    public partial class CreateTour : Window
    {
        private int _id;        //tour _id
        private string _name;
        //private int _locationId;     //locationId
        private string _description;
        private string _language;
        private int _maxNumOfGuests;
        private int _checkpointId;   //checkpointId
        private int _duration;

        private string _city;
        private string _country;

        private readonly TourRepository _tourRepository;
        private readonly LocationRepository _locationRepository;
        private readonly CheckpointRepository _checkpointRepository;
        private readonly AppointmentRepository _appointmentRepository;
        private readonly ImageRepository _imageRepository;
        public ObservableCollection<Checkpoint> Checkpoints { get; set; }
        public ObservableCollection<Appointment> Appointments { get; set; }
        public ObservableCollection<Image> Images { get; set; }
        public User LoggedInUser { get; set; }

        public CreateTour(User user)
        {
            InitializeComponent();
            DataContext = this;
            LoggedInUser = user;
            _tourRepository = new TourRepository();
            _locationRepository = new LocationRepository();
            _checkpointRepository = new CheckpointRepository();
            _appointmentRepository = new AppointmentRepository();
            _imageRepository = new ImageRepository();
            Checkpoints = new ObservableCollection<Checkpoint>();
            Appointments = new ObservableCollection<Appointment>();
            Images = new ObservableCollection<Image>();
        }

        public string TourName      //wpf framework already have Name
        {
            get => _name;
            set
            {
                if (value != _name)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }
        public string Description
        {
            get => _description;
            set
            {
                if (value != _description)
                {
                    _description = value;
                    OnPropertyChanged();
                }
            }
        }
        public string TourLanguage  //wpf framework already have Language
        {
            get => _language;
            set
            {
                if (value != _language)
                {
                    _language = value;
                    OnPropertyChanged();
                }
            }
        }
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
        public int CheckpointId
        {
            get => _checkpointId;
            set
            {
                if (value != _checkpointId)
                {
                    _checkpointId = value;
                    OnPropertyChanged();
                }
            }
        }
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

        public string City
        {
            get => _city;
            set
            {
                if (value != _city)
                {
                    _city = value;
                    OnPropertyChanged();
                }
            }
        }
        public string Country
        {
            get => _country;
            set
            {
                if (value != _country)
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

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }


        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
            Location location = new Location(Country, City);
            Tour newTour = new Tour(TourName, _locationRepository.SaveAndGetId(location), Description, TourLanguage, MaxNumOfGuests, Duration, LoggedInUser.Id);
            Tour savedTour = _tourRepository.Save(newTour);
            SetCheckpointsTourId(savedTour);
            _checkpointRepository.SaveAll(Checkpoints);
            SetAppointmentsTourId(savedTour);
            _appointmentRepository.SaveAll(Appointments);
            SetImagesTourId(savedTour);
            _imageRepository.SaveAll(Images);
            ShowToursWindow.Tours.Add(savedTour);

            Close();
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
            foreach(Image image in Images)
            {
                image.TourId = tour.Id;
            }
        }

        private void AddCheckpointButtonClick(object sender, RoutedEventArgs e)
        {
            AddCheckpointsWindow addCheckpointsWindow = new AddCheckpointsWindow(Checkpoints);
            addCheckpointsWindow.Owner = Window.GetWindow(this);
            addCheckpointsWindow.Show();
        }

        private void AddDatesButtonClick(object sender, RoutedEventArgs e)
        {
            AddAppointmentsWindow addApointemntsWindow = new AddAppointmentsWindow(Appointments);
            addApointemntsWindow.Owner = Window.GetWindow(this);
            addApointemntsWindow.Show();
        }

        private void AddImagesButtonClick(object sender, RoutedEventArgs e)
        {
            AddImagesWindow addImagesWindow = new AddImagesWindow(Images);
            addImagesWindow.Owner = Window.GetWindow(this);
            addImagesWindow.Show();
        }
    }
}
