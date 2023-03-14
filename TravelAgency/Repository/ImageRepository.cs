using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Model;
using TravelAgency.Serializer;

namespace TravelAgency.Repository
{
    public class ImageRepository
    {
        private const string FilePath = "../../../Resources/Data/images.csv";
        private readonly Serializer<Image> _serializer;
        private List<Image> _images;

        public ImageRepository()
        {
            _serializer = new Serializer<Image>();
            _images = _serializer.FromCSV(FilePath);
        }

        public List<Image> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public void Save(Image image)
        {
            image.Id = NextId();
            _images = _serializer.FromCSV(FilePath);
            _images.Add(image);
            _serializer.ToCSV(FilePath, _images);
        }

        public void Delete(Image image)
        {
            _images = _serializer.FromCSV(FilePath);
            Image founded = _images.Find(i => i.Id == image.Id) ?? throw new ArgumentException();
            _images.Remove(image);
            _serializer.ToCSV(FilePath, _images);
        }

        public int NextId()
        {
            _images = _serializer.FromCSV(FilePath);
            if (_images.Count < 1)
            {
                return 1;
            }
            return _images.Max(l => l.Id) + 1;
        }

    }
}
