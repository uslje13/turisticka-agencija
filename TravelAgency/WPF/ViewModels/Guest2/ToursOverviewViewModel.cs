using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Repositories;
using SOSTeam.TravelAgency.WPF.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest2
{
    public class ToursOverviewViewModel : ViewModel
    {
        private Window _window;
        public User LoggedInUser { get; set; }
        public static ObservableCollection<Tour> Tours { get; set; }
        public static ObservableCollection<Location> Locations { get; set; }
        public List<GuestAttendance> UserAttendances { get; set; }
        public static ObservableCollection<Appointment> Appointments { get; set; }
       
        public static ObservableCollection<TourViewModel> TourViewModels { get; set; }

        private TourViewModel selected;

        public TourViewModel Selected
        {
            get { return selected; }
            set
            {
                selected = value;
                OnPropertyChanged("Selected");
            }
        }

        private TourService _tourService;
        private LocationService _locationService;
        private AppointmentService _appointmenService;
        private GuestAttendanceService _guestAttendanceService;

        private RelayCommand searchCommand;

        public RelayCommand SearchCommand
        {
            get { return searchCommand; }
            set
            {
                searchCommand = value; 
            }
        }

        private RelayCommand reserveCommand;

        public RelayCommand ReserveCommand
        {
            get { return reserveCommand; }
            set
            {
                reserveCommand = value;
            }
        }

        public ToursOverviewViewModel(User loggedInUser, Window window)
        {
            InitializeServices();
            GetUsableLists();
            FillTourViewModelList();
            LoggedInUser = loggedInUser;
            SearchCommand = new RelayCommand(Execute_OpenSearchWindow, CanExecuteMethod);
            ReserveCommand = new RelayCommand(Execute_OpenBookTourWindow, CanExecuteMethod);
            _window = window;
        }

        private void GetUsableLists()
        {
            Appointments = new ObservableCollection<Appointment>(_appointmenService.GetAll());
            Locations = new ObservableCollection<Location>(_locationService.GetAll());
            Tours = new ObservableCollection<Tour>(_tourService.GetAll());
            UserAttendances = new List<GuestAttendance>(_guestAttendanceService.GetAll());
            TourViewModels = new ObservableCollection<TourViewModel>();
        }

        private void InitializeServices()
        {
            _tourService = new TourService();
            _locationService = new LocationService();
            _appointmenService = new AppointmentService();
            _guestAttendanceService = new GuestAttendanceService();
        }

        private bool CanExecuteMethod(object parameter)
        {
            return true;
        }
        public void GetAttendanceMessage()
        {
            foreach (var attendance in UserAttendances)
            {
                if (attendance.Presence.Equals(GuestPresence.UNKNOWN))
                {
                    ShowAttendanceMessage(attendance);
                }
            }
        }

        private void ShowAttendanceMessage(GuestAttendance attendance)
        {
            MessageBoxResult result = MessageBox.Show(attendance.Message, "Prisutnost",
                        MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                attendance.Presence = GuestPresence.YES;
                _guestAttendanceService.Update(attendance);
            }
            else
            {
                attendance.Presence = GuestPresence.NO;
                _guestAttendanceService.Update(attendance);
            }
        }

        private static void FillTourViewModelList()
        {
            foreach (Tour t in Tours)
            {
                foreach (Location l in Locations)
                {
                    foreach (Appointment a in Appointments)
                    {
                        if (l.Id == t.LocationId && t.Id == a.TourId)
                        {
                            TourViewModel tourDTO = new TourViewModel(t.Name, t.Language, t.MaxNumOfGuests, t.Duration, a.Occupancy, l.City, l.Country, t.Id, a.Time, a.Date);
                            TourViewModels.Add(tourDTO);
                        }
                    }
                }
            }
        }

        private void Execute_OpenBookTourWindow(object sender)
        {
            if (Selected == null)
            {
                MessageBox.Show("Izaberi turu za rezervaciju");
            }
            else
            {
                if (Selected.Ocupancy < Selected.MaxNumOfGuests)
                {
                    OpenBookTourWindow();
                }
                else
                {
                    OpenAlternativeToursWindow();
                }
            }
        }

        private void OpenBookTourWindow()
        {
            BookTourWindow bookTourWindow = new BookTourWindow(Selected, LoggedInUser);
            bookTourWindow.Show();
            _window.Close();
        }

        private void OpenAlternativeToursWindow()
        {
            MessageBox.Show("Nema slobodnih mesta za odabranu turu");
            AlternativeToursWindow alternativeTours = new AlternativeToursWindow(Selected, LoggedInUser, TourViewModels);
            alternativeTours.Show();
            _window.Close();
        }

        private void Execute_OpenSearchWindow(object sender)
        {
            SearchToursWindow searchToursWindow = new SearchToursWindow(TourViewModels, LoggedInUser);
            searchToursWindow.Show();
            _window.Close();
        }
    }
}
