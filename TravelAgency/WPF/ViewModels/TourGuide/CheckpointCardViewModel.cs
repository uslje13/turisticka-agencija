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
        public bool CanShowAttendance { get; set; }
        public bool Background { get; set; }

        public CheckpointCardViewModel()
        {
            CheckpointId = -1;
            ActivityId = -1;
            Name = string.Empty;

        }

        public CheckpointCardViewModel(int checkpointId, string name, CheckpointType type, CheckpointStatus status)
        {
            CheckpointId = checkpointId;
            Name = name;
            Type = type;
            Status = status;
        }

        public void SetCanShowAttendance()
        {
            if (Status == CheckpointStatus.NOT_STARTED)
            {
                CanShowAttendance = false;
            }
        }
    }
}
