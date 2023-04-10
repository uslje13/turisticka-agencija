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
    public class AccommodationService
    {
        private readonly IAccommodationRepository _accommodationRepository = Injector.CreateInstance<IAccommodationRepository>();

        public AccommodationService()
        {

        }

        public void Delete(int id)
        {
            _accommodationRepository.Delete(id);
        }

        public List<Accommodation> GetAll()
        {
            return _accommodationRepository.GetAll();
        }

        public Accommodation GetById(int id)
        {
            return _accommodationRepository.GetById(id);
        }

        public void Save(Accommodation accommodation)
        {
            _accommodationRepository.Save(accommodation);
        }

        public void Update(Accommodation accommodation)
        {
            _accommodationRepository.Update(accommodation);
        }
    }
}
