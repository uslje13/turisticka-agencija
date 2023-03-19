using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TravelAgency.Repository;
using TravelAgency.Serializer;

namespace TravelAgency.DTO
{
    public class LocAccommodationDTO
    {
        public enum AccommType
        {
            APARTMENT, HOUSE, HUT, NOTYPE
        }

        public int AccommodationId { get; set; }
        public string AccommodationName { get; set; }
        public string LocationCity { get; set; }
        public string LocationCountry { get; set; }
        public AccommType AccommodationType { get; set; }
        public int AccommodationMaxGuests { get; set; }
        public int AccommodationMinDaysStay { get; set; }
        public int GuestNumber { get; set; }


        public LocAccommodationDTO(int id, string name, string city, string country, AccommType type, int guests, int days, int guestNumber)

        {
            AccommodationId = id;
            AccommodationName = name;
            LocationCity = city;
            LocationCountry = country;
            AccommodationType = type;
            AccommodationMaxGuests = guests;
            AccommodationMinDaysStay = days;
            GuestNumber = guestNumber;
        }

        public LocAccommodationDTO(string name, string city, string country, AccommType type, int guestNumber, int days)
        {
            AccommodationName = name;
            LocationCity = city;
            LocationCountry = country;
            AccommodationType = type;
            AccommodationMinDaysStay = days;
            GuestNumber = guestNumber;
        }
    }
}
