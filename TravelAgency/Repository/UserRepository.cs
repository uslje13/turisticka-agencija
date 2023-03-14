using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Model;
using TravelAgency.Serializer;

namespace TravelAgency.Repository
{
    internal class UserRepository
    {
        private const string FilePath = "../../../Resources/Data/users.txt";

        private readonly Serializer<User> _serializer;

        private List<User> _users;

        public UserRepository()
        {
            _serializer = new Serializer<User>();
            _users = _serializer.FromCSV(FilePath);
        }

        public User GetByUsername(string username)
        {
            User user = _users.Find(l => l.Username == username) ?? throw new ArgumentException();
            return user;
        }

        public User GetById(int id)
        {
            User user = _users.Find(l => l.Id == id) ?? throw new ArgumentException();
            return user;
        }
    }
}
