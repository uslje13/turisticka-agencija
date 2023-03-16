using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
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

namespace TravelAgency.View
{
    /// <summary>
    /// Interaction logic for BookTour.xaml
    /// </summary>
    public partial class BookTour : Window
    {
        public User LoggedInUser { get; set; }
        private string _vacantSeats; // potencijalno promeni naziv prom
        private string _touristNum; // potencijalno promeni naziv prom
        private TourDTO _selectedTourDTO;
        private ObservableCollection<string> _times;
        private ObservableCollection<string> _dates;

        public static ObservableCollection<Appointment> Appointments;
        private readonly TourRepository _tourRepository;
        private readonly LocationRepository _locationRepository;
        private readonly AppointmentRepository _appointmentRepository; 
        public BookTour(TourDTO selectedTourDTO, User user)
        {
            InitializeComponent();
            int vacantSeats;
            DataContext = this;
            LoggedInUser = user;
            _selectedTourDTO= selectedTourDTO;
            vacantSeats = selectedTourDTO.MaxNumOfGuests - selectedTourDTO.Ocupancy;
            _vacantSeats = vacantSeats.ToString();
            _tourRepository = new TourRepository();
            _locationRepository = new LocationRepository();
            _appointmentRepository = new AppointmentRepository();
            Appointments = new ObservableCollection<Appointment>(_appointmentRepository.GetAll());
            _times = new ObservableCollection<string>();
            _dates = new ObservableCollection<string>();

            foreach(Appointment a in Appointments)
            {
                if(selectedTourDTO.TourId == a.TourId)
                {
                    _times.Add(a.Time.ToString());
                    _dates.Add(a.Date.ToString());
                }
            }
        }

        public ObservableCollection<string> Times
        {
            get { return _times; }
            set
            {
                if (value != _times)
                {
                    _times = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<string> Dates
        {
            get { return _dates; }
            set
            {
                if (value != _dates)
                {
                    _dates = value;
                    OnPropertyChanged();
                }
            }
        }

        public string VacantSeats
        {
            get { return _vacantSeats;}
            set
            {
                if(value != _vacantSeats )
                {
                    _vacantSeats = value;
                    OnPropertyChanged();
                }
            }
        }

        public string TouristNum
        {
            get { return _touristNum; }
            set
            {
                if (value != _touristNum)
                {
                    _touristNum = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ReserveClick(object sender, RoutedEventArgs e)
        {
            if (_touristNum == null || _touristNum == "")
            {
                MessageBox.Show("Niste uneli broj osoba prilikom rezervacije");
            }
            else if (int.Parse(_touristNum) > int.Parse(_vacantSeats))
            {
                MessageBox.Show("Ne moze se rezervisati tura, nema dovoljno slobodnih mesta");
            }
            else
            {
                MessageBoxResult result = ConfirmReservation();

                if (result == MessageBoxResult.Yes)
                {
                    foreach (Appointment a in Appointments)
                    {
                        if (_selectedTourDTO.TourId == a.TourId && _selectedTourDTO.Date == a.Date && _selectedTourDTO.Time == a.Time)
                        {
                            a.Occupancy += int.Parse(_touristNum);
                            _selectedTourDTO.Ocupancy += int.Parse(_touristNum);
                            _appointmentRepository.Update(a);
                            Reservation newReservation = new Reservation(a.TourId, int.Parse(_touristNum), LoggedInUser.Id, a.Id);
                            ReservationRepository reservationRepository = new ReservationRepository();
                            reservationRepository.Save(newReservation);
                        }
                    }
                    ShowAndSearchTours showAndSearchTours = new ShowAndSearchTours(LoggedInUser);
                    showAndSearchTours.Show();
                    this.Close();
                }
            }
        }

        private MessageBoxResult ConfirmReservation()
        {
            string sMessageBoxText = $"Da li ste sigurni da želite da rezervisete turu";
            string sCaption = "Porvrda rezervacije";

            MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
            MessageBoxImage icnMessageBox = MessageBoxImage.Warning;

            MessageBoxResult result = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
            return result;
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            ShowAndSearchTours showAndSearchTours = new ShowAndSearchTours(LoggedInUser);
            showAndSearchTours.Show();
            this.Close();
        }
    }
}
