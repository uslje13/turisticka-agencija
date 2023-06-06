using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.Domain.Models
{
    public class Forum
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int LocationId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsOpen { get; set; }


        public Forum()
        {
            Id = -1;
            UserId = -1;
            LocationId = -1;
            Title = "";
            Description = "";
            IsOpen = false;
        }
        public Forum(int id, int userId, int locationId, string title, string description, bool isOpen)
        {
            Id = id;
            UserId = userId;
            LocationId = locationId;
            Title = title;
            Description = description;
            IsOpen = isOpen;
        }
    }
}
