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

        public RelayCommand HomeButtonCommand { get; private set; }
        public RelayCommand AccommodationButtonCommand { get; private set; }
        public RelayCommand UserButtonCommand { get; private set; }
        public RelayCommand ReviewButtonCommand { get; private set; }
        public RelayCommand RenovationsButtonCommand { get; private set; }
        public RelayCommand RequestsButtonCommand { get; private set; }
        public RelayCommand SuggestionsButtonCommand { get; private set; }
        public RelayCommand ForumButtonCommand { get; private set; }
        public MainWindowViewModel(User user,MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
            Username = user.Username;
            AccommodationService accommodationService = new();
            accommodationService.GetAll();


            LoggedInUser = user;
            HomeButtonCommand = new RelayCommand(Execute_HomeButtonCommand, CanExecuteMethod);
            AccommodationButtonCommand = new RelayCommand(Execute_AccommodationButtonCommand, CanExecuteMethod);
        }

        private void Execute_AccommodationButtonCommand(object sender)
        {
            _mainWindow.MainFrame.NavigationService.Navigate(new AccommodationsPage());
        }

        private void Execute_HomeButtonCommand(object sender)
        {
            _mainWindow.MainFrame.NavigationService.Navigate(new HomePage()) ;
        }

        private bool CanExecuteMethod(object parameter)
        {
            return true;
        }


    }
}
