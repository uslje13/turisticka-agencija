using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelAgency.DTO
{
    public class GuestAttendanceDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public GuestPresence Presence { get; set; }

        public GuestAttendanceDTO(int id, string name, GuestPresence presence)
        {
            Id = id;
            Username = name;
            Presence = presence;
        }
    }
}
