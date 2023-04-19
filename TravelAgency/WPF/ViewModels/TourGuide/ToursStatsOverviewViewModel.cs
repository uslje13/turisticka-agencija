using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;

namespace SOSTeam.TravelAgency.WPF.ViewModels.TourGuide
{
    public class ToursStatsOverviewViewModel : ViewModel
    {
        private TourCardViewModel _mostAttendedTourEver;

        public TourCardViewModel MostAttendedTourEver
        {
            get => _mostAttendedTourEver;
            set
            {
                if (_mostAttendedTourEver != value)
                {
                    _mostAttendedTourEver = value;
                    OnPropertyChanged("MostAttendedTourEver");
                }
            }
        }

        private TourCardViewModel _mostAttendedTourOfYear;

        public TourCardViewModel MostAttendedTourOfYear
        {
            get => _mostAttendedTourOfYear;
            set
            {
                if (_mostAttendedTourOfYear != value)
                {
                    _mostAttendedTourOfYear = value;
                    OnPropertyChanged("MostAttendedTourOfYear");
                }
            }
        }

        private string _selectedYear;

        public string SelectedYear
        {
            get => _selectedYear;
            set
            {
                if (_selectedYear != value)
                {
                    _selectedYear = value;
                    OnPropertyChanged("SelectedYear");
                }
            }
        }

        private ObservableCollection<string> _availableYears;

        public ObservableCollection<string> AvailableYears
        {
            get => _availableYears;
            set
            {
                if (_availableYears != value)
                {
                    _availableYears = value;
                    OnPropertyChanged("AvailableYears");
                }
            }

        }

        public User LoggedUser { get; set; }
        public RelayCommand YearSelectionChangedCommand { get; set; }

        private readonly AppointmentService _appointmentService;
        private readonly TourService _tourService;
        private readonly LocationService _locationService;
        private readonly ImageService _imageService;
        private readonly TourStatsService _tourStatsService;

        public ToursStatsOverviewViewModel(User loggedUser)
        {
            LoggedUser = loggedUser;
            _appointmentService = new AppointmentService();
            _tourService = new TourService();
            _locationService = new LocationService();
            _imageService = new ImageService();
            _tourStatsService = new TourStatsService();

            AvailableYears = new ObservableCollection<string>();
            MostAttendedTourEver = new TourCardViewModel();
            MostAttendedTourOfYear = new TourCardViewModel();
            AvailableYears = new ObservableCollection<string>();
            YearSelectionChangedCommand = new RelayCommand(ExecuteYearSelectionChanged, CanExecuteMethod);

            FindAvailableYears();
            FindMostAttendedTourEver();
            FindMostAttendedTourOfYear();
        }

        private bool CanExecuteMethod(object parameter)
        {
            return true;
        }


        private void FindMostAttendedTourEver()
        {
            Appointment? mostAttendedAppointmentEver = _tourStatsService.FindMostAttendedAppointment(false, string.Empty, LoggedUser);
            CreateMostAttendedTour(mostAttendedAppointmentEver, false);
        }

        private void FindMostAttendedTourOfYear()
        {
            Appointment? mostAttendedAppointmentOfYear = _tourStatsService.FindMostAttendedAppointment(true, SelectedYear, LoggedUser);
            CreateMostAttendedTour(mostAttendedAppointmentOfYear, true);
        }

        private void CreateMostAttendedTour(Appointment? mostAttendedAppointment, bool isPerYear)
        {
            if (mostAttendedAppointment == null) { return; }
            var tour = _tourService.GetById(mostAttendedAppointment.TourId);
            var location = _locationService.GetById(tour.LocationId);
            TourCardViewModel tourCard = new TourCardViewModel
            {
                TourId = tour.Id,
                AppointmentId = mostAttendedAppointment.Id,
                LocationId = tour.LocationId,
                Date = mostAttendedAppointment.Date,
                Name = tour.Name,
            };
            tourCard.SetAppointmentStatus(mostAttendedAppointment);
            tourCard.SetLocation(location);
            tourCard.SetCoverImage(_imageService.GetTourCover(tourCard.TourId));

            if (isPerYear)
            {
                MostAttendedTourOfYear = tourCard;
            }
            else
            {
                MostAttendedTourEver = tourCard;
            }
        }

        public void ExecuteYearSelectionChanged(object parameter)
        {
            YearSelectionChanged(parameter, null);
        }

        public void YearSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FindMostAttendedTourOfYear();
        }

        private void FindAvailableYears()
        {
            foreach (var appointment in _appointmentService.GetAllByUserId(LoggedUser.Id))
            {
                AvailableYears.Add(appointment.Date.Year.ToString());
            }

            AvailableYears = new ObservableCollection<string>(AvailableYears.Distinct());
            
            if (AvailableYears.Count > 0)
            {
                SelectedYear = AvailableYears[0];
            }
        }
    }
}
