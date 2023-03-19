using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TravelAgency.DTO;
using TravelAgency.Model;
using TravelAgency.Repository;
using static System.Net.Mime.MediaTypeNames;

namespace TravelAgency.View
{
    /// <summary>
    /// Interaction logic for ToursOverview.xaml
    /// </summary>
    public partial class ToursOverview : Window
    {
        public User LoggedInUser { get; set; }
        public static ObservableCollection<Tour> Tours { get; set; }
        public static ObservableCollection<Location> Locations { get; set; }

        public static ObservableCollection<Appointment> Appointments { get; set; }
        public static ObservableCollection<TourDTO> TourDTOs { get; set; }
        public List<GuestAttendance> UserAttendances { get; set; }
        public TourDTO Selected { get; set; }

        private readonly TourRepository _repository;
        private readonly LocationRepository _locationRepository;
        private readonly AppointmentRepository _appointmentRepository;
        private readonly GuestAttendanceRepository _guestAttendanceRepository;
        public ToursOverview(User loggedInUser)
        {
            InitializeComponent();
            DataContext = this;
            LoggedInUser = loggedInUser;
            _repository = new TourRepository();
            _locationRepository = new LocationRepository();
            _appointmentRepository = new AppointmentRepository();
            _guestAttendanceRepository = new GuestAttendanceRepository();
            Tours = new ObservableCollection<Tour>(_repository.GetAll());
            TourDTOs = new ObservableCollection<TourDTO>();
            Locations = new ObservableCollection<Location>(_locationRepository.GetAll());
            Appointments = new ObservableCollection<Appointment>(_appointmentRepository.GetAll());
            UserAttendances = new List<GuestAttendance>(_guestAttendanceRepository.GetByUserId(loggedInUser.Id));

            FillDTOList();
        }


        public void GetAttendanceMessage()
        {
            foreach(var attendance in UserAttendances)
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
                _guestAttendanceRepository.Update(attendance);
            }
            else
            {
                attendance.Presence = GuestPresence.NO;
                _guestAttendanceRepository.Update(attendance);
            }
        }

        private static void FillDTOList()
        {
            foreach (Tour t in Tours)
            {
                foreach (Location l in Locations)
                {
                    foreach(Appointment a in Appointments)
                    {
                        if (l.Id == t.LocationId && t.Id == a.TourId)
                        {
                            TourDTO tourDTO = new TourDTO(t.Name, t.Language, t.MaxNumOfGuests, t.Duration, a.Occupancy, l.City, l.Country, t.Id, a.Time, a.Date);
                            TourDTOs.Add(tourDTO);
                        }
                    }
                }
            }
        }

        private void BookButtonClick(object sender, RoutedEventArgs e)
        {
            if(Selected == null)
            {
                MessageBox.Show("Izaberi turu za rezervaciju");
            }
            else
            {
               if(Selected.Ocupancy < Selected.MaxNumOfGuests)
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
            this.Close();
        }

        private void OpenAlternativeToursWindow()
        {
            MessageBox.Show("Nema slobodnih mesta za odabranu turu");
            AlternativeToursWindow alternativeTours = new AlternativeToursWindow(Selected, LoggedInUser, TourDTOs);
            alternativeTours.Show();
            Close();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void SearchToursButtonClick(object sender, RoutedEventArgs e)
        {
            SearchToursWindow searchToursWindow = new SearchToursWindow(TourDTOs,LoggedInUser);
            searchToursWindow.Show();
            this.Close();
        }

        private void RefreshToursButtonClick(object sender, RoutedEventArgs e)
        {
            ToursGrid.ItemsSource = TourDTOs;
        }
    }
}