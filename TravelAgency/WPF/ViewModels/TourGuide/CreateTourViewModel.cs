using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.Views.TourGuide;
using Image = SOSTeam.TravelAgency.Domain.Models.Image;



namespace SOSTeam.TravelAgency.WPF.ViewModels.TourGuide
{
    public class CreateTourViewModel : ViewModel
    {
        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged("Name");
                }
            }
        }
        private string _city;
        public string City
        {
            get => _city;
            set
            {
                if (_city != value)
                {
                    _city = value;
                    OnPropertyChanged("City");
                }
            }
        }
        private string _country;
        public string Country
        {
            get => _country;
            set
            {
                if (_country != value)
                {
                    _country = value;
                    OnPropertyChanged("Country");
                }
            }
        }
        private string _description;
        public string Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged("Description");
                }
            }
        }
        private string _language;
        public string Language
        {
            get => _language;
            set
            {
                if (_language != value)
                {
                    _language = value;
                    OnPropertyChanged("Language");
                }
            }
        }
        private int _maxNumOfGuests;
        public int MaxNumOfGuests
        {
            get => _maxNumOfGuests;
            set
            {
                if (_maxNumOfGuests != value)
                {
                    _maxNumOfGuests = value;
                    OnPropertyChanged("MaxNumOfGuests");
                }
            }
        }
        private int _duration;
        public int Duration
        {
            get => _duration;

            set
            {
                if (_duration != value)
                {
                    _duration = value;
                    OnPropertyChanged("Duration");
                }
            }
        }
        private readonly LocationService _locationService;
        private readonly TourService _tourService;
        private readonly CheckpointService _checkpointService;
        private readonly AppointmentService _appointmentService;
        private readonly ImageService _imageService;

        public ObservableCollection<string> Countries { get; set; }

        public ObservableCollection<string> Cities { get; set; }

        public User LoggedUser { get; set; }

        public List<Location> Locations { get; set; }
        public List<string> ImagePaths { get; set; }

        public ObservableCollection<Checkpoint> Checkpoints { get; set; }
        public ObservableCollection<Appointment> Appointments { get; set; }

        public List<Image> Images { get; set; }

        public RelayCommand CitySelectionChangedCommand { get; set; }
        public RelayCommand CountrySelectionChangedCommand { get; set; }

        public RelayCommand ShowAddCheckpointsPageCommand { get; set; }
        public RelayCommand ShowAddAppointmentsPageCommand { get; set; }
        public RelayCommand ShowAddImagesCommand { get; set; }
        public RelayCommand ShowGalleryCommand { get; set; }
        public RelayCommand CreateTourClickCommand { get; set; }

        public CreateTourViewModel(User loggedUser)
        {
            _locationService = new LocationService();
            _tourService = new TourService();
            _checkpointService = new CheckpointService();
            _appointmentService = new AppointmentService();
            _imageService = new ImageService();
            LoggedUser = loggedUser;


            Locations = _locationService.GetAll();
            Countries = GetCountries();
            Cities = GetCities();

            Checkpoints = new ObservableCollection<Checkpoint>();
            Appointments = new ObservableCollection<Appointment>();
            ImagePaths = new List<string>();
            Images = new List<Image>();

            CountrySelectionChangedCommand = new RelayCommand(ExecuteCountrySelectionChanged, CanExecuteMethod);
            CitySelectionChangedCommand = new RelayCommand(ExecuteCitySelectionChanged, CanExecuteMethod);
            ShowAddCheckpointsPageCommand = new RelayCommand(ShowAddCheckpointsPage, CanExecuteMethod);
            ShowAddAppointmentsPageCommand = new RelayCommand(ShowAddAppointmentsPage, CanExecuteMethod);
            ShowAddImagesCommand = new RelayCommand(SelectImagesPaths, CanExecuteMethod);
            ShowGalleryCommand = new RelayCommand(ShowImageGallery, CanExecuteMethod);
            CreateTourClickCommand = new RelayCommand(CreateTour, CanExecuteMethod);
        }
        private bool CanExecuteMethod(object parameter)
        {
            return true;
        }

        public ObservableCollection<string> GetCountries()
        {
            ObservableCollection<string> countries = new ObservableCollection<string>();
            foreach (Location location in Locations)
            {
                countries.Add(location.Country);
            }
            countries = new ObservableCollection<string>(countries.Distinct());
            return countries;
        }

        public ObservableCollection<string> GetCities()
        {
            ObservableCollection<string> citites = new ObservableCollection<string>();
            foreach (Location location in Locations)
            {
                citites.Add(location.City);
            }
            return citites;
        }

        public void ExecuteCountrySelectionChanged(object parameter)
        {
            CountrySelectionChanged(parameter, null);
        }

        public void ExecuteCitySelectionChanged(object parameter)
        {
            CitySelectionChanged(parameter, null);
        }

        public void CountrySelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Cities.Clear();
           
            var filteredLocations = Locations.Where(location => location.Country == Country).ToList();

            foreach (var location in filteredLocations)
            {
                Cities.Add(location.City);
            }
        }

        public void CitySelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedLocation = Locations.Find(location => location.City == City);

            if (selectedLocation != null)
            {
                Country = selectedLocation.Country;
                City = selectedLocation.City;
            }
        }

        private void ShowAddCheckpointsPage(object sender)
        {
            AddCheckpointsPage addCheckpointsPage = new AddCheckpointsPage(Checkpoints);
            System.Windows.Application.Current.Windows.OfType<MainWindow>().FirstOrDefault().ToursOverviewFrame.Content = addCheckpointsPage;
        }

        private void ShowAddAppointmentsPage(object sender)
        {
            AddAppointmentsPage addAppointmentsPage = new AddAppointmentsPage(Appointments);
            System.Windows.Application.Current.Windows.OfType<MainWindow>().FirstOrDefault().ToursOverviewFrame.Content = addAppointmentsPage;
        }

        private void ShowImageGallery(object sender)
        {
            CreateImages();
            TourGalleryPage tourGalleryPage = new TourGalleryPage(Images);
            System.Windows.Application.Current.Windows.OfType<MainWindow>().FirstOrDefault().ToursOverviewFrame.Content = tourGalleryPage;
        }

        private void CreateTour(object sender)
        {
            var id = _tourService.NextId();
            var tour = new Tour(id, Name, FindLocationId(), Description, Language, MaxNumOfGuests, Duration);
            SetCheckpointsTourId(id);
            SetAppointmentsTourAndUserId(id);
            SetImagesTourId(id);
            _tourService.Save(tour);
            _checkpointService.SaveAll(new List<Checkpoint>(Checkpoints));
            _appointmentService.SaveAll(new List<Appointment>(Appointments));
            _imageService.SaveAll(Images);
        }

        private void SetImagesTourId(int tourId)
        {
            foreach (var image in Images)
            {
                image.EntityId = tourId;
            }
        }

        private void SetCheckpointsTourId(int id)
        {
            foreach (Checkpoint checkpoint in Checkpoints)
            {
                checkpoint.TourId = id;
            }
        }

        private void SetAppointmentsTourAndUserId(int id)
        {
            foreach (Appointment appointment in Appointments)
            {
                appointment.TourId = id;
                appointment.UserId = LoggedUser.Id;
            }
        }

        private int FindLocationId()
        {
            var location = Locations.Find(l => l.Country == Country && l.City == City);
           
            return location.Id;
        }

        private void SelectImagesPaths(object parameter)
        {
            ImagePaths.Clear();
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "Image Files|*.jpg;*.png;*.bmp|All Files|*.*";
            openFileDialog.InitialDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources\\Images\\Tours");
            openFileDialog.RestoreDirectory = true;

            bool? result = openFileDialog.ShowDialog();

            foreach (string fileName in openFileDialog.FileNames)
            {
                string relativePath = fileName.Replace(openFileDialog.InitialDirectory, "").TrimStart('\\');
                if (!string.IsNullOrEmpty(relativePath))
                {
                    string imagePath = Path.Combine("/Resources/Images/Tours", relativePath).Replace('\\', '/');
                    ImagePaths.Add(imagePath);
                }
            }
        }

        private void CreateImages()
        {
            Images.Clear();
            if (ImagePaths.Count > 0)
            {
                foreach (var imagePath in ImagePaths)
                {

                    var image = new Image
                    {
                        Cover = false,
                        Path = imagePath,
                        Type = Image.ImageType.TOUR
                    };

                    //Set first image as cover (default)
                    if (Images.Count == 0)
                    {
                        image.Cover = true;
                    }

                    Images.Add(image);
                }
            }
        }
    }

}