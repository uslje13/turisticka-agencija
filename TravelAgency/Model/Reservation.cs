using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TravelAgency.Serializer;

namespace TravelAgency.Model
{
    public class Reservation : ISerializable
    {
        public int Id { get; set; }
        public int TourId { get; set; }
        public int AppointmentId { get; set; }
        public int Ocupancy { get; set; }
        public int UserId { get; set; }

        public Reservation() { }

        public Reservation(int tourId, int ocupancy, int userId, int appointmentId)
        {
            TourId = tourId;
            Ocupancy = ocupancy;
            UserId = userId;
            AppointmentId = appointmentId;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            UserId = int.Parse(values[1]);
            TourId = int.Parse(values[2]);
            AppointmentId = int.Parse(values[3]);
            Ocupancy = int.Parse(values[4]);
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                UserId.ToString(),
                TourId.ToString(),
                AppointmentId.ToString(),
                Ocupancy.ToString()
            };

            return csvValues;
        }
    }
}
