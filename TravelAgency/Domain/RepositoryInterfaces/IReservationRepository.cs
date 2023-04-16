using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOSTeam.TravelAgency.Domain.Models;

namespace SOSTeam.TravelAgency.Domain.RepositoryInterfaces
{
    public interface IReservationRepository : IRepository<Reservation>
    {
        int GetId(Reservation reservation);
        List<Reservation> GetAllByAppointmentId(int id);
    }
}
