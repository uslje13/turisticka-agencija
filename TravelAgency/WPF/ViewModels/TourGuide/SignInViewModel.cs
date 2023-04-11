using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Domain.RepositoryInterfaces;
using SOSTeam.TravelAgency.WPF.Views;

namespace SOSTeam.TravelAgency.WPF.ViewModels.TourGuide
{
    public class SignInViewModel : ViewModel
    {
        private UserService _userService;

        private string _username;

        private string _password;

        public string Username
        {
            get => _username;
            set
            {
                if (value != _username)
                {
                    _username = value;
                    OnPropertyChanged("Username");
                }
                
            }
        }
        public string Password
        {
            get => _password;
            set
            {
                if (value != _password)
                {
                    _password = value;
                    OnPropertyChanged("Password");
                }
                
            }
        }



        public SignInViewModel()
        {
            _userService = new UserService();
        }

        private void SignIn(object sender, RoutedEventArgs e)
        {
            User user = _userService.GetByUsername(Username);
            if (user != null)
            {
                if (user.Password == Password && user.Role == Roles.TOURISTGUIDE)
                {
                    Views.TourGuide.MainWindow mainWindow = new Views.TourGuide.MainWindow();
                    mainWindow.Show();

                    /*
                    Views.Owner.MainPage mainPage = new Views.Owner.MainPage(user);
                    mainPage.Show();
                    Close();
                    */
                }
                else
                {
                    MessageBox.Show("Wrong password!");
                }
            }
            else
            {
                MessageBox.Show("Wrong username!");
            }
        }

        public RelayCommand SignInCommand { get; set; }


    }
}
