using SOSTeam.TravelAgency.Domain.RepositoryInterfaces;
using SOSTeam.TravelAgency.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOSTeam.TravelAgency.Domain.Models;

namespace SOSTeam.TravelAgency.Application.Services
{
    public class TourReviewService
    {
        private readonly ITourReviewRepository _tourReviewRepository = Injector.CreateInstance<ITourReviewRepository>();

        public TourReviewService() { }

        public void Delete(int id)
        {
            _tourReviewRepository.Delete(id);
        }

        public List<TourReview> GetAll()
        {
            return _tourReviewRepository.GetAll();
        }

        public TourReview GetById(int id)
        {
            return _tourReviewRepository.GetById(id);
        }

        public void Save(TourReview review)
        {
            _tourReviewRepository.Save(review);
        }

        public void Update(TourReview review)
        {
            _tourReviewRepository.Update(review);
        }

    }
}
