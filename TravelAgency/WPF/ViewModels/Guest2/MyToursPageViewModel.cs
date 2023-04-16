using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.Views.Guest2;
using SOSTeam.TravelAgency.WPF.Views.Owner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest2
{
    public class MyToursPageViewModel : ViewModel
    {
        private MyToursPage _page;
        private RelayCommand _backCommand;
        public static User LoggedInUser { get; set; }
        public RelayCommand BackCommand
        {
            get { return _backCommand; }
            set
            {
                _backCommand = value;
            }
        }

        private RelayCommand _navigationCommand;
        public RelayCommand NavigationCommand
        {
            get { return _navigationCommand; }
            set
            {
                _navigationCommand = value;
            }
        }
        public MyToursPageViewModel(User loggedInUser, MyToursPage page) 
        {
            BackCommand = new RelayCommand(Execute_BackCommand,CanExecuteMethod);
            NavigationCommand = new RelayCommand(Execute_NavigationCommand, CanExecuteMethod);
            LoggedInUser = loggedInUser;
            _page = page;
        }
        public void SetStartupPage()
        {
            Execute_NavigationCommand("ActiveTour");
        }
        private void Execute_NavigationCommand(object obj)
        {
            string nextPage = obj.ToString();
            var navigationService = _page.MainFrame.NavigationService;

            switch(nextPage)
            {
                case "ActiveTour":
                    navigationService.Navigate(new ActiveTourPage(LoggedInUser));
                    break;
            }
        }

        private void Execute_BackCommand(object obj)
        {
            ToursOverviewWindow window = new ToursOverviewWindow(LoggedInUser);
            window.Show();
            Window.GetWindow(_page).Close();
        }

        private bool CanExecuteMethod(object parameter)
        {
            return true;
        }
    }
}
