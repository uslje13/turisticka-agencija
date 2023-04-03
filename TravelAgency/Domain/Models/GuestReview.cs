using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOSTeam.TravelAgency.Repositories.Serializer;

namespace SOSTeam.TravelAgency.Domain.Models
{
    public class GuestReview : ISerializable
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public int GuestId { get; set; }
        public int CleanlinessGrade { get; set; }
        public int RespectGrade { get; set; }
        public string Comment { get; set; }

        public GuestReview()
        {
            Id = -1;
            OwnerId = -1;
            GuestId = -1;
            CleanlinessGrade = -1;
            RespectGrade = -1;
            Comment = string.Empty;
        }

        public GuestReview(int ownerId, int guestId, int cleanlinessGrade, int respectGrade, string comment)
        {
            Id = -1;
            OwnerId = ownerId;
            GuestId = guestId;
            CleanlinessGrade = cleanlinessGrade;
            RespectGrade = respectGrade;
            Comment = comment;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { 
                Id.ToString(),
                OwnerId.ToString(),
                GuestId.ToString(),
                CleanlinessGrade.ToString(),
                RespectGrade.ToString(),
                Comment };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            int i = 0;
            Id = Convert.ToInt32(values[i++]);
            OwnerId = Convert.ToInt32(values[i++]);
            GuestId = Convert.ToInt32(values[i++]);
            CleanlinessGrade = Convert.ToInt32(values[i++]);
            RespectGrade = Convert.ToInt32(values[i++]);
            Comment = values[i++];
        }
    }
}
