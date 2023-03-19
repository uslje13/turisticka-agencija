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
using TravelAgency.DTO;
using TravelAgency.Model;
using TravelAgency.Repository;

namespace TravelAgency.View
{
    /// <summary>
    /// Interaction logic for BookTourWindow.xaml
    /// </summary>
    public partial class BookTourWindow : Window
    {
        public User LoggedInUser { get; set; }
        private string _availableSlots; 
        private string _touristNum; 
        private TourDTO _selected;

        public static ObservableCollection<Appointment> Appointments;
        private readonly AppointmentRepository _appointmentRepository;
        private readonly ReservationRepository _reservationRepository;
        public BookTourWindow(TourDTO selected, User loggedInUser)
        {
            InitializeComponent();
            DataContext = this;
            LoggedInUser = loggedInUser;
            _selected= selected;
            _availableSlots = (selected.MaxNumOfGuests - selected.Ocupancy).ToString();
            _appointmentRepository = new AppointmentRepository();
            _reservationRepository = new ReservationRepository();
            Appointments = new ObservableCollection<Appointment>(_appointmentRepository.GetAll());
        }

        public string AvailableSlots
        {
            get { return _availableSlots;}
            set
            {
                if(value != _availableSlots )
                {
                    _availableSlots = value;
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
            if (_touristNum == null || _touristNum == "" || int.Parse(_touristNum) == 0)
            {
                MessageBox.Show("Niste uneli broj osoba prilikom rezervacije");
            }
            else if (int.Parse(_touristNum) > int.Parse(_availableSlots))
            {
                MessageBox.Show("Ne moze se rezervisati tura, nema dovoljno slobodnih mesta");
            }
            else
            {
                MessageBoxResult result = ConfirmReservation();

                if (result == MessageBoxResult.Yes)
                {
                    CreateReservation();
                    OpenToursOverviewWindow();
                    this.Close();
                }
            }
        }

        private void CreateReservation()
        {
            foreach (Appointment a in Appointments)
            {
                if (_selected.TourId == a.TourId && _selected.Date == a.Date && _selected.Time == a.Time)
                {
                    a.Occupancy += int.Parse(_touristNum);
                    _selected.Ocupancy += int.Parse(_touristNum);
                    _appointmentRepository.Update(a);
                    Reservation newReservation = new Reservation( int.Parse(_touristNum), LoggedInUser.Id, a.Id);
                    _reservationRepository.Save(newReservation);
                }
            }
        }

        private void OpenToursOverviewWindow()
        {
            ToursOverviewWindow overview = new ToursOverviewWindow(LoggedInUser);
            overview.Show();
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
            OpenToursOverviewWindow();
            this.Close();
        }
    }
}
