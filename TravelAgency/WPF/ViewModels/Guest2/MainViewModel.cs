using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.Views.Guest2;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest2
{
    public class MainViewModel : ViewModel
    {
        private ToursOverviewWindow _window;
        public static User LoggedInUser { get; set; }
        public static ObservableCollection<Tour> Tours { get; set; }
        public static ObservableCollection<Location> Locations { get; set; }
        public List<GuestAttendance> UserAttendances { get; set; }

        public static ObservableCollection<TourViewModel2> TourViewModels { get; set; }

        private TourService _tourService;
        private LocationService _locationService;
        private GuestAttendanceService _guestAttendanceService;

        private RelayCommand _searchCommand;

        public RelayCommand SearchCommand
        {
            get { return _searchCommand; }
            set
            {
                _searchCommand = value;
            }
        }

        private RelayCommand _helpCommand;

        public RelayCommand HelpCommand
        {
            get { return _helpCommand; }
            set
            {
                _helpCommand = value;
            }
        }

        private RelayCommand _requestsCommand;

        public RelayCommand RequestsCommand
        {
            get { return _requestsCommand; }
            set
            {
                _requestsCommand = value;
            }
        }

        private RelayCommand _myToursCommand;

        public RelayCommand MyToursCommand
        {
            get { return _myToursCommand; }
            set
            {
                _myToursCommand = value;
            }
        }
        public MainViewModel(User loggedInUser, ToursOverviewWindow window)
        {
            _window = window;
            InitializeServices();
            GetUsableLists();
            LoggedInUser = loggedInUser;
            TourViewModels = new ObservableCollection<TourViewModel2>();
            CreateCommands();
            FillTourViewModelList();
        }

        private void CreateCommands()
        {
            SearchCommand = new RelayCommand(Execute_SearchPageCommand, CanExecuteMethod);
            HelpCommand = new RelayCommand(Execute_HelpPageCommand, CanExecuteMethod);
            RequestsCommand = new RelayCommand(Execute_RequestsPageCommand, CanExecuteMethod);
            MyToursCommand = new RelayCommand(Execute_MyToursPageCommand, CanExecuteMethod);
        }

        private void Execute_MyToursPageCommand(object obj)
        {
            var navigationService = _window.MyToursFrame.NavigationService;
            navigationService.Navigate(new MyToursPage());
        }

        private void Execute_RequestsPageCommand(object obj)
        {
            var navigationService = _window.RequestsFrame.NavigationService;
            navigationService.Navigate(new RequestsPage());
        }

        private void Execute_SearchPageCommand(object obj)
        {
            var navigationService = _window.SearchFrame.NavigationService;
            navigationService.Navigate(new SearchPage());
        }

        private void Execute_HelpPageCommand(object obj)
        {
            var navigationService = _window.HelpFrame.NavigationService;
            navigationService.Navigate(new HelpPage());
        }

        private bool CanExecuteMethod(object parameter)
        {
            return true;
        }
        private void GetUsableLists()
        {
            Locations = new ObservableCollection<Location>(_locationService.GetAll());
            Tours = new ObservableCollection<Tour>(_tourService.GetAll());
            UserAttendances = new List<GuestAttendance>(_guestAttendanceService.GetAll());
        }

        private void InitializeServices()
        {
            _tourService = new TourService();
            _locationService = new LocationService();
            _guestAttendanceService = new GuestAttendanceService();
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
                    if (l.Id == t.LocationId)
                    {
                        TourViewModel2 tourDTO = new TourViewModel2(t.Id,t.Name, t.Language,t.Duration,l.City, l.Country, LoggedInUser);
                        TourViewModels.Add(tourDTO);
                    }
                }
            }
        }
    }
}
