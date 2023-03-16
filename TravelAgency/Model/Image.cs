using TravelAgency.Serializer;

namespace TravelAgency.Model
{
    public class Image : ISerializable
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public bool Cover { get; set; }
        public int TourId { get; set; }

        public Image()
        {
            Id = -1;
            Url = string.Empty;
            Cover = false;
            TourId = -1;
        }

        public Image(int id, string url, bool cover, int tourId)
        {
            Id = id;
            Url = url;
            Cover = cover;
            TourId = tourId;
        }
        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            Url = values[1];
            Cover = bool.Parse(values[2]);
            TourId = int.Parse(values[3]);
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                Url,
                Cover.ToString(),
                TourId.ToString()
            };
            return csvValues;
        }
    }
}
