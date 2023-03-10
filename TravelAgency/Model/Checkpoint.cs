using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Serializer;

public enum CheckpointType { UNKNOWN = 0, START = 1, END = 2, EXTRA = 3 };

namespace TravelAgency.Model
{
    public class Checkpoint : ISerializable
    {
        private int _id;
        private string _name;
        private bool _active;
        private CheckpointType _type;
        private int _tourId;
        public Checkpoint() 
        {
            _id = -1;
            _name = string.Empty;
            _active = false;
            _type = CheckpointType.UNKNOWN;
            _tourId = -1;
        }

        public Checkpoint(int id, string name, bool active, CheckpointType type, int tourId)
        {
            _id = id;
            _name = name;
            _active = active;
            _type = type;
            _tourId = tourId;
        }

        public int Id { get => _id; set => _id = value; }
        public string Name { get => _name; set => _name = value; }
        public bool Active { get => _active; set => _active = value; }
        public CheckpointType Type { get => _type; set => _type = value; }
        public int TourId { get => _tourId; set => _tourId = value; }

        public void FromCSV(string[] values)
        {
            _id = int.Parse(values[0]);
            _name = values[1];
            _active = bool.Parse(values[2]);
            if (values[3].Equals("START"))
            {
                _type = CheckpointType.START;
            }
            else if (values[3].Equals("END"))
            {
                _type = CheckpointType.END;
            }
            else if (values[3].Equals("EXTRA"))
            {
                _type = CheckpointType.EXTRA;
            }
            else
            {
                _type = CheckpointType.UNKNOWN;
            }
            _tourId = int.Parse(values[4]);
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                _id.ToString(),
                _name,
                _active.ToString(),
                _type.ToString(),
                _tourId.ToString()
            };
            return csvValues;
        }
    }
}
