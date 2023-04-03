using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Repositories.Serializer;

namespace SOSTeam.TravelAgency.Repositories
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

        public void SaveAll(ObservableCollection<Image> images)
        {
            foreach (Image image in images)
            {
                Save(image);
            }
        }

        public void Delete(Image image)
        {
            _images = _serializer.FromCSV(FilePath);
            Image founded = _images.Find(i => i.Id == image.Id) ?? throw new ArgumentException();
            _images.Remove(founded);
            _serializer.ToCSV(FilePath, _images);
        }

        public void DeleteByTourId(int id)
        {
            foreach (Image image in _images)
            {
                if (image.EntityId == id)
                {
                    Delete(image);
                }
            }
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
