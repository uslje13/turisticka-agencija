using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TravelAgency.Model;
using TravelAgency.Repository;
using TravelAgency.Serializer;

namespace TravelAgency.Model
{
    public class AccommodationDTO
    {
        public enum AccommType
        {
            APARTMENT, HOUSE, HUT, NOTYPE
        }

        //public int AccommodationDTOId { get; set; }
        //public int AccommodationId { get; set; }
        public string AccommodationName { get; set; }
        //public int LocationId { get; set; }
        public string LocationCity { get; set; }
        public string LocationCountry { get; set; }
        public AccommType AccommodationType { get; set; }
        public int AccommodationMaxGuests { get; set; }
        public int AccommodationMinDaysStay { get; set; }
        public List<Accommodation> accommodations { get; set; }
        public List<Location> locations { get; set; }
        public ObservableCollection<AccommodationDTO> AccommDTOsCollection { get; set; }
        public AccommodationRepository accommodationRepository { get; set; }
        public LocationRepository locationRepository { get; set; }

        public AccommodationDTO(string name, string city, string country, AccommType type, int guests, int days)
        {
            AccommodationName = name;
            LocationCity = city;
            LocationCountry = country;
            AccommodationType = type;
            AccommodationMaxGuests = guests;
            AccommodationMinDaysStay = days;
        }

        public AccommodationDTO() 
        {
            AccommDTOsCollection = new ObservableCollection<AccommodationDTO>();
            accommodationRepository = new AccommodationRepository();
            locationRepository = new LocationRepository();
            
            accommodations = accommodationRepository.GetAll();
            locations = locationRepository.GetAll();

            CreateAllDTOForms();
        }

        private void CreateAllDTOForms()
        {
            foreach(var accommodation in accommodations)
            {
                foreach(var location in locations)
                {
                    if(accommodation.LocationId == location.Id)
                    {
                        AccommodationDTO dto = CreateDTOForm(accommodation, location);
                        AccommDTOsCollection.Add(dto);
                    }
                }
            }
        }

        private AccommodationDTO CreateDTOForm(Accommodation acc, Location loc)
        {
            AccommodationDTO dto = new AccommodationDTO(acc.Name, loc.City, loc.Country, FindAccommodationType(acc), acc.MaxGuests, acc.MinDaysStay);
            //dto.AccommodationDTOId = NextId();
            //dto.AccommodationId = acc.Id;
            //dto.LocationId = loc.Id;
            
            return dto;
        }

        private AccommType FindAccommodationType(Accommodation acc)
        {
            if (acc.Type == Accommodation.AccommodationType.APARTMENT)
                return AccommType.APARTMENT;
            else if (acc.Type == Accommodation.AccommodationType.HOUSE)
                return AccommType.HOUSE;
            else if (acc.Type == Accommodation.AccommodationType.HUT)
                return AccommType.HUT;
            else
                return AccommType.NOTYPE;
        }

        /*
        private int NextId()
        {
            if (AccommDTOsCollection.Count < 1)
            {
                return 1;
            }
            return AccommDTOsCollection.Max(l => l.AccommodationDTOId) + 1;
        }
        */
        public List<AccommodationDTO> GetAll()
        {
            return AccommDTOsCollection.ToList(); 
        }
    }
}
