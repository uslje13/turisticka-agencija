using System;
using TravelAgency.Serializer;

namespace TravelAgency.Model
{
    public class Tour : ISerializable
    {
        private int _id;        //tour _id
        private string _name;
        private int _locationId;     //locationId
        private string _description;
        private string _language;
        private int _maxNumOfGuests;
        private int _duration;      //in hours
        private int _userId;

        public Tour()
        {
            _id = -1;
            _name = string.Empty;
            _locationId = -1;
            _description = string.Empty;
            _language = string.Empty;
            _maxNumOfGuests = 0;
            _duration = 0;
            _userId = -1;
        }
        public Tour(string name, int locationId, string description, string language, int maxNumOfGuests, int duration, int userId)
        {
            _name = name;
            _locationId = locationId;
            _description = description;
            _language = language;
            _maxNumOfGuests = maxNumOfGuests;
            _duration = duration;
            _userId = userId;
        }

        public int Id { get => _id; set => _id = value; }
        public string Name { get => _name; set => _name = value; }
        public int LocationId { get => _locationId; set => _locationId = value; }
        public string Description { get => _description; set => _description = value; }
        public string Language { get => _language; set => _language = value; }
        public int MaxNumOfGuests { get => _maxNumOfGuests; set => _maxNumOfGuests = value; }
        public int Duration { get => _duration; set => _duration = value; }
        public int UserId { get => _userId; set => _userId = value; }

        public void FromCSV(string[] values)
        {
            _id = int.Parse(values[0]);
            _name = values[1];
            _locationId = int.Parse(values[2]);
            _description = values[3];
            _language = values[4];
            _maxNumOfGuests = int.Parse(values[5]);
            _duration = int.Parse(values[6]);
            _userId = int.Parse(values[7]);
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                _id.ToString(),
                _name,
                _locationId.ToString(),
                _description,
                _language,
                _maxNumOfGuests.ToString(),
                _duration.ToString(),
                _userId.ToString()
            };

            return csvValues;
        }
    }
}
