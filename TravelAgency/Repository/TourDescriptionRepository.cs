using System;
using System.Collections.Generic;
using System.Linq;
using TravelAgency.Model;
using TravelAgency.Serializer;

namespace TravelAgency.Repository
{
    public class TourDescriptionRepository
    {
        private const string FilePath = "../../../Resources/Data/tourDescriptions.csv";

        private readonly Serializer<Comment> _serializer;

        private List<Comment> _descriptions;

        public TourDescriptionRepository()
        {
            _serializer = new Serializer<Comment>();
            _descriptions = _serializer.FromCSV(FilePath);
        }

        public void Save(Comment comment)
        {
            comment.Id = NextId();
            _descriptions = _serializer.FromCSV(FilePath);
            _descriptions.Add(comment);
            _serializer.ToCSV(FilePath, _descriptions);
            //return comment;
        }

        public Comment FingById(int id)
        {
            Comment founded = _descriptions.Find(c => c.Id == id) ?? throw new ArgumentException();
            return founded;

        }

        public int NextId()
        {
            _descriptions = _serializer.FromCSV(FilePath);
            if (_descriptions.Count < 1)
            {
                return 1;
            }
            return _descriptions.Max(c => c.Id) + 1;
        }

    }
}
