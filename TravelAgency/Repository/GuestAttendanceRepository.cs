using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Model;
using TravelAgency.Serializer;

namespace TravelAgency.Repository
{
    public class GuestAttendanceRepository
    {
        private const string FilePath = "../../../Resources/Data/guestsAttendances.csv";

        private readonly Serializer<GuestAttendance> _serializer;

        private List<GuestAttendance> _guestsAttendances;

        public GuestAttendanceRepository()
        {
            _serializer = new Serializer<GuestAttendance>();
            _guestsAttendances = _serializer.FromCSV(FilePath);
        }

        public List<GuestAttendance> GetAll()
        {
            return _guestsAttendances;
        }

        public List<GuestAttendance> GetAllByActivityId(int id)
        {
            _guestsAttendances = _serializer.FromCSV(FilePath);
            return _guestsAttendances.FindAll(a => a.CheckpointActivityId == id);
        }

        public List<GuestAttendance> GetByUserId(int id)
        {
            _guestsAttendances = _serializer.FromCSV(FilePath);
            return _guestsAttendances.FindAll(a => a.UserId == id) ?? throw new ArgumentException();
        }

        public void Save(GuestAttendance guestAttendance)
        {
            guestAttendance.Id = NextId();
            _guestsAttendances = _serializer.FromCSV(FilePath);
            _guestsAttendances.Add(guestAttendance);
            _serializer.ToCSV(FilePath, _guestsAttendances);
        }

        public void SaveAll(List<GuestAttendance> guestsAttendances)
        {
            foreach(GuestAttendance guestAttendance in guestsAttendances)
            {
                Save(guestAttendance);
            }
        }

        public void Update(GuestAttendance guestAttendance)
        {
            _guestsAttendances = _serializer.FromCSV(FilePath);
            GuestAttendance current = _guestsAttendances.Find(a => a.Id == guestAttendance.Id) ?? throw new ArgumentException();
            int index = _guestsAttendances.IndexOf(current);
            _guestsAttendances.Remove(current);
            _guestsAttendances.Insert(index, guestAttendance);
            _serializer.ToCSV(FilePath, _guestsAttendances);
        }

        public int NextId()
        {
            _guestsAttendances = _serializer.FromCSV(FilePath);
            if (_guestsAttendances.Count < 1)
            {
                return 1;
            }
            return _guestsAttendances.Max(d => d.Id) + 1;
        }

    }
}
