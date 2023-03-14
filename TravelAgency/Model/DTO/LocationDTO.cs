using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Model;
using TravelAgency.Repository;


namespace TravelAgency.Model.DTO
{
    
    public class LocationDTO
    {
        
        private LocationRepository _locationRepository;
        public LocationDTO()
        {
            _locationRepository = new();
        }

        public List<string> GetFullNames() 
        {
             List<Location> locations = _locationRepository.GetAll();
            List<string> fullNames = new List<string>();
            foreach (Location location in locations)
            {
                string fullName = location.City + " (" + location.Country + ")";
                fullNames.Add(fullName);
            }

            return fullNames;
        }

        public string GetFullNameById(int locationId) 
        {
            Location location = _locationRepository.GetById(locationId);
            string fullName = location.City + " (" + location.Country + ")";

            return fullName;
        }
    }
}
