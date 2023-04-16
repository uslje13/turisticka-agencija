using System;
using SOSTeam.TravelAgency.Repositories.Serializer;

namespace SOSTeam.TravelAgency.Domain.Models
{
    public class Image : ISerializable
    {
        public enum ImageType
        {
            ACCOMMODATION,TOUR,RESERVATION,NOTYPE,GUEST2
        }
        public int Id { get; set; }
        public string Path { get; set; }
        public bool Cover { get; set; }
        public int EntityId { get; set; }
        public ImageType Type { get; set; }

        public Image()
        {
            Id = -1;
            Path = string.Empty;
            Cover = false;
            EntityId = -1;
            Type = ImageType.NOTYPE;
        }

        public Image(string path, bool cover, int entityId,ImageType type)
        {
            Path = path;
            Cover = cover;
            EntityId = entityId;
            Type = type;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            Path = values[1];
            Cover = bool.Parse(values[2]);
            EntityId = int.Parse(values[3]);
            Type = (ImageType)Convert.ToInt32(values[4]);
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                Path,
                Cover.ToString(),
                EntityId.ToString(),
                ((int)Type).ToString()
            };
            return csvValues;
        }
    }
}
