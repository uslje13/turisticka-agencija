using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Serializer;

namespace TravelAgency.Model
{
    public class Image : ISerializable
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public bool Cover { get; set; }

        public Image()
        {
            Id = -1;
            Url = string.Empty;
            Cover = false;
        }

        public Image(int id, string url, bool cover)
        {
            Id = id;
            Url = url;
            Cover = cover;
        }
        public void FromCSV(string[] values)
        {
            throw new NotImplementedException();
        }

        public string[] ToCSV()
        {
            throw new NotImplementedException();
        }

    }
}
