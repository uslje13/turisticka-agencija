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
    public class NewTourNotificationService
    {
        private readonly INewTourNotificationRepository _newTourNotificationRepository = Injector.CreateInstance<INewTourNotificationRepository>();
        public NewTourNotificationService() { }

        public void Delete(int id)
        {
            _newTourNotificationRepository.Delete(id);
        }

        public List<NewTourNotification> GetAll()
        {
            return _newTourNotificationRepository.GetAll();
        }

        public NewTourNotification GetById(int id)
        {
            return _newTourNotificationRepository.GetById(id);
        }

        public void Save(NewTourNotification notification)
        {
            _newTourNotificationRepository.Save(notification);
        }

        public void SaveAll(List<NewTourNotification> notifications)
        {
            _newTourNotificationRepository.SaveAll(notifications);
        }

        public void Update(NewTourNotification notification)
        {
            _newTourNotificationRepository.Update(notification);
        }

        public List<NewTourNotification> GetAllByGuestId(int guestId)
        {
            return _newTourNotificationRepository.GetAllByGuestId(guestId);
        }
    }
}
