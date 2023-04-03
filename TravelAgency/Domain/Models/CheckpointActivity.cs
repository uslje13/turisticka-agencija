using SOSTeam.TravelAgency.Repositories.Serializer;

namespace SOSTeam.TravelAgency.Domain.Models
{
    public class CheckpointActivity : ISerializable
    {
        public int Id { get; set; }
        public int AppointmentId { get; set; }
        public int CheckpointId { get; set; }
        public bool Activated { get; set; }

        public CheckpointActivity()
        {
            Id = -1;
            AppointmentId = -1;
            CheckpointId = -1;
            Activated = false;
        }

        public CheckpointActivity(int id, int appointmentId, int checkpointId, bool checkpointActive)
        {
            Id = id;
            AppointmentId = appointmentId;
            CheckpointId = checkpointId;
            Activated = checkpointActive;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            AppointmentId = int.Parse(values[1]);
            CheckpointId = int.Parse(values[2]);
            Activated = bool.Parse(values[3]);
        }
        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                AppointmentId.ToString(),
                CheckpointId.ToString(),
                Activated.ToString()
            };
            return csvValues;
        }
    }
}
