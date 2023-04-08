using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOSTeam.TravelAgency.Domain;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Domain.RepositoryInterfaces;

namespace SOSTeam.TravelAgency.Application.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository = Injector.CreateInstance<IUserRepository>();

        public UserService() { }

        public void Delete(int id)
        {
            _userRepository.Delete(id);
        }

        public List<User> GetAll()
        {
            return _userRepository.GetAll();
        }

        public User GetById(int id)
        {
            return _userRepository.GetById(id);
        }

        public void Save(User user)
        {
            _userRepository.Save(user);
        }

        public void Update(User user)
        {
            _userRepository.Update(user);
        }

        public User GetByUsername(string username)
        {
           return _userRepository.GetByUsername(username);
        }
    }
}
