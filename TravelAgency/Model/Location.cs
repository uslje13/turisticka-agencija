using TravelAgency.Serializer;

namespace TravelAgency.Model
{
    public class Location : ISerializable
    {
        private int _id;
        private string _country;
        private string _city;

        public Location()
        {
            _id = -1;
            _country = string.Empty;
            _city = string.Empty;
        }

        public Location(string country, string city)
        {
            _country = country;
            _city = city;
        }

        public int Id { get => _id; set => _id = value; }
        public string Country { get => _country; set => _country = value; }
        public string City { get => _city; set => _city = value; }

        public void FromCSV(string[] values)
        {
            _id = int.Parse(values[0]);
            _country = values[1];
            _city = values[2];
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                _id.ToString(),
                _country,
                _city
            };

            return csvValues;
        }
    }
}
