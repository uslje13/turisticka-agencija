using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using SOSTeam.TravelAgency.Domain;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Domain.RepositoryInterfaces;

namespace SOSTeam.TravelAgency.Application.Services
{
    public class TourService
    {

        private readonly ITourRepository _tourRepository = Injector.CreateInstance<ITourRepository>();
        //Why Drasko?
        private readonly ILocationRepository _locationRepository = Injector.CreateInstance<ILocationRepository>();

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

        public void Save(Tour tour)
        {
            _tourRepository.Save(tour);
        }

        public void Update(Tour tour)
        {
            _tourRepository.Update(tour);
        }

        //Why Drasko?
        public Tour? FindTourById(int id)
        {
            foreach(var tour in _tourRepository.GetAll())
            {
                if(tour.Id == id) return tour;
            }
            return null;
        }

        public List<Tour> GetSameLocatedTours(int locationId)
        {
            List<Tour> tours = new List<Tour>();
            foreach(Tour tour in _tourRepository.GetAll())
            {
                if(tour.LocationId == locationId)
                {
                    tours.Add(tour);
                }
            }
            return tours;
        }
        //Why Drasko?
        public string GetTourCity(Tour tour)
        {
            foreach(Location location in _locationRepository.GetAll())
            {
                if(tour.LocationId == location.Id)
                {
                    return location.City;
                }
            }
            return string.Empty;
        }
        //Why Drasko?
        public string GetTourCountry(Tour tour)
        {
            foreach (Location location in _locationRepository.GetAll())
            {
                if (tour.LocationId == location.Id)
                {
                    return location.Country;
                }
            }
            return string.Empty;
        }
        //Why Drasko?
        public string GetTourName(int id)
        {
            foreach(Tour tour in _tourRepository.GetAll())
            {
                if(tour.Id == id)
                {
                    return tour.Name;
                }
            }
            return string.Empty;
        }

        public int NextId()
        {
            return _tourRepository.NextId();
        }

    }
}
