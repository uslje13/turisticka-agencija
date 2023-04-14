﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Domain.RepositoryInterfaces;
using SOSTeam.TravelAgency.Repositories;

namespace SOSTeam.TravelAgency.Domain
{
    public class Injector
    {
        private static Dictionary<Type, object> _implementations = new Dictionary<Type, object>
        {
            { typeof(ITourRepository), new TourRepository() },
            { typeof(ILocationRepository), new LocationRepository() },
            { typeof(ICheckpointRepository), new CheckpointRepository() },
            { typeof(IImageRepository), new ImageRepository() },
            { typeof(IAppointmentRepository), new AppointmentRepository() },
            { typeof(ICheckpointActivityRepository), new CheckpointActivityRepository() },
            { typeof(IGuestAttendanceRepository), new GuestAttendanceRepository() },
            { typeof(IReservationRepository), new ReservationRepository() },
            { typeof(IUserRepository), new UserRepository() },
            { typeof(IAccReservationRepository), new AccommodationReservationRepository() },
            { typeof(IAccommodationRepository), new AccommodationRepository() },
            { typeof(INotificationRepository), new NotificationRepository() },
            { typeof(IChangedResRequestRepositroy), new ChangedResRequestRepository() },
            //{ typeof(IUserService), new UserService() },


        };

        public static T CreateInstance<T>()
        {
            Type type = typeof(T);

            if (_implementations.ContainsKey(type))
            {
                return (T)_implementations[type];
            }

            throw new ArgumentException($"No implementation found for type {type}");
        }
    }
}
