using SOSTeam.TravelAgency.Repositories.Serializer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.Domain.Models
{
    public class AccommodationRenovation : ISerializable
    {
        public int Id { get; set; }
        public int AccommodationId { get; set; }
        public DateTime FirstDay { get; set; }
        public DateTime LastDay { get; set; }

        public bool IsFinished { get; set; }


        public AccommodationRenovation()
        {
            Id = -1;
            AccommodationId = -1;
            FirstDay = DateTime.MinValue;
            LastDay = DateTime.MinValue;
            IsFinished = false;
        }

        public AccommodationRenovation(int accommodationId, DateTime firstDay, DateTime lastDay)
        {
            Id = -1;
            AccommodationId = accommodationId;
            FirstDay = firstDay;
            LastDay = lastDay;
            IsFinished = false;
        }

        public string[] ToCSV()
        {
            string[] csvValues = {
            Id.ToString(),
            AccommodationId.ToString(),
            FirstDay.ToString("dd-MM-yyyy"),
            LastDay.ToString("dd-MM-yyyy"),
            IsFinished == true ? "1" : "0"
        };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            int i = 0;
            Id = Convert.ToInt32(values[i++]);
            AccommodationId = Convert.ToInt32(values[i++]);
            FirstDay = DateTime.ParseExact(values[i++], "dd-MM-yyyy", CultureInfo.InvariantCulture);
            LastDay = DateTime.ParseExact(values[i++], "dd-MM-yyyy", CultureInfo.InvariantCulture);
            IsFinished = values[i++].Equals("1");
        }
    }
}
