﻿using SOSTeam.TravelAgency.Domain;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Domain.RepositoryInterfaces;
using SOSTeam.TravelAgency.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.Application.Services
{
    internal class NotificationService
    {
        private readonly INotificationRepository _notificationRepository = Injector.CreateInstance<INotificationRepository>();
        private readonly IAccReservationRepository _accommodationReservationRepository = Injector.CreateInstance<IAccReservationRepository>();
        private readonly IAccommodationRepository _accommodationRepository = Injector.CreateInstance<IAccommodationRepository>();
        private readonly IGuestReviewRepository _guestReviewRepository = Injector.CreateInstance<IGuestReviewRepository>();


        
        public NotificationService()
        {

        }

        public void Delete(int id)
        {
            _notificationRepository.Delete(id);
        }

        public List<Notification> GetAll()
        {
            return _notificationRepository.GetAll();
        }

        public Notification GetById(int id)
        {
            return _notificationRepository.GetById(id);
        }

        public void Save(Notification notification)
        {
            _notificationRepository.Save(notification);
        }


        public void Update(Notification notification)
        {
            _notificationRepository.Update(notification);
        }

        public void Refresh(int userId) 
        {
            foreach (var reservation in _accommodationReservationRepository.GetAll())
            {
                foreach (Accommodation accommodation in _accommodationRepository.GetAllByUserId(userId))
                {
                    if (reservation.AccommodationId == accommodation.Id && IsInLastFiveDays(reservation.LastDay) && !IsReviewedGuest(userId, reservation.UserId))
                    {
                        _notificationRepository.Save(new Notification(
                            userId,
                            "Ocenite korisnika",
                            Notification.NotificationType.GUESTREVIEW,
                            false,
                            reservation.UserId
                            ));
                    }
                }
            }
        }

        private bool IsInLastFiveDays(DateTime time)
        {
            int dayDifference = DateTime.Now.DayOfYear - time.DayOfYear;
            return dayDifference <= 5 && dayDifference > -1;
        }

        private bool IsReviewedGuest(int ownerId, int userId)
        {
            return _guestReviewRepository.ReviewExists(ownerId, userId);
        }
    }
}
