using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Domain.RepositoryInterfaces;

namespace SOSTeam.TravelAgency.Application.Services
{
    public class TourService
    {

        private readonly ITourRepository _tourRepository = Injector.CreateInstance<ITourRepository>();

        public TourService()
        {

        }

        public void Delete(int id)
        {
            _tourRepository.Delete(id);
        }

        public List<Tour> GetAll()
        {
            return _tourRepository.GetAll();
        }

        public Tour GetById(int id)
        {
            return _tourRepository.GetById(id);
        }

        public List<Tour> GetByUserId(int id)
        {
            return _tourRepository.GetAllByUserId(id);
        }
        public void Save(Tour tour)
        {
            _tourRepository.Save(tour);
        }

        public void Update(Tour tour)
        {
            _tourRepository.Update(tour);
        }

    }
}
