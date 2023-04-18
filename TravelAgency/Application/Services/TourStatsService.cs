using SOSTeam.TravelAgency.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.Application.Services
{
    public class TourStatsService
    {
        private readonly AppointmentService _appointmentService;
        private readonly ReservationService _reservationService;

        public TourStatsService()
        {
            _appointmentService = new AppointmentService();
            _reservationService = new ReservationService();
        }

        public Appointment? FindMostAttendedAppointment(bool isPerYear, string year, User loggedUser)
        {
            int maxOfPresenceGuests = 0;
            Appointment? mostAttendedAppointment = null;
            var finishedAppointments = _appointmentService.GetAllFinishedByUserId(loggedUser.Id);

            foreach (var appointment in finishedAppointments)
            {
                if (!isPerYear || appointment.Date.Year.ToString() == year)
                {
                    int currentPresenceGuests = 0;

                    var reservations = _reservationService.GetAllByAppointmentId(appointment.Id);

                    foreach (var reservation in reservations)
                    {
                        if (reservation.Presence)
                        {
                            currentPresenceGuests += reservation.TouristNum;
                        }
                    }

                    if (currentPresenceGuests > maxOfPresenceGuests)
                    {
                        maxOfPresenceGuests = currentPresenceGuests;
                        mostAttendedAppointment = appointment;
                    }
                }
            }

            return mostAttendedAppointment;
        }
    }
}
