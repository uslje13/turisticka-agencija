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

        public List<Appointment> GetTodayAppointmentsByUserId(int id)
        {
            List<Appointment> appointments = new List<Appointment>();

            foreach (var userAppointment in _appointmentRepository.GetAllByUserId(id))
            {
                foreach (var todayAppointment in _appointmentRepository.GetTodayAppointments())
                {
                    if (userAppointment.Id == todayAppointment.Id)
                    {
                        appointments.Add(userAppointment);
                    }
                }
            }
            return appointments;
        }

        public Appointment GetActiveByUserId(int id)
        {
            return _appointmentRepository.GetAllByUserId(id).Find(a => a.Started == true && a.Finished == false);
        }

        public List<Appointment> GetAllByTours(List<Tour> tours)
        {
            return _appointmentRepository.GetAllByTours(tours);
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

        public List<Appointment> GetAllFinishedByUserId(int id)
        {
            return _appointmentRepository.GetAllFinishedByUserId(id);
        }

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
