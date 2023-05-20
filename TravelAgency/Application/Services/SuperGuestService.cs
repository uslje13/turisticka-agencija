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
    }
}
