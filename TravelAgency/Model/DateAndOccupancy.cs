using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Serializer;

namespace TravelAgency.Model
{
    public class DateAndOccupancy : ISerializable
    {
        //id|date|int|id ---> .csv
        public int Id { get; set; }
        public DateTime Start { get; set; }
        public int Occupancy { get; set; }
        public int TourId { get; set; }

        public DateAndOccupancy()
        {

        }

        public DateAndOccupancy(int id, DateTime start, int occupancy)
        {
            Id = id;
            Start = start;
            Occupancy = occupancy;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            Start = DateTime.Parse(values[1]);
            Occupancy = int.Parse(values[2]);
            TourId = int.Parse(values[3]);
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                Start.ToString(),
                Occupancy.ToString(),
                TourId.ToString()
            };
            return csvValues;
        }
    }
}
