using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Serializer;

namespace TravelAgency.Model
{
    public class Accommodation : ISerializable
    {
        public enum AccommodationType
        {
            APARTMENT, HOUSE, HUT, NOTYPE
        }
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public string Name { get; set; }
        public string Town { get; set; }
        public string Country { get; set; }
        public AccommodationType Type { get; set; }
        public int MaxGuests { get; set; }
        public int MinDaysStay { get; set; }
        public int MinDaysForCancelation { get; set; }
        //TODO: Url slika

        public Accommodation()
        {
            Id = -1;
            Name = string.Empty;
            Town = string.Empty;
            Country = string.Empty;
            Type = AccommodationType.NOTYPE;
            MaxGuests = 0;
            MinDaysStay = 0;
            MinDaysForCancelation = 0;
        }
        public Accommodation(int id, string name, AccommodationType type, string town, string country, int maxGuests, int minDaysStay, int minDaysForCancelation = 1)
        {
            Id = id;
            Name = name;
            Town = town;
            Country = country;
            Type = type;
            MaxGuests = maxGuests;
            MinDaysStay = minDaysStay;
            MinDaysForCancelation = minDaysForCancelation;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Name, Town, Country, Type.ToString(), MaxGuests.ToString(), MinDaysStay.ToString(), MinDaysForCancelation.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            int i = 0;
            Id = Convert.ToInt32(values[i++]);
            Name = values[i++];
            Town = values[i++];
            Country = values[i++];
            Type = (AccommodationType)Convert.ToInt32(values[i++]);
            MaxGuests = Convert.ToInt32(values[i++]);
            MinDaysStay = Convert.ToInt32(values[i++]);
            MinDaysForCancelation = Convert.ToInt32(values[i++]);
        }
    }
}
