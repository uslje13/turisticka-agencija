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
    public class GuestReviewService 
    {
        private readonly IGuestReviewRepository _guestReviewRepository = Injector.CreateInstance<IGuestReviewRepository>();

        public GuestReviewService() { }

        public void Delete(int id)
        {
            _guestReviewRepository.Delete(id);
        }

        public List<GuestReview> GetAll()
        {
            return _guestReviewRepository.GetAll();
        }

        public List<GuestReview> GetAllByUserId(int ownerId)
        {
            return _guestReviewRepository.GetAll().Where(p => p.OwnerId == ownerId).ToList();
        }


        public GuestReview GetById(int id)
        {
            return _guestReviewRepository.GetById(id);
        }


        public void Save(GuestReview guestReview)
        {
            _guestReviewRepository.Save(guestReview);
        }

        public bool ReviewExists(int ownerId, int guestId)
        {
            return _guestReviewRepository.ReviewExists(ownerId, guestId);
        }


        public void Update(GuestReview guestReview)
        {
            _guestReviewRepository.Update(guestReview);
        }
    }
    
}
