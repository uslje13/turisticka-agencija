using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOSTeam.TravelAgency.Domain.Models;

namespace SOSTeam.TravelAgency.WPF.ViewModels.TourGuide
{
    public class CheckpointCardViewModel
    {
        public int CheckpointId { get; set; }
        public int ActivityId { get; set; }
        public string Name { get; set; }
        public CheckpointType Type { get; set; }
        public CheckpointStatus Status { get; set; }
        public bool CanCallGuests { get; set; }

        public CheckpointCardViewModel() { }

        public CheckpointCardViewModel(int checkpointId, string name, CheckpointType type, CheckpointStatus status, bool canCallGuests)
        {
            CheckpointId = checkpointId;
            Name = name;
            Type = type;
            Status = status;
            CanCallGuests = canCallGuests;
        }
    }
}
