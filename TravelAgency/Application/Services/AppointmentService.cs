using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Domain.RepositoryInterfaces;

namespace SOSTeam.TravelAgency.Application.Services
{
    public class AppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository = Injector.CreateInstance<IAppointmentRepository>();

        public AppointmentService()
        {
            
        }

        public void Delete(int id)
        {
            _appointmentRepository.Delete(id);
        }

        public List<Appointment> GetAll()
        {
            return _appointmentRepository.GetAll();
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

    }
}
