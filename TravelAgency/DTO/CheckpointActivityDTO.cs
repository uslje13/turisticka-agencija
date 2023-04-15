using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SOSTeam.TravelAgency.Domain.Models;

namespace TravelAgency.DTO
{

    public class CheckpointActivityDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public CheckpointType Type { get; set; }
        public CheckpointStatus Status { get; set; }
        public int CheckpointId { get; set; }

        public CheckpointActivityDTO(int id, string name, CheckpointType type, CheckpointStatus status, int checkpointId)
        {
            Id = id;
            Name = name;
            Type = type;
            Status = status;
            CheckpointId = checkpointId;
        }
    }
}
