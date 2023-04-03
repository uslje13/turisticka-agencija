using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Repositories.Serializer;

namespace SOSTeam.TravelAgency.Repositories
{
    internal class ReservationRepository
    {
        private const string FilePath = "../../../Resources/Data/reservations.csv";
        private readonly Serializer<Reservation> _serializer;
        private List<Reservation> _resevations;

        public ReservationRepository()
        {
            _serializer = new Serializer<Reservation>();
            _resevations = new List<Reservation>();
        }

        public List<Reservation> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public Reservation Save(Reservation reservation)
        {
            reservation.Id = NextId();
            _resevations = _serializer.FromCSV(FilePath);
            _resevations.Add(reservation);
            _serializer.ToCSV(FilePath, _resevations);
            return reservation;
        }

        public void Delete(Reservation reservation)
        {
            _resevations = _serializer.FromCSV(FilePath);
            Reservation founded = _resevations.Find(r => r.Id == reservation.Id) ?? throw new ArgumentException();
            _resevations.Remove(founded);
            _serializer.ToCSV(FilePath, _resevations);
        }

        public Reservation Update(Reservation reservation)
        {
            _resevations = _serializer.FromCSV(FilePath);
            Reservation current = _resevations.Find(r => r.Id == reservation.Id) ?? throw new ArgumentException();
            int index = _resevations.IndexOf(current);
            _resevations.Remove(current);
            _resevations.Insert(index, reservation);
            _serializer.ToCSV(FilePath, _resevations);
            return reservation;
        }

        public int GetId(Reservation reservation)
        {
            return reservation.Id;
        }

        public int NextId()
        {
            _resevations = _serializer.FromCSV(FilePath);
            if (_resevations.Count < 1)
            {
                return 1;
            }
            return _resevations.Max(l => l.Id) + 1;
        }
    }
}
