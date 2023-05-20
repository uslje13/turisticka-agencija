using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Domain.RepositoryInterfaces;
using SOSTeam.TravelAgency.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.Application.Services
{
    public class SuperGuestService
    {
        private readonly ISuperGuestRepository superGuestRepository = Injector.CreateInstance<ISuperGuestRepository>();

        public SuperGuestService() { }

        public void Delete(int id)
        {
            superGuestRepository.Delete(id);
        }

        public List<SuperGuest> GetAll()
        {
            return superGuestRepository.GetAll();
        }

        public SuperGuest GetById(int id)
        {
            return superGuestRepository.GetById(id);
        }

        public void Save(SuperGuest superGuest)
        {
            superGuestRepository.Save(superGuest);
        }

        public void Update(SuperGuest superGuest)
        {
            superGuestRepository.Update(superGuest);
        }

        public void ClearSuperGuestCSV()
        {
            List<SuperGuest> _superGuests = superGuestRepository.GetAll();
            if (_superGuests.Count > 0)
            {
                foreach (var guest in _superGuests)
                {
                    superGuestRepository.Delete(guest.Id);
                }
            }
        }

        public int CalculateUnusedBonusPoints(User user, int points)
        {
            AccommodationReservationService reservationService = new AccommodationReservationService();
            foreach (var reservation in reservationService.GetAll())
            {
                if (reservation.UserId == user.Id && reservation.FirstDay.Year == DateTime.Today.Year && points > 0)
                {
                    points--;
                }
            }
            return points;
        }

        public int InitializeBonusPoints(int resNumber)
        {
            int points = 0;
            if (resNumber >= 10) points = 5;
            return points;
        }

        public bool IntializeSuperStatus(int resNumber)
        {
            bool isSuper = false;
            if (resNumber >= 10) isSuper = true;
            return isSuper;
        }
    }
}
