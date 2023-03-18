using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TravelAgency.DTO
{

    public class CheckpointActivityDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public CheckpointType Type { get; set; }
        public bool Activated { get; set; }
        public int CheckpointId { get; set; }

        public CheckpointActivityDTO(int id, string name, CheckpointType type, bool activated, int checkpointId)
        {
            Id = id;
            Name = name;
            Type = type;
            Activated = activated;
            CheckpointId = checkpointId;
        }
    }
}
