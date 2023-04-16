using SOSTeam.TravelAgency.Repositories.Serializer;

namespace SOSTeam.TravelAgency.Domain.Models
{
    public enum CheckpointStatus { NOT_STARTED = 0, ACTIVE = 1, FINISHED = 2 };
    public class CheckpointActivity : ISerializable
    {
        public int Id { get; set; }
        public int AppointmentId { get; set; }
        public int CheckpointId { get; set; }
        public CheckpointStatus Status { get; set; }

        public CheckpointActivity()
        {
            Id = -1;
            AppointmentId = -1;
            CheckpointId = -1;
            Status = CheckpointStatus.NOT_STARTED;
        }

        public CheckpointActivity(int id, int appointmentId, int checkpointId, CheckpointStatus status)
        {
            Id = id;
            AppointmentId = appointmentId;
            CheckpointId = checkpointId;
            Status = status;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            AppointmentId = int.Parse(values[1]);
            CheckpointId = int.Parse(values[2]);
            if (values[3].Equals("NOT_STARTED"))
            {
                Status = CheckpointStatus.NOT_STARTED;
            }
            else if (values[3].Equals("ACTIVE"))
            {
                Status = CheckpointStatus.ACTIVE;
            }
            else if (values[3].Equals("FINISHED"))
            {
                Status = CheckpointStatus.FINISHED;
            }
        }
        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                AppointmentId.ToString(),
                CheckpointId.ToString(),
                Status.ToString(),
            };
            return csvValues;
        }
    }
}
