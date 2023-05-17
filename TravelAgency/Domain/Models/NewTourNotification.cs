using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using SOSTeam.TravelAgency.Repositories.Serializer;

namespace SOSTeam.TravelAgency.Domain.Models
{
    public class NewTourNotification : ISerializable
    {
        public int Id { get; set; }
        public int AppointmentId { get; set; }
        public bool IsRead { get; set; }
        public int GuestId { get; set; }

        public NewTourNotification()
        {
            Id = -1;
            AppointmentId = -1;
            IsRead = false;
            GuestId = -1;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            AppointmentId = int.Parse(values[1]);
            IsRead = bool.Parse(values[2]);
            GuestId = int.Parse(values[3]);
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                AppointmentId.ToString(),
                IsRead.ToString(),
                GuestId.ToString()
            };

            return csvValues;
        }
    }
}
