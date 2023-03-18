using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
using TravelAgency.Converter;
using TravelAgency.DTO;
using TravelAgency.Model;
using TravelAgency.Repository;

namespace TravelAgency.View
{
    /// <summary>
    /// Interaction logic for ShowGuestsAttendanceWindow.xaml
    /// </summary>
    public partial class ShowGuestsAttendanceWindow : Window
    {
        public List<Reservation> AppointmentReservations { get; set; }

        public ObservableCollection<GuestAttendance> GuestsAttendances { get; set; }
        public ObservableCollection<GuestAttendanceDTO> GuestsAttendancesDTO { get; set; }

        public CheckpointActivity SelectedCheckpointActivity { get; set; }

        private readonly ReservationRepository _reservationRepository;

        private readonly GuestAttendanceRepository _guestAttendanceRepository;

        private readonly CheckpointRepository _checkpointRepository;

        private readonly UserRepository _userRepository;

        private void FillObservableCollection()
        {
            foreach (GuestAttendance guestAttendance in GuestsAttendances)
            {
                User user = _userRepository.GetById(guestAttendance.UserId);
                GuestsAttendancesDTO.Add(new GuestAttendanceDTO(guestAttendance.Id, user.Username, guestAttendance.Presence));
            }
        }

        /*
        public void UpdateObservableCollection()
        {
            GuestsAttendancesDTO.Clear();
            FillObservableCollection();
        }*/


        public ShowGuestsAttendanceWindow(CheckpointActivity selectedCheckpointActivity)
        {
            InitializeComponent();
            DataContext = this;

            SelectedCheckpointActivity = selectedCheckpointActivity;
            _reservationRepository = new ReservationRepository();
            _guestAttendanceRepository = new GuestAttendanceRepository();
            _checkpointRepository = new CheckpointRepository();
            _userRepository = new UserRepository();

            AppointmentReservations = GetAppointemntReservatons();

            CreateGuestsAttendances();      //odmah kada otvorim prozor kreiram listu prisutnih gostiju za taj aktivni cekpoint
            GuestsAttendances = new ObservableCollection<GuestAttendance>(_guestAttendanceRepository.GetAllByChechpointActivityId(SelectedCheckpointActivity.Id));
            GuestsAttendancesDTO = new ObservableCollection<GuestAttendanceDTO>();
            FillObservableCollection();
        }

        private List<Reservation> GetAppointemntReservatons()
        {
            List<Reservation> reservations = new List<Reservation>(_reservationRepository.GetAll());
            return reservations.FindAll(r => r.AppointmentId == SelectedCheckpointActivity.AppointmentId);
        }

        private void CreateGuestsAttendances()
        {
            //proverim da li je u posecenosti vec kreirana poruka o aktivnom cekpointu
            //Ako nije znaci da je selektovan drugi aktivni cekpoint i njega dodam u listu!
            List<GuestAttendance> guestsAttendances = new List<GuestAttendance>(_guestAttendanceRepository.GetAll());
            List<GuestAttendance> newGuestsAttendances = new List<GuestAttendance>();
            GuestAttendance founded = guestsAttendances.Find(a => a.CheckpointActivityId == SelectedCheckpointActivity.Id);
            if(founded == null)
            {
                foreach (Reservation reservation in AppointmentReservations)
                {
                    newGuestsAttendances.Add(CreateGuestAttendance(reservation));
                }
                _guestAttendanceRepository.SaveAll(newGuestsAttendances);
            }
            
        }

        private GuestAttendance CreateGuestAttendance(Reservation reservation) 
        {

            GuestAttendance guestAttendance = new GuestAttendance();
            guestAttendance.UserId = reservation.UserId;
            guestAttendance.CheckpointActivityId = SelectedCheckpointActivity.Id;
            guestAttendance.Message = CreateMessage();

            return guestAttendance;
        }

        private string CreateMessage()
        {
            Checkpoint checkpoint = _checkpointRepository.GetById(SelectedCheckpointActivity.CheckpointId);
            string message = "Da li ste prisutni na klučnoj tački: " + checkpoint.Name;
            return message;
        }
    }
}
