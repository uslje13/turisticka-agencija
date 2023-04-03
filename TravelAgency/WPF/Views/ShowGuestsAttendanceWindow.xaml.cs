using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using TravelAgency.DTO;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Repositories;

namespace SOSTeam.TravelAgency.WPF.Views
{
    /// <summary>
    /// Interaction logic for ShowGuestsAttendanceWindow.xaml
    /// </summary>
    public partial class ShowGuestsAttendanceWindow : Window
    {
        public List<Reservation> Resevations { get; set; }

        public List<GuestAttendance> GuestsAttendances { get; set; }
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


        public ShowGuestsAttendanceWindow(CheckpointActivity selectedCheckpointActivity)
        {
            InitializeComponent();
            DataContext = this;

            SelectedCheckpointActivity = selectedCheckpointActivity;
            _reservationRepository = new ReservationRepository();
            _guestAttendanceRepository = new GuestAttendanceRepository();
            _checkpointRepository = new CheckpointRepository();
            _userRepository = new UserRepository();

            Resevations = GetAppointemntReservatons();

            CreateGuestsAttendances();
            GuestsAttendances = _guestAttendanceRepository.GetAllByActivityId(SelectedCheckpointActivity.Id);
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
            List<GuestAttendance> guestsAttendances = new List<GuestAttendance>(_guestAttendanceRepository.GetAll());
            List<GuestAttendance> attendances = new List<GuestAttendance>();
            GuestAttendance founded = guestsAttendances.Find(a => a.CheckpointActivityId == SelectedCheckpointActivity.Id);
            if (founded == null)
            {
                foreach (Reservation reservation in Resevations)
                {
                    attendances.Add(CreateGuestAttendance(reservation));
                }
                attendances = new List <GuestAttendance>(attendances.DistinctBy(i => i.UserId));
                _guestAttendanceRepository.SaveAll(attendances);
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
