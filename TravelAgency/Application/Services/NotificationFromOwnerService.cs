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
    public class NotificationFromOwnerService
    {
        private readonly INotificationFromOwnerRepository notificationFromOwnerRepository = Injector.CreateInstance<INotificationFromOwnerRepository>();

        public NotificationFromOwnerService() { }

        public void Delete(int id)
        {
            notificationFromOwnerRepository.Delete(id);
        }

        public List<NotificationFromOwner> GetAll()
        {
            return notificationFromOwnerRepository.GetAll();
        }

        public NotificationFromOwner GetById(int id)
        {
            return notificationFromOwnerRepository.GetById(id);
        }

        public void Save(NotificationFromOwner notificationFromOwner)
        {
            notificationFromOwnerRepository.Save(notificationFromOwner);
        }

        public void Update(NotificationFromOwner notificationFromOwner)
        {
            notificationFromOwnerRepository.Update(notificationFromOwner);
        }
    }
}
