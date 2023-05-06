using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Owner
{
    public class UserPageViewModel
    {
        public string Username { get; private set; }
        public User LoggedInUser { get; private set; }
        public RelayCommand LogOut { get; private set; }
        public UserPageViewModel(User user, MainWindowViewModel mainWindowVM)
        {
            Username = user.Username;
            LoggedInUser = user;
            LogOut = new RelayCommand(Execute_LogOut, CanExecuteLogOut);
        }

        private void Execute_LogOut(object obj)
        {
            App.Current.Shutdown();
        }

        private bool CanExecuteLogOut(object obj)
        {
            return true;
        }
    }
}
