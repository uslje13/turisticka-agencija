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
    public partial class CreateTour : Window, INotifyPropertyChanged
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
        private readonly DateAndOccupancyRepository _dateAndOccupancyRepository;
        public ObservableCollection<Checkpoint> Checkpoints { get; set; }
        public ObservableCollection<Appointment> DatesAndOccupancies { get; set; }

        public CreateTour()
        {
            InitializeComponent();
            DataContext = this;
            _tourRepository = new TourRepository();
            _locationRepository = new LocationRepository();
            _checkpointRepository = new CheckpointRepository();
            _dateAndOccupancyRepository = new DateAndOccupancyRepository();
            Checkpoints = new ObservableCollection<Checkpoint>();
            DatesAndOccupancies = new ObservableCollection<Appointment>();
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

            Tour newTour = new Tour(TourName, _locationRepository.SaveAndGetId(location), Description, TourLanguage, MaxNumOfGuests, Duration);
            Tour savedTour = _tourRepository.Save(newTour);
            SetCheckpointsTourId(savedTour);
            _checkpointRepository.SaveAll(Checkpoints);
            SetDateAndOccupancyTourId(savedTour);
            _dateAndOccupancyRepository.SaveAll(DatesAndOccupancies);
            ToursOverview.Tours.Add(savedTour);
            Close();
        }

        private void SetCheckpointsTourId(Tour tour)
        {
            foreach (Checkpoint checkpoint in Checkpoints)
            {
                checkpoint.TourId = tour.Id;
            }
        }

        private void SetDateAndOccupancyTourId(Tour tour)
        {
            foreach (Appointment dateAndOccupancy in DatesAndOccupancies)
            {
                dateAndOccupancy.TourId = tour.Id;
            }
        }

        private void AddCheckpointButtonClick(object sender, RoutedEventArgs e)
        {
            CreateCheckpoint createCheckpoint = new CreateCheckpoint(Checkpoints);
            createCheckpoint.Owner = Window.GetWindow(this);
            createCheckpoint.Show();
        }

        private void AddDatesButtonClick(object sender, RoutedEventArgs e)
        {
            CreateTourDates createTourDates = new CreateTourDates(DatesAndOccupancies);
            createTourDates.Owner = Window.GetWindow(this);
            createTourDates.Show();
        }
    }
}
