using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOSTeam.TravelAgency.Domain;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Domain.RepositoryInterfaces;

namespace SOSTeam.TravelAgency.Application.Services
{
    public class ReservationService
    {
        private readonly IReservationRepository _reservationRepository = Injector.CreateInstance<IReservationRepository>();
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
    }
}
