using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SOSTeam.TravelAgency.Repositories.Serializer;

namespace SOSTeam.TravelAgency.Domain.Models
{
    public class Reservation : ISerializable
    {
        public int Id { get; set; }
        public int AppointmentId { get; set; }
        public int TouristNum { get; set; }
        public float AverageAge { get; set; }
        public int UserId { get; set; }

        public Reservation() { }

        public Reservation(int touristNum,float averageAge, int userId, int appointmentId)
        {
            TouristNum = touristNum;
            AverageAge = averageAge;
            UserId = userId;
            AppointmentId = appointmentId;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            UserId = int.Parse(values[1]);
            AppointmentId = int.Parse(values[2]);
            TouristNum = int.Parse(values[3]);
            AverageAge = float.Parse(values[4]);
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                UserId.ToString(),
                AppointmentId.ToString(),
                TouristNum.ToString(),
                AverageAge.ToString()
            };

            return csvValues;
        }
    }
}
