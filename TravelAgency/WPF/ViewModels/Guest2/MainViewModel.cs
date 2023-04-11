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
        public User LoggedInUser { get; set; }
        public static ObservableCollection<Tour> Tours { get; set; }
        public static ObservableCollection<Location> Locations { get; set; }
        public List<GuestAttendance> UserAttendances { get; set; }

        public static ObservableCollection<TourViewModel2> TourViewModels { get; set; }

        private TourService _tourService;
        private LocationService _locationService;
        private GuestAttendanceService _guestAttendanceService;

        private RelayCommand _tourDetailsCommand;

        public RelayCommand TourDetailsCommand
        {
            get { return _tourDetailsCommand; }
            set
            {
                _tourDetailsCommand = value;
            }
        }
        public MainViewModel(User loggedInUser, Window window)
        {
            InitializeServices();
            GetUsableLists();
            TourViewModels = new ObservableCollection<TourViewModel2>();
            FillTourViewModelList();
            LoggedInUser = loggedInUser;
            TourDetailsCommand = new RelayCommand(Execute_OpenBookTourWindow,CanExecuteMethod);
        }

        private void Execute_OpenBookTourWindow(object obj)
        {
            BookTourWindow window = new BookTourWindow();
            window.ShowDialog();
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
                        TourViewModel2 tourDTO = new TourViewModel2(t.Name, t.Language,t.Duration,l.City, l.Country);
                        TourViewModels.Add(tourDTO);
                    }
                }
            }
        }
    }
}
