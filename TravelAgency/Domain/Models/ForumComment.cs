using SOSTeam.TravelAgency.Repositories.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.Domain.Models
{
    public class ForumComment : ISerializable
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ForumId { get; set; }
        public string Comment { get; set; }
        public bool WasOnLocation { get; set; }
        public Roles UserType { get; set; }

        public ForumComment()
        {
            Id = -1;
            UserId = -1;
            ForumId = -1;
            Comment = "";
            WasOnLocation = false;
            UserType = Roles.GUEST1;
        }
        public ForumComment(int id, int userId, int forumId, string comment, bool wasOnLocation, Roles userType)
        {
            Id = id;
            UserId = userId;
            ForumId = forumId;
            Comment = comment;
            WasOnLocation = wasOnLocation;
            UserType = userType;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), UserId.ToString(), ForumId.ToString(), Comment, WasOnLocation.ToString(), UserType.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            int i = 0;
            Id = Convert.ToInt32(values[i++]);
            UserId = Convert.ToInt32(values[i++]);
            ForumId = Convert.ToInt32(values[i++]);
            Comment = values[i++];
            if (values[i++].Equals("False"))
                WasOnLocation = false;
            else
                WasOnLocation = true;
            if (values[i++].Equals("OWNER"))
                UserType = Roles.OWNER;
            else if (values[i++].Equals("TOURISTGUIDE"))
                UserType = Roles.TOURISTGUIDE;
            else if (values[i++].Equals("GUEST1"))
                UserType = Roles.GUEST1;
            else if (values[i++].Equals("GUEST2"))
                UserType = Roles.GUEST2;
        }
    }
}
