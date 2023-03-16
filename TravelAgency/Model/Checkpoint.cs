using TravelAgency.Serializer;

public enum CheckpointType { UNKNOWN = 0, START = 1, END = 2, EXTRA = 3 };

namespace TravelAgency.Model
{
    public class Checkpoint : ISerializable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public CheckpointType Type { get; set; }
        public int TourId { get; set; }
        public Checkpoint()
        {
            Id = -1;
            Name = string.Empty;
            Active = false;
            Type = CheckpointType.UNKNOWN;
            TourId = -1;
        }

        public Checkpoint(int id, string name, bool active, CheckpointType type, int tourId)
        {
            Id = id;
            Name = name;
            Active = active;
            Type = type;
            TourId = tourId;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            Name = values[1];
            Active = bool.Parse(values[2]);

            if (values[3].Equals("START"))
            {
                Type = CheckpointType.START;
            }
            else if (values[3].Equals("END"))
            {
                Type = CheckpointType.END;
            }
            else if (values[3].Equals("EXTRA"))
            {
                Type = CheckpointType.EXTRA;
            }
            else
            {
                Type = CheckpointType.UNKNOWN;
            }

            TourId = int.Parse(values[4]);
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                Name,
                Active.ToString(),
                Type.ToString(),
                TourId.ToString()
            };
            return csvValues;
        }
    }
}
