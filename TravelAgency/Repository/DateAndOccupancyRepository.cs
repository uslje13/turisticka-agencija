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
    public class DateAndOccupancyRepository
    {
        private const string FilePath = "../../../Resources/Data/datesAndOccupancies.csv";
        private readonly Serializer<DateAndOccupancy> _serializer;
        private List<DateAndOccupancy> _datesAndOccupancies;

        public DateAndOccupancyRepository()
        {
            _serializer = new Serializer<DateAndOccupancy>();
            _datesAndOccupancies = _serializer.FromCSV(FilePath);
        }

        public List<DateAndOccupancy> GetAll()
        {
            return _datesAndOccupancies;
        }

        public void Save(DateAndOccupancy dateAndOccupancy)
        {
            dateAndOccupancy.Id = NextId();
            _datesAndOccupancies = _serializer.FromCSV(FilePath);
            _datesAndOccupancies.Add(dateAndOccupancy);
            _serializer.ToCSV(FilePath, _datesAndOccupancies);
        }

        public void SaveAll(ObservableCollection<DateAndOccupancy> datesAndOccupancies) 
        {
            foreach(DateAndOccupancy dateAndOccupancies in datesAndOccupancies)
            {
                Save(dateAndOccupancies);
            }
        }

        public void Delete(DateAndOccupancy dateAndOccupancy)
        {
            _datesAndOccupancies = _serializer.FromCSV(FilePath);
            DateAndOccupancy founded = _datesAndOccupancies.Find(d => d.Id == dateAndOccupancy.Id) ?? throw new ArgumentException();
            _datesAndOccupancies.Remove(founded);
            _serializer.ToCSV(FilePath, _datesAndOccupancies);
        }

        public void DeleteByTourId(int id)
        {
            foreach (DateAndOccupancy dateAndOccupancy in _datesAndOccupancies)
            {
                if (dateAndOccupancy.TourId == id)
                {
                    Delete(dateAndOccupancy);
                }
            }
        }

        public void Update(DateAndOccupancy dateAndOccupancy)
        {
            _datesAndOccupancies = _serializer.FromCSV(FilePath);
            DateAndOccupancy current = _datesAndOccupancies.Find(d => d.Id == dateAndOccupancy.Id) ?? throw new ArgumentException();
            int index = _datesAndOccupancies.IndexOf(current);
            _datesAndOccupancies.Insert(index, dateAndOccupancy);
            _serializer.ToCSV(FilePath, _datesAndOccupancies);
        }

        public int NextId()
        {
            _datesAndOccupancies = _serializer.FromCSV(FilePath);
            if(_datesAndOccupancies.Count < 1)
            {
                return 1;
            }
            return _datesAndOccupancies.Max(d => d.Id) + 1;
        }

    }
}
