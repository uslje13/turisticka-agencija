using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.Domain.Models
{
    public class ForumCommentReport
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CommentId { get; set; }

        public ForumCommentReport(int id, int userId, int commentId)
        {
            Id = id;
            UserId = userId;
            CommentId = commentId;
        }

        public ForumCommentReport()
        {
            Id = -1;
            UserId = -1;
            CommentId = -1;
        }
    }
}
