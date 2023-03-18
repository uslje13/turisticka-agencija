using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TravelAgency.Model;
using TravelAgency.Serializer;

namespace TravelAgency.Repository
{
    public class AppointmentRepository
    {
        private const string FilePath = "../../../Resources/Data/appointments.csv";

        private readonly Serializer<Appointment> _serializer;

        private List<Appointment> _appointments;

        public AppointmentRepository()
        {
            _serializer = new Serializer<Appointment>();
            _appointments = _serializer.FromCSV(FilePath);
        }

        public List<Appointment> GetAll()
        {
            return _appointments;
        }

        public List<Appointment> GetAppointmentsByTours(List<Tour> tours)
        {
            _appointments = _serializer.FromCSV(FilePath);
            List<Appointment> appointments = new List<Appointment>();

            foreach (Tour tour in tours)
            {
                foreach(Appointment appointment in _appointments)
                {
                    if(appointment.TourId == tour.Id)
                    {
                        appointments.Add(appointment);
                    }
                }
            }
            return appointments;
        }

        public Appointment GetById(int id) 
        {
            _appointments = _serializer.FromCSV(FilePath);
            return _appointments.Find(a => a.Id == id) ?? throw new ArgumentException();
        }


        public void Save(Appointment appointment)
        {
            appointment.Id = NextId();
            _appointments = _serializer.FromCSV(FilePath);
            _appointments.Add(appointment);
            _serializer.ToCSV(FilePath, _appointments);
        }

        public void SaveAll(ObservableCollection<Appointment> appointments)
        {
            foreach (Appointment appointment in appointments)
            {
                Save(appointment);
            }
        }

        public void Delete(Appointment appointment)
        {
            _appointments = _serializer.FromCSV(FilePath);
            Appointment founded = _appointments.Find(d => d.Id == appointment.Id) ?? throw new ArgumentException();
            _appointments.Remove(founded);
            _serializer.ToCSV(FilePath, _appointments);
        }

        public void DeleteByTourId(int id)
        {
            foreach (Appointment appointment in _appointments)
            {
                if (appointment.TourId == id)
                {
                    Delete(appointment);
                }
            }
        }

        public void Update(Appointment appointment)
        {
            _appointments = _serializer.FromCSV(FilePath);
            Appointment current = _appointments.Find(d => d.Id == appointment.Id) ?? throw new ArgumentException();
            int index = _appointments.IndexOf(current);
            _appointments.Remove(current);
            _appointments.Insert(index, appointment);
            _serializer.ToCSV(FilePath, _appointments);
        }

        public List<Appointment> GetTodayAppointments()
        {
            _appointments = _serializer.FromCSV(FilePath);
            DateOnly today = DateOnly.FromDateTime(DateTime.Today);
            return _appointments.FindAll(a => a.Date.Equals(today));
        }

        public int NextId()
        {
            _appointments = _serializer.FromCSV(FilePath);
            if (_appointments.Count < 1)
            {
                return 1;
            }
            return _appointments.Max(d => d.Id) + 1;
        }
    }
}
