using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.WPF.Views;
using System.Windows;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.WPF.Views.Owner;
using System.Windows.Controls;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Owner
{
    public class MainWindowViewModel : ViewModel
    {
        public string Username { get; private set; }
        public User LoggedInUser { get; private set; }
        private MainWindow _mainWindow;

        public RelayCommand NavigationButtonCommand { get; private set; }
        public MainWindowViewModel(User user,MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
            Username = user.Username;
            LoggedInUser = user;
            NavigationButtonCommand = new RelayCommand(Execute_NavigationButtonCommand, CanExecuteMethod);
        }

        public void SetStartupPage() 
        {
            Execute_NavigationButtonCommand("Home");
        }

        private void Execute_NavigationButtonCommand(object parameter)
        {
            string nextPage = parameter.ToString();
            var navigationService = _mainWindow.MainFrame.NavigationService;

            switch (nextPage) 
            {
                case "Home":
                    navigationService.Navigate(new HomePage(LoggedInUser));
                    break;
                case "Accommodation":
                    navigationService.Navigate(new AccommodationsPage(LoggedInUser));
                    break;
                /*
                case "Review":
                    navigationService.Navigate(new HomePage());
                    break;
                case "Renovation":
                    navigationService.Navigate(new HomePage());
                    break;
                case "Request":
                    navigationService.Navigate(new HomePage());
                    break;
                case "Suggestion":
                    navigationService.Navigate(new HomePage());
                    break;
                case "Forum":
                    navigationService.Navigate(new HomePage());
                    break;
                */

                default:
                    break;
            }
            return;

            
        }

        private bool CanExecuteMethod(object parameter)
        {
            return true;
        }


    }
}
