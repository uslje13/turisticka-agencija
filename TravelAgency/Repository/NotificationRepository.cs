﻿using System;
using System.Collections.Generic;
using System.Linq;
using TravelAgency.Model;
using TravelAgency.Serializer;

namespace TravelAgency.Repository
{
    public class NotificationRepository

    {
        private const string FilePath = "../../../Resources/Data/notification.csv";
        private readonly Serializer<Notification> _serializer;
        private List<Notification> _notifications;

        public NotificationRepository()
        {
            _serializer = new Serializer<Notification>();
            _notifications = new List<Notification>();
        }

        public List<Notification> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public Notification Save(Notification notification)
        {
            notification.Id = NextId();
            _notifications = _serializer.FromCSV(FilePath);
            _notifications.Add(notification);
            _serializer.ToCSV(FilePath, _notifications);
            return notification;
        }

        public void Delete(Notification notification)
        {
            _notifications = _serializer.FromCSV(FilePath);
            Notification found = _notifications.Find(t => t.Id == notification.Id) ?? throw new ArgumentException();
            _notifications.Remove(found);
            _serializer.ToCSV(FilePath, _notifications);
        }

        public Notification Update(Notification notification)
        {
            _notifications = _serializer.FromCSV(FilePath);
            Notification current = _notifications.Find(t => t.Id == notification.Id) ?? throw new ArgumentException();
            int index = _notifications.IndexOf(current);
            _notifications.Remove(current);
            _notifications.Insert(index, notification);
            _serializer.ToCSV(FilePath, _notifications);
            return notification;
        }
        public int NextId()
        {
            _notifications = _serializer.FromCSV(FilePath);
            if (_notifications.Count < 1)
            {
                return 1;
            }
            return _notifications.Max(l => l.Id) + 1;
        }

    }
}
