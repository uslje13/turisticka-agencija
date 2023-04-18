using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using SOSTeam.TravelAgency.Domain;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Domain.RepositoryInterfaces;
using SOSTeam.TravelAgency.WPF.ViewModels.Guest2;

namespace SOSTeam.TravelAgency.Application.Services
{
    public class AppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository = Injector.CreateInstance<IAppointmentRepository>();

        public AppointmentService() { }

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

        public Appointment GetById(int id)
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
                .FindAll(a => a.Date == DateOnly.FromDateTime(DateTime.Now));
        }

        public Appointment? GetActiveByUserId(int id)
        {
            return _appointmentRepository.GetAllByUserId(id).Find(a => a.Started && !a.Finished);
        }

        public List<Appointment> GetAllFinishedByUserId(int id)
        {
            return _appointmentRepository.GetAllByUserId(id).FindAll(a => a.Started && a.Finished);
        }

        public void StartAppointment(int id)
        {
            var appointment = _appointmentRepository.GetById(id);
            appointment.Started = true;
            _appointmentRepository.Update(appointment);
        }

        public void FinishAppointment(int id)
        {
            var appointment = _appointmentRepository.GetById(id);
            appointment.Finished = true;
            _appointmentRepository.Update(appointment);
        }

        //Zar ovo nije moglo u jednoj liniji da se pita posto moraju???
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
