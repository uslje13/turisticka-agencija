using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest2
{
    public class ActiveTourPageViewModel : ViewModel
    {
        public User LoggedInUser { get; set; }
        public ActiveTourViewModel ActiveTour { get; set; }

        private ReservationService _reservationService;
        private AppointmentService _appointmentService;
        private CheckpointActivityService _checkpointActivityService;
        private CheckpointService _checkpointService;
        private TourService _tourService;
        public ActiveTourPageViewModel(User loggedInUser)
        {
            LoggedInUser = loggedInUser;
            _reservationService = new ReservationService();
            _appointmentService = new AppointmentService();
            _checkpointActivityService = new CheckpointActivityService();
            _checkpointService = new CheckpointService();
            _tourService = new TourService();
            FindActiveTour();
        }

        public void FindActiveTour()
        {
            Reservation reservation = _reservationService.FindReservationWhereUserIsPresent(LoggedInUser);

            if(reservation != null)
            {
                Appointment appointment = _appointmentService.GetById(reservation.AppointmentId);
                if (appointment.Started)
                {
                    CheckpointActivity activeCheckpoint =_checkpointActivityService.FindActiveCheckpoint(appointment.Id);
                    if(activeCheckpoint != null)
                    {
                        string checkpoinName = GetCheckpointName(activeCheckpoint.CheckpointId);
                        string tourName = GetTourName(activeCheckpoint.CheckpointId);
                        ActiveTour = new ActiveTourViewModel(tourName, checkpoinName);
                    }
                }
            }
        }

        private string GetTourName(int checkpointId)
        {
            int tourId = GetCheckpointTourId(checkpointId);
            return _tourService.GetTourName(tourId);
        }

        private int GetCheckpointTourId(int checkpointId)
        {
            return _checkpointService.GetById(checkpointId).TourId;
        }

        private string GetCheckpointName(int checkpointId)
        {
            return _checkpointService.GetById(checkpointId).Name;
        }

    }
}
