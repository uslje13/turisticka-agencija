using System;
using System.Collections.Generic;
using System.Linq;
using SOSTeam.TravelAgency.Domain;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Domain.RepositoryInterfaces;

namespace SOSTeam.TravelAgency.Application.Services
{
    public class AccommodationStatsService
    {
        private AccommodationService _accommodationService;
        private AccommodationReservationService _accommodationReservationService;
        private int _userId;
        public AccommodationStatsService(int userId)
        {
            _userId = userId;
            _accommodationReservationService = new();
            _accommodationService = new();
        }

        public Tuple<int, int> GetCurrentOccupation() 
        {
            var accommodations = _accommodationService.GetAllByUserId(_userId);
            var reservations = _accommodationReservationService.GetAll().Where(r => accommodations.Any(a => r.AccommodationId == a.Id));
            reservations = reservations.Where(r => r.FirstDay <= DateTime.Today && DateTime.Today <= r.LastDay);
            return Tuple.Create(accommodations.Count - reservations.Count(), reservations.Count());
        }

        public List<int> GetOccupationInRange(DateTime startDate,int daysRange) 
        {
            List<int> result = new List<int>();
            var accommodations = _accommodationService.GetAllByUserId(_userId);
            var reservations = _accommodationReservationService.GetAll().Where(r => accommodations.Any(a => r.AccommodationId == a.Id));
            for (int i = 0; i < daysRange; i++) 
            {
                var todayReservations = reservations.Where(r => r.FirstDay.Date <= startDate.AddDays(i).Date && startDate.AddDays(i).Date <= r.LastDay.Date);
                result.Add(todayReservations.Count());
            }
            return result;
            
        }

        public Accommodation GetMostPopularAccommodation() 
        {
            var accommodations = _accommodationService.GetAllByUserId(_userId);
            var reservations = _accommodationReservationService.LoadFinishedReservations().Where(r => accommodations.Any(a => r.AccommodationId == a.Id));
            return accommodations.OrderByDescending(a => reservations.Count(r => r.AccommodationId == a.Id)).First();
        }
    }
}
