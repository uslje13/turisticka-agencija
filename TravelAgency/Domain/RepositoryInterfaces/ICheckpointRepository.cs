using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOSTeam.TravelAgency.Domain.Models;

namespace SOSTeam.TravelAgency.Domain.RepositoryInterfaces
{
    public interface ICheckpointRepository : IRepository<Checkpoint>
    {
        void SaveAll(List<Checkpoint> checkpoints);
        List<Checkpoint> GetAllByTourId(int id);
    }
}
