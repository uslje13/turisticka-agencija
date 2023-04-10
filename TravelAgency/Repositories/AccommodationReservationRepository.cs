using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Domain.RepositoryInterfaces;
using SOSTeam.TravelAgency.Repositories.Serializer;

namespace SOSTeam.TravelAgency.Repositories
{
    public class AccommodationReservationRepository : IAccReservationRepository
    {
        private const string FilePath = "../../../Resources/Data/accommodationReservation.csv";
        private readonly Serializer<AccommodationReservation> _serializer;
        private List<AccommodationReservation> _accommodationReservations;

        public AccommodationReservationRepository() 
        { 
            _serializer = new Serializer<AccommodationReservation>();
            _accommodationReservations = new List<AccommodationReservation>();
        }

        public void Update(AccommodationReservation accommodationReservation)
        {

        }

        public AccommodationReservation GetById(int id)
        {
            _accommodationReservations = _serializer.FromCSV(FilePath);
            return _accommodationReservations.Find(a => a.Id == id) ?? throw new ArgumentException();
        }

        public List<AccommodationReservation> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public void Save(AccommodationReservation accommodationReservation)
        {
            accommodationReservation.Id = NextId();
            _accommodationReservations = _serializer.FromCSV(FilePath);
            _accommodationReservations.Add(accommodationReservation);
            _serializer.ToCSV(FilePath, _accommodationReservations);
        }

        public void Delete(int id)
        {
            _accommodationReservations = _serializer.FromCSV(FilePath);
            AccommodationReservation found = _accommodationReservations.Find(t => t.Id == id) ?? throw new ArgumentException();
            _accommodationReservations.Remove(found);
            _serializer.ToCSV(FilePath, _accommodationReservations);
        }

        public int NextId()
        {
            _accommodationReservations = _serializer.FromCSV(FilePath);
            if (_accommodationReservations.Count < 1)
            {
                return 1;
            }
            return _accommodationReservations.Max(l => l.Id) + 1;
        }
    }
}
