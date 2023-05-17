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
    public class TourRequestService
    {
        private readonly ITourRequestRepository _tourRequestRepository = Injector.CreateInstance<ITourRequestRepository>();

        public TourRequestService() { }

        public void Delete(int id)
        {
            _tourRequestRepository.Delete(id);
        }

        public List<TourRequest> GetAll()
        {
            return _tourRequestRepository.GetAll();
        }

        public TourRequest GetById(int id)
        {
            return _tourRequestRepository.GetById(id);
        }

        public void Save(TourRequest tourRequest)
        {
            _tourRequestRepository.Save(tourRequest);
        }

        public void Update(TourRequest tourRequest)
        {
            _tourRequestRepository.Update(tourRequest);
        }

        public void UpdateInvalidRequests()
        {
            var invalidRequests = _tourRequestRepository.GetInvalidRequests();
            foreach (var request in invalidRequests)
            {
                request.Status = StatusType.INVALID;
                Update(request);
            }
        }

        public List<TourRequest> GetAllOnHold()
        {
            return _tourRequestRepository.GetAll().FindAll(r => r.Status == StatusType.ON_HOLD);
        }

        public List<TourRequest> GetAllInvalid()
        {
            return _tourRequestRepository.GetAll().FindAll(r => r.Status == StatusType.INVALID);
        }
    }
}
