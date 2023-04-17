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
        private readonly ReservationService _reservationService;
        private readonly TourService _tourService;
        private readonly LocationService _locationService;
        private readonly ImageService _imageService;

        public ToursStatsOverviewViewModel(User loggedUser)
        {
            LoggedUser = loggedUser;
            _appointmentService = new AppointmentService();
            _reservationService = new ReservationService();
            _tourService = new TourService();
            _locationService = new LocationService();
            _imageService = new ImageService();
            AvailableYears = new ObservableCollection<string>();
            MostAttendedTourEver = new TourCardViewModel();
            MostAttendedTourOfYear = new TourCardViewModel();
            AvailableYears = new ObservableCollection<string>();
            YearSelectionChangedCommand = new RelayCommand(ExecuteYearSelectionChanged, CanExecuteMethod);

            FindAvailableYears();
            if (AvailableYears.Count > 0)
            {
                SelectedYear = AvailableYears[0];
            }
            FindMostAttendedTourEver();
            FindMostAttendedTourOfYear();
        }

        private bool CanExecuteMethod(object parameter)
        {
            return true;
        }


        private void FindMostAttendedTourEver()
        {
            int maxOfPresenceGuests = 0;
            int currentPresenceGuests = 0;
            Appointment maxAttendedAppointment = new Appointment();
            //pordjem kroz sve zavrsenene termine
            foreach (var appointment in _appointmentService.GetAllFinishedByUserId(LoggedUser.Id))
            {
                //prodjem kroz rezervacije za zavrsen termin
                foreach (var reservation in _reservationService.GetAllByAppointmentId(appointment.Id))
                {
                    //pitam jel bio prisutan
                    if (reservation.Presence)
                    {
                        currentPresenceGuests += reservation.TouristNum;
                    }           
                }
                //Sada sam nasao za termin koliko je bilo gostiju i pitam da li je vece od pretodnog prisustva
                if (currentPresenceGuests > maxOfPresenceGuests)
                {
                    maxOfPresenceGuests = currentPresenceGuests;
                    maxAttendedAppointment = appointment;
                }

                currentPresenceGuests = 0;
            }
            CreateMostAttendedTourEver(maxAttendedAppointment);
        }

        private void FindMostAttendedTourOfYear()
        {
            int maxOfPresenceGuests = 0;
            int currentPresenceGuests = 0;
            Appointment maxAttendedAppointmentOfYear = new Appointment();
            //pordjem kroz sve zavrsenene termine
            foreach (var appointment in _appointmentService.GetAllFinishedByUserId(LoggedUser.Id))
            {
                //prodjem kroz rezervacije za zavrsen termin
                if (appointment.Date.Year.ToString() == SelectedYear)
                {
                    foreach (var reservation in _reservationService.GetAllByAppointmentId(appointment.Id))
                    {
                        //pitam jel bio prisutan
                        if (reservation.Presence)
                        {
                            currentPresenceGuests += reservation.TouristNum;
                        }
                    }
                    //Sada sam nasao za termin koliko je bilo gostiju i pitam da li je vece od pretodnog prisustva
                    if (currentPresenceGuests > maxOfPresenceGuests)
                    {
                        maxOfPresenceGuests = currentPresenceGuests;
                        maxAttendedAppointmentOfYear = appointment;
                    }
                }
                currentPresenceGuests = 0;
            }
            CreateMostAttendedTourOfYear(maxAttendedAppointmentOfYear);
        }

        private void CreateMostAttendedTourEver(Appointment maxAttendedAppointmentEver)
        {
            Tour tour = _tourService.GetById(maxAttendedAppointmentEver.TourId);
            Location location = _locationService.GetById(tour.LocationId);
            TourCardViewModel tourCard = new TourCardViewModel();
            tourCard.Date = maxAttendedAppointmentEver.Date;
            tourCard.AppointmentId = maxAttendedAppointmentEver.Id;
            tourCard.Name = tour.Name;
            tourCard.Status = "Finished";
            tourCard.LocationId = location.Id;
            tourCard.Location = location.Country + " ," + location.City;
            SetImageField(tour, tourCard);
            MostAttendedTourEver = tourCard;
        }

        private void CreateMostAttendedTourOfYear(Appointment maxAttendedAppointmentOfYear)
        {
            Tour tour = _tourService.GetById(maxAttendedAppointmentOfYear.TourId);
            Location location = _locationService.GetById(tour.LocationId);
            TourCardViewModel tourCard = new TourCardViewModel();
            tourCard.Date = maxAttendedAppointmentOfYear.Date;
            tourCard.AppointmentId = maxAttendedAppointmentOfYear.Id;
            tourCard.Name = tour.Name;
            tourCard.Status = "Finished";
            tourCard.LocationId = location.Id;
            tourCard.Location = location.Country + " ," + location.City;
            SetImageField(tour, tourCard);
            MostAttendedTourOfYear = tourCard;
        }

        public void ExecuteYearSelectionChanged(object parameter)
        {
            YearSelectionChanged(parameter, null);
        }

        public void YearSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FindMostAttendedTourOfYear();
        }

        private void SetImageField(Tour tour, TourCardViewModel tourCard)
        {
            foreach (var image in _imageService.GetAllForTours())
            {
                if (image.Cover && image.EntityId == tour.Id)
                {
                    tourCard.CoverImagePath = image.Path;
                    break;
                }
            }
        }

        private void FindAvailableYears()
        {
            foreach (var appointment in _appointmentService.GetAllByUserId(LoggedUser.Id))
            {
                AvailableYears.Add(appointment.Date.Year.ToString());
            }
            AvailableYears = new ObservableCollection<string>(AvailableYears.Distinct());
        }

    }
}
