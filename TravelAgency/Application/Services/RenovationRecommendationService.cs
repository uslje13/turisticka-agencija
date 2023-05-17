using SOSTeam.TravelAgency.Domain;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.Application.Services
{
    public class RenovationRecommendationService
    {
        private readonly IRenovationRecommendationRepository _renovationRecommendationRepository = Injector.CreateInstance<IRenovationRecommendationRepository>();

        public RenovationRecommendationService() { }

        public void Delete(int id)
        {
            _renovationRecommendationRepository.Delete(id);
        }

        public List<RenovationRecommendation> GetAll()
        {
            return _renovationRecommendationRepository.GetAll();
        }

        public RenovationRecommendation GetById(int id)
        {
            return _renovationRecommendationRepository.GetById(id);
        }

        public void Save(RenovationRecommendation renovationRecommendation)
        {
            _renovationRecommendationRepository.Save(renovationRecommendation);
        }

        public void Update(RenovationRecommendation renovationRecommendation)
        {
            _renovationRecommendationRepository.Update(renovationRecommendation);
        }
    }
}
