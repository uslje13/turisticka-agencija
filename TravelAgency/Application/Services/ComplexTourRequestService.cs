using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Domain.RepositoryInterfaces;
using SOSTeam.TravelAgency.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.Application.Services
{
    public class ComplexTourRequestService
    {
        private readonly IComplexTourRequestRepository _complexTourRequestRepository = Injector.CreateInstance<IComplexTourRequestRepository>();

        public ComplexTourRequestService() { }

        public void Delete(int id)
        {
            _complexTourRequestRepository.Delete(id);
        }

        public List<ComplexTourRequest> GetAll()
        {
            return _complexTourRequestRepository.GetAll();
        }

        public ComplexTourRequest GetById(int id)
        {
            return _complexTourRequestRepository.GetById(id);
        }

        public void Save(ComplexTourRequest tourRequest)
        {
            _complexTourRequestRepository.Save(tourRequest);
        }

        public void Update(ComplexTourRequest tourRequest)
        {
            _complexTourRequestRepository.Update(tourRequest);
        }
    }
}
