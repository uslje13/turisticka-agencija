using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOSTeam.TravelAgency.Domain;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Domain.RepositoryInterfaces;
using SOSTeam.TravelAgency.WPF.ViewModels.Guest2;

namespace SOSTeam.TravelAgency.Application.Services
{
    public class ReservationService
    {
        private readonly IReservationRepository _reservationRepository = Injector.CreateInstance<IReservationRepository>();
        private readonly IAppointmentRepository _appointmentRepository = Injector.CreateInstance<IAppointmentRepository>();
        public ReservationService() { }

        public void Delete(int id)
        {
            _reservationRepository.Delete(id);
        }

        public List<Reservation> GetAll()
        {
            return _reservationRepository.GetAll();
        }

        public Reservation GetById(int id)
        {
            return _reservationRepository.GetById(id);
        }

        public void Update(Reservation reservation)
        {
            _reservationRepository.Update(reservation);
        }

        public void Save(Reservation reservation)
        {
            _reservationRepository.Save(reservation);
        }

        public List<Reservation> GetAllByAppointmentId(int id)
        {
            return  _reservationRepository.GetAllByAppointmentId(id);
        }

       public void CreateReservation(AppoitmentOverviewViewModel selected, User loggedInUser, int touristNum, float averageAge)
       {
           foreach (Appointment a in _appointmentRepository.GetAll())
           {
               if (selected.TourId == a.TourId && selected.Date == a.Date && selected.Time == a.Time)
               {
                   a.Occupancy += touristNum;
                  // selected.Ocupancy += touristNum;
                   _appointmentRepository.Update(a);
                   Reservation newReservation = new Reservation(touristNum,averageAge, loggedInUser.Id, a.Id);
                   _reservationRepository.Save(newReservation);
               }
           }
        }
    }
}
