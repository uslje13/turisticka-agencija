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
        private int _dateTimeStartId;    //date and time when tour start 
        private int _duration;

        private string _city;
        private string _country;

        private readonly TourRepository _tourRepository;
        private readonly LocationRepository _locationRepository;

        public CreateTour()
        {
            InitializeComponent();
            DataContext = this;
            _tourRepository = new TourRepository();
            _locationRepository = new LocationRepository();
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
        public int DateTimeStartId
        {
            get => _dateTimeStartId;
            set
            {
                if (value != _dateTimeStartId)
                {
                    _dateTimeStartId = value;
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
            Tour newTour = new Tour(TourName, _locationRepository.SaveAndGetId(location), Description, TourLanguage, MaxNumOfGuests, -1, -1, Duration);
            Tour savedTour = _tourRepository.Save(newTour);
            ToursOverview.Tours.Add(savedTour);
            Close();
        }

        private void AddCheckpointButtonClick(object sender, RoutedEventArgs e)
        {
            CreateCheckpoint createCheckpoint = new CreateCheckpoint();
            createCheckpoint.Show();
        }
    }
}
