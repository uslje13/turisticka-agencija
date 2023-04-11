using System;
using SOSTeam.TravelAgency.Repositories.Serializer;

namespace SOSTeam.TravelAgency.Domain.Models
{
    public class Appointment : ISerializable
    {
        public int Id { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly Time { get; set; }
        public int Occupancy { get; set; }
        public bool Started { get; set; }
        public bool Finished { get; set; }
        public int TourId { get; set; }
        public int UserId { get; set; }

        public Appointment()
        {
            Id = -1;
            Date = DateOnly.MinValue;
            Time = TimeOnly.MinValue;
            Occupancy = 0;
            Started = false;
            Finished = false;
            TourId = -1;
            UserId = -1;
        }

        public Appointment(int id, DateOnly date, TimeOnly time, int occupancy, bool started, bool finished, int tourId, int userId)
        {
            Id = id;
            Date = date;
            Time = time;
            Occupancy = occupancy;
            Started = started;
            Finished = finished;
            TourId = tourId;
            UserId = userId;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            Date = DateOnly.ParseExact(values[1],"dd/MM/yyyy");
            Time = TimeOnly.Parse(values[2]);
            Occupancy = int.Parse(values[3]);
            Started = bool.Parse(values[4]);
            Finished = bool.Parse(values[5]);
            TourId = int.Parse(values[6]);
            UserId = int.Parse(values[7]);
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                Date.ToString("dd/MM/yyyy"),
                Time.ToString(),
                Occupancy.ToString(),
                Started.ToString(),
                Finished.ToString(),
                TourId.ToString(),
                UserId.ToString()
            };
            return csvValues;
        }
    }
}
