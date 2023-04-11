using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOSTeam.TravelAgency.Domain.Models;

namespace SOSTeam.TravelAgency.Domain.RepositoryInterfaces
{
    public interface IImageRepository : IRepository<Image>
    {
        void SaveAll(List<Image> images);
        List<Image> GetAllForTours();
    }
}
