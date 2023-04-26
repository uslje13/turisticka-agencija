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
        private static ToursOverviewWindow _window;
        public static User LoggedInUser { get; set; }
        public static ObservableCollection<Tour> Tours { get; set; }
        public static ObservableCollection<Location> Locations { get; set; }
        public List<GuestAttendance> UserAttendances { get; set; }

        public static ObservableCollection<TourViewModel> TourViewModels { get; set; }
        public static ObservableCollection<TourViewModel> SerbiaTourViewModels { get; set; }
        public static ObservableCollection<TourViewModel> SummerTourViewModels { get; set; }

        public static ArrowCommandsViewModel ArrowCommands { get; set; }

        private TourService _tourService;
        private LocationService _locationService;
        private GuestAttendanceService _guestAttendanceService;
        private ReservationService _reservationService;
        private AppointmentService _appointmentService;
        private ImageService _imageService;

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

        private RelayCommand _vouchersCommand;

        public RelayCommand VouchersCommand
        {
            get { return _vouchersCommand; }
            set
            {
                _vouchersCommand = value;
            }
        }

        private RelayCommand _notificationsCommand;

        public RelayCommand NotificationsCommand
        {
            get { return _notificationsCommand; }
            set
            {
                _notificationsCommand = value;
            }
        }

        public MainViewModel(User loggedInUser, ToursOverviewWindow window)
        {
            _window = window;
            InitializeServices();
            GetUsableLists();
            LoggedInUser = loggedInUser;
            TourViewModels = new ObservableCollection<TourViewModel>();
            SummerTourViewModels = new ObservableCollection<TourViewModel>();
            SerbiaTourViewModels = new ObservableCollection<TourViewModel>();
            CreateCommands();
            FillTourViewModelList();
            FillSerbiaTourViewModelList();
            FillSummerTourViewModelList();
            ArrowCommands = new ArrowCommandsViewModel(TourViewModels,SerbiaTourViewModels,SummerTourViewModels);
        }

        private void FindImage(Tour t,Location l, ObservableCollection<TourViewModel> tourViewModels)
        {
            foreach(Image i in _imageService.GetAll())
            {
                if(i.Type.Equals(ImageType.TOUR) && i.EntityId == t.Id && i.Cover)
                {
                    TourViewModel tourDTO = new TourViewModel(t.Id, t.Name, t.Language, t.Duration, t.MaxNumOfGuests, l.City, l.Country, LoggedInUser, _window,i.Path);
                    tourViewModels.Add(tourDTO);
                    break;
                }
            }
        }
        private void FillSummerTourViewModelList()
        {
            DateTime summerStart = new DateTime(DateTime.Now.Year, 6, 21);
            DateTime summerEnd = new DateTime(DateTime.Now.Year, 9, 23);

            foreach (Tour t in Tours)
            {
                foreach (Location l in Locations)
                {
                    if (l.Id == t.LocationId)
                    {
                        FillList(summerStart, summerEnd, t, l);
                    }
                }
            }
        }

        private void FillList(DateTime summerStart, DateTime summerEnd, Tour t, Location l)
        {
            foreach (Appointment a in _appointmentService.GetAll())
            {
                if (a.TourId == t.Id && a.Start >= summerStart && a.Start <= summerEnd)
                {
                    FindImage(t, l,SummerTourViewModels);
                    break;
                }
            }
        }

        private void FillSerbiaTourViewModelList()
        {
            foreach (Tour t in Tours)
            {
                foreach (Location l in Locations)
                {
                    if (l.Id == t.LocationId && l.Country == "Srbija")
                    {
                        FindImage(t, l,SerbiaTourViewModels);
                    }
                }
            }
        }

        private void CreateCommands()
        {
            SearchCommand = new RelayCommand(Execute_SearchPageCommand, CanExecuteMethod);
            HelpCommand = new RelayCommand(Execute_HelpPageCommand, CanExecuteMethod);
            RequestsCommand = new RelayCommand(Execute_RequestsPageCommand, CanExecuteMethod);
            MyToursCommand = new RelayCommand(Execute_MyToursPageCommand, CanExecuteMethod);
            VouchersCommand = new RelayCommand(Execute_VouchersWindowCommand, CanExecuteMethod);
            NotificationsCommand = new RelayCommand(Execute_NotificationsWindowCommand, CanExecuteMethod);
        }
        private void Execute_NotificationsWindowCommand(object obj)
        {
            NotificationsWindow window = new NotificationsWindow(LoggedInUser);
            window.ShowDialog();
        }

        private void Execute_VouchersWindowCommand(object obj)
        {
            VouchersWindow window = new VouchersWindow();
            window.ShowDialog();
        }

        private void Execute_MyToursPageCommand(object obj)
        {
            var navigationService = _window.MyToursFrame.NavigationService;
            navigationService.Navigate(new MyToursPage(LoggedInUser));
        }

        private void Execute_RequestsPageCommand(object obj)
        {
            var navigationService = _window.RequestsFrame.NavigationService;
            navigationService.Navigate(new RequestsPage());
        }

        private void Execute_SearchPageCommand(object obj)
        {
            var navigationService = _window.SearchFrame.NavigationService;
            navigationService.Navigate(new SearchPage(LoggedInUser,TourViewModels,SerbiaTourViewModels,SummerTourViewModels));
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
            _reservationService = new ReservationService();
            _appointmentService= new AppointmentService();
            _imageService = new ImageService();
        }

        public void GetAttendanceMessage()
        {
            foreach (var attendance in UserAttendances)
            {
                if (attendance.Presence.Equals(GuestPresence.UNKNOWN) && attendance.UserId == LoggedInUser.Id)
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
                _reservationService.SetPresence(attendance.ReservationId);
            }
            else
            {
                attendance.Presence = GuestPresence.NO;
                _guestAttendanceService.Update(attendance);                
            }
        }

        private void FillTourViewModelList()
        {
            foreach (Tour t in Tours)
            {
                foreach (Location l in Locations)
                {
                    if (l.Id == t.LocationId)
                    {
                        FindImage(t, l,TourViewModels);
                    }
                }
            }
        }
    }
}
