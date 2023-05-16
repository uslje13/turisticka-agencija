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
        private ChangedReservationRequestService _changedReservationRequestService;
        private int _userId;
        public AccommodationStatsService(int userId)
        {
            _userId = userId;
            _accommodationReservationService = new();
            _accommodationService = new();
            _changedReservationRequestService = new();
        }

        public Tuple<int, int> GetCurrentOccupation() 
        {
            var accommodations = _accommodationService.GetAllByUserId(_userId);
            var reservations = _accommodationReservationService.GetAll().Where(r => accommodations.Any(a => r.AccommodationId == a.Id));
            reservations = reservations.Where(r => r.FirstDay <= DateTime.Today && DateTime.Today <= r.LastDay);
            return Tuple.Create(accommodations.Count - reservations.Count(), reservations.Count());
        }

        public List<int> GetOccupationInDays(DateTime startDate,int daysRange) 
        {
            List<int> result = new List<int>();
            var accommodations = _accommodationService.GetAllByUserId(_userId);
            var reservations = _accommodationReservationService.LoadFinishedReservations().Where(r => accommodations.Any(a => r.AccommodationId == a.Id)).ToList();
            reservations.AddRange(_accommodationReservationService.GetAll().Where(r => accommodations.Any(a => r.AccommodationId == a.Id)).ToList());
            for (int i = 0; i < daysRange; i++) 
            {
                var todayReservations = reservations.Where(r => r.FirstDay.Date <= startDate.AddDays(i).Date && startDate.AddDays(i).Date <= r.LastDay.Date);
                result.Add(todayReservations.Count());
            }
            return result;
            
        }

        public List<int> GetOccupationInYears(DateTime endYear, int yearsRange, int accommodationId)
        {
            var reservations = _accommodationReservationService.LoadFinishedReservations().Where(r => accommodationId == r.AccommodationId).ToList();
            reservations.AddRange(_accommodationReservationService.GetAll().Where(r => accommodationId == r.AccommodationId).ToList());
            return CalculateYearStats(endYear, yearsRange, reservations);

        }

        public List<int> GetCancelationInYears(DateTime endYear, int yearsRange, int accommodationId)
        {
            var reservations = _accommodationReservationService.LoadCanceledReservations().Where(r => accommodationId == r.AccommodationId).ToList();
            return CalculateYearStats(endYear, yearsRange, reservations);

        }

        public List<int> GetReservationMovesInYears(DateTime endYear, int yearsRange, int accommodationId)
        {
            var requests = _changedReservationRequestService.GetAll().Where(r => accommodationId == r.AccommodationId && r.status == ChangedReservationRequest.Status.ACCEPTED).ToList();
            List<int> result = new List<int>();

            for (int i = 0; i < yearsRange; i++)
            {
                var yearReservations = requests.Where(r => r.OldFirstDay.Year == endYear.AddYears(-i).Year || endYear.AddYears(-i).Year == r.OldLastDay.Year);
                result.Add(yearReservations.Count());
            }
            return result;

        }

        private List<int> CalculateYearStats(DateTime endYear, int yearsRange, List<AccommodationReservation> reservations)
        {
            List<int> result = new List<int>();

            for (int i = 0; i < yearsRange; i++)
            {
                var yearReservations = reservations.Where(r => r.FirstDay.Year == endYear.AddYears(-i).Year || endYear.AddYears(-i).Year == r.LastDay.Year);
                result.Add(yearReservations.Count());
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
