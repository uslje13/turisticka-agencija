using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.Views.Guest2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest2
{
    public class HelpWindowViewModel : ViewModel
    {
        public event EventHandler CloseRequested;

        private RelayCommand _backCommand;
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

        public NavigationService NavigationService { get; set; }

        public HelpWindowViewModel(NavigationService navigationService)
        {
            BackCommand = new RelayCommand(Execute_BackCommand, CanExecuteMethod);
            NavigationCommand = new RelayCommand(Execute_NavigationCommand, CanExecuteMethod);
            NavigationService = navigationService;
        }

        public void SetStartupPage()
        {
            Execute_NavigationCommand("HelpPage");
        }
        private void Execute_NavigationCommand(object obj)
        {
            string nextPage = obj.ToString();

            switch (nextPage)
            {
                case "HelpPage":
                    NavigationService.Navigate(new GeneralHelpPage());
                    break;
                case "TutorialPage":
                    NavigationService.Navigate(new TutorialPage());
                    break;
            }
        }

        private void Execute_BackCommand(object obj)
        {
            CloseRequested?.Invoke(this, EventArgs.Empty);
        }

        private bool CanExecuteMethod(object parameter)
        {
            return true;
        }
    }
}
