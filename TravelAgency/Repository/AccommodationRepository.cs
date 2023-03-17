using System;
using System.Collections.Generic;
using System.Linq;
using TravelAgency.Model;
using TravelAgency.Serializer;

namespace TravelAgency.Repository
{
    public class AccommodationRepository
    {
        private const string FilePath = "../../../Resources/Data/accommodation.csv";
        private readonly Serializer<Accommodation> _serializer;
        private List<Accommodation> _accommodations;

        public AccommodationRepository()
        {
            _serializer = new Serializer<Accommodation>();
            _accommodations = new List<Accommodation>();
        }

        public List<Accommodation> GetAll() 
        {
            return _serializer.FromCSV(FilePath);
        }

        public Accommodation Save(Accommodation accommodation) 
        {
            accommodation.Id = NextId();
            _accommodations = _serializer.FromCSV(FilePath);
            _accommodations.Add(accommodation);
            _serializer.ToCSV(FilePath,_accommodations);
            return accommodation;
        }

        public void Delete(Accommodation accommodation)
        {
            _accommodations = _serializer.FromCSV(FilePath);
            Accommodation found = _accommodations.Find(t => t.Id == accommodation.Id) ?? throw new ArgumentException();
            _accommodations.Remove(found);
            _serializer.ToCSV(FilePath, _accommodations);
        }

        public void DeleteById(int id)
        {
            _accommodations = _serializer.FromCSV(FilePath);
            Accommodation found = _accommodations.Find(t => t.Id == id) ?? throw new ArgumentException();
            _accommodations.Remove(found);
            _serializer.ToCSV(FilePath, _accommodations);
        }

        public Accommodation Update(Accommodation accommodation)
        {
            _accommodations = _serializer.FromCSV(FilePath);
            Accommodation current = _accommodations.Find(t => t.Id == accommodation.Id) ?? throw new ArgumentException();
            int index = _accommodations.IndexOf(current);
            _accommodations.Remove(current);
            _accommodations.Insert(index, accommodation);
            _serializer.ToCSV(FilePath, _accommodations);
            return accommodation;
        }
        public int NextId()
        {
            _accommodations = _serializer.FromCSV(FilePath);
            if (_accommodations.Count < 1)
            {
                return 1;
            }
            return _accommodations.Max(l => l.Id) + 1;
        }

    }
}

