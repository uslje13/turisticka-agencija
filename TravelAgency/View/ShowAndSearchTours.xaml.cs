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
using TravelAgency.Model;
using TravelAgency.Repository;
using static System.Net.Mime.MediaTypeNames;

namespace TravelAgency.View
{
    /// <summary>
    /// Interaction logic for ShowAndSearchTours.xaml
    /// </summary>
    public partial class ShowAndSearchTours : Window
    {
        public User LoggedInUser { get; set; }
        public static ObservableCollection<Tour> Tours { get; set; }
        public static ObservableCollection<Location> Locations { get; set; }

        public static ObservableCollection<Appointment> Appointments { get; set; }
        public static ObservableCollection<TourDTO> TourDTOs { get; set; }
        public TourDTO SelectedTourDTO { get; set; }

        private readonly TourRepository _repository;
        private readonly LocationRepository _locationRepository;
        private readonly AppointmentRepository _appointmentRepository;
        public ShowAndSearchTours(User user)
        {
            InitializeComponent();
            DataContext = this;
            LoggedInUser = user;
            _repository = new TourRepository();
            _locationRepository = new LocationRepository();
            _appointmentRepository = new AppointmentRepository();
            Tours = new ObservableCollection<Tour>(_repository.GetAll());
            TourDTOs = new ObservableCollection<TourDTO>();
            Locations = new ObservableCollection<Location>(_locationRepository.GetAll());
            Appointments = new ObservableCollection<Appointment>(_appointmentRepository.GetAll());
            GetDTOs();
        }

        private static void GetDTOs()
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
            if(SelectedTourDTO == null)
            {
                MessageBox.Show("Izaberi turu za rezervaciju");
            }
            else
            {
               if(SelectedTourDTO.Ocupancy < SelectedTourDTO.MaxNumOfGuests)
               {
                    BookTour bookTourWindow = new BookTour(SelectedTourDTO, LoggedInUser);
                    bookTourWindow.Show();
                    this.Close();
               }
               else
               {
                    MessageBox.Show("Nema slobodnih mesta za odabranu turu");
                    AlternativeTours alternativeTours = new AlternativeTours(SelectedTourDTO, LoggedInUser, TourDTOs);
                    alternativeTours.Show();
                    Close();
               }
            }
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