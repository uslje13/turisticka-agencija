using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOSTeam.TravelAgency.Domain;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Domain.RepositoryInterfaces;

namespace SOSTeam.TravelAgency.Application.Services
{
    public class GuestAttendanceService
    {
        private readonly IGuestAttendanceRepository _guestAttendanceRepository = Injector.CreateInstance<IGuestAttendanceRepository>();

        public GuestAttendanceService() { }

        public void Delete(int id)
        {
            _guestAttendanceRepository.Delete(id);
        }

        public List<GuestAttendance> GetAll()
        {
            return _guestAttendanceRepository.GetAll();
        }

        public GuestAttendance GetById(int id)
        {
            return  _guestAttendanceRepository.GetById(id);
        }

        public List<GuestAttendance> GetByUserId(int id)
        {
            return _guestAttendanceRepository.GetByUserId(id);
        }
        public List<GuestAttendance> GetAllByActivityId(int id)
        {
            return _guestAttendanceRepository.GetAllByActivityId(id);
        }

        public void Save(GuestAttendance guestAttendance)
        {
            _guestAttendanceRepository.Save(guestAttendance);
        }

        public void SaveAll(List<GuestAttendance> guestAttendances)
        {
            _guestAttendanceRepository.SaveAll(guestAttendances);
        }

        public void Update(GuestAttendance guestAttendance)
        {
            _guestAttendanceRepository.Update(guestAttendance);
        }
    }
}
