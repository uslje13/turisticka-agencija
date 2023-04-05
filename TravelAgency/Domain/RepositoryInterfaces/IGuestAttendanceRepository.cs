using SOSTeam.TravelAgency.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.Domain.RepositoryInterfaces
{
    public interface IGuestAttendanceRepository : IRepository<GuestAttendance>
    {
        List<GuestAttendance> GetAllByActivityId(int id);
        List<GuestAttendance> GetByUserId(int id);
        void SaveAll(List<GuestAttendance> attendances);
    }
}
