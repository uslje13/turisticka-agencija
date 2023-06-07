using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.Domain.Models
{
    public class ForumComment
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
            UserType = Roles.GOST1;
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
    }
}
