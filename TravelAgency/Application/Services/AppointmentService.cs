using System;
using System.Collections.Generic;
using SOSTeam.TravelAgency.Domain;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Domain.RepositoryInterfaces;

namespace SOSTeam.TravelAgency.Application.Services
{
    public class AppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly TourService _tourService;

        public AppointmentService()
        {
            _appointmentRepository = Injector.CreateInstance<IAppointmentRepository>();
            _tourService = new TourService();
        }

        public void Delete(int id)
        {
            _appointmentRepository.Delete(id);
        }

        public List<Appointment> GetAll()
        {
            return _appointmentRepository.GetAll();
        }

        public List<Appointment> GetAllByUserId(int userId)
        {
            return _appointmentRepository.GetAllByUserId(userId);
        }

        public Appointment? GetById(int id)
        {
            return _appointmentRepository.GetById(id);
        }

        public void Save(Appointment appointment)
        {
            _appointmentRepository.Save(appointment);
        }

        public void SaveAll(List<Appointment> appointments)
        {
            _appointmentRepository.SaveAll(appointments);
        }

        public void Update(Appointment appointment)
        {
            _appointmentRepository.Update(appointment);
        }
        public List<Appointment> GetTodayAppointmentsByUserId(int id)
        {
            return _appointmentRepository.GetAllByUserId(id)
                .FindAll(a => a.Start.Date == DateTime.Today);
        }

        public Appointment? GetActiveByUserId(int id)
        {
            return _appointmentRepository.GetAllByUserId(id).Find(a => a.IsActive());
        }

        public List<Appointment> GetAllFinishedByUserId(int id)
        {
            return _appointmentRepository.GetAllByUserId(id).FindAll(a => a.IsFinished());
        }

        public void StartAppointment(int id)
        {
            var appointment = _appointmentRepository.GetById(id);
            if (appointment == null) { return; }
            appointment.Started = true;
            _appointmentRepository.Update(appointment);
        }

        public void FinishAppointment(int id)
        {
            var appointment = _appointmentRepository.GetById(id);
            if(appointment == null) { return; }
            appointment.Finished = true;
            _appointmentRepository.Update(appointment);
        }

        public void SetExpiredAppointments(int userId)
        {
            foreach (var appointment in GetAllByUserId(userId))
            {
                var tour = _tourService.GetById(appointment.TourId);
                if(tour == null) { return; }

                if (appointment.IsExpired(tour.Duration))
                {
                    FinishAppointment(appointment.Id);
                }
            }
        }

        //Zar ovo nije moglo u jednoj liniji da se pita posto moraju oba uslova biti ispunjena???
        public bool CheckAvailableAppointments(Tour tour)
        {
            foreach(Appointment appointment in _appointmentRepository.GetAll()) 
            {
                if(tour.Id == appointment.TourId)
                {
                    if(appointment.Occupancy < tour.MaxNumOfGuests)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
