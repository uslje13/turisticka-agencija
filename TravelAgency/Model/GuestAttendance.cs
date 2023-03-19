using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Serializer;

public enum GuestPresence { UNKNOWN = 0, YES = 1, NO = 2 };

namespace TravelAgency.Model
{
    public class GuestAttendance : ISerializable
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CheckpointActivityId { get; set; }
        public GuestPresence Presence { get; set; }
        public string Message { get; set; }

        public GuestAttendance()
        {
            Id = -1;
            UserId = -1;
            CheckpointActivityId = -1;
            Presence = GuestPresence.UNKNOWN;
            Message = string.Empty;
        }

        public GuestAttendance(int id, int userId, int checkpointActivityId, GuestPresence presence, string message)
        {
            Id = id;
            UserId = userId;
            CheckpointActivityId = checkpointActivityId;
            Presence = presence;
            Message = message;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            UserId = int.Parse(values[1]);
            CheckpointActivityId = int.Parse(values[2]);
            if (values[3].Equals("YES")) 
            {
                Presence = GuestPresence.YES;
            } 
            else if (values[3].Equals("NO"))
            {
                Presence = GuestPresence.NO;
            }
            else
            {
                Presence = GuestPresence.UNKNOWN;
            }
            Message = values[4];
        }

        public string[] ToCSV()
        {
            string[] csvValues = 
            { 
                Id.ToString(),
                UserId.ToString(),
                CheckpointActivityId.ToString(),
                Presence.ToString(),
                Message
            };
            return csvValues;
        }
    }
}
