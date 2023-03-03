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
        private int _checkpointId;   //checkpointId ---> moraci biti lista cekpointa
        private int _dateTimeStartId;    //date and time when tour start 
        private int _duration;

        private string _city;
        private string _country;

        private readonly TourRepository _tourRepository;
        private readonly LocationRepository _locationRepository;

        public CreateTour()
        {
            InitializeComponent();
        }

        public string Name
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
        public string Language
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

        private void ViewDescriptionButtonClick(object sender, RoutedEventArgs e)
        {
            DescriptionTour descriptionTour = new DescriptionTour();
            descriptionTour.Show();
        }


        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
            Location location = new Location(City, Country);
            Tour tour = new Tour(Name, _locationRepository.SaveAndGetId(location), "opis", Language, MaxNumOfGuests, CheckpointId, DateTimeStartId, Duration);
        }
    }
}
