using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Domain.RepositoryInterfaces;
using SOSTeam.TravelAgency.Repositories.Serializer;

namespace SOSTeam.TravelAgency.Repositories
{
    public class NewTourNotificationRepository : INewTourNotificationRepository
    {
        private const string FilePath = "../../../Resources/Data/images.csv";

        private readonly Serializer<NewTourNotification> _serializer;

        private List<NewTourNotification> _newTourNotifications;
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<NewTourNotification> GetAll()
        {
            throw new NotImplementedException();
        }

        public List<NewTourNotification> GetAllByGuestId(int guestId)
        {
            throw new NotImplementedException();
        }

        public NewTourNotification? GetById(int id)
        {
            throw new NotImplementedException();
        }

        public int NextId()
        {
            throw new NotImplementedException();
        }

        public void Save(NewTourNotification entity)
        {
            throw new NotImplementedException();
        }

        public void SaveAll(List<NewTourNotification> notifications)
        {
            throw new NotImplementedException();
        }

        public void Update(NewTourNotification entity)
        {
            throw new NotImplementedException();
        }
    }
}
