using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.Views.TourGuide;

namespace SOSTeam.TravelAgency.WPF.ViewModels.TourGuide
{
    public class MainWindowViewModel
    {

        public RelayCommand ShowCreateTourFormCommand { get; set; }
        public RelayCommand ShowHomePageCommand { get; set; }
        public RelayCommand ShowLiveTourCommand { get; set; }

        public User LoggedUser { get; set; }

        public MainWindowViewModel(User loggedUser)
        {
            LoggedUser = loggedUser;
            ShowCreateTourFormCommand = new RelayCommand(ShowCreateTourForm, CanExecuteMethod);
            ShowHomePageCommand = new RelayCommand(ShowHomePage, CanExecuteMethod);
            ShowLiveTourCommand = new RelayCommand(ShowLiveTourPage, CanExecuteMethod);
        }

        private bool CanExecuteMethod(object parameter)
        {
            return true;
        }

        private void ShowCreateTourForm(object sender)
        {
            CreateTourPage createTourPage = new CreateTourPage(LoggedUser);
            System.Windows.Application.Current.Windows.OfType<MainWindow>().FirstOrDefault().ToursOverviewFrame.Content = createTourPage;
        }

        private void ShowHomePage(object sender)
        {
            HomePage homePage = new HomePage(LoggedUser);
            System.Windows.Application.Current.Windows.OfType<MainWindow>().FirstOrDefault().ToursOverviewFrame.Content = homePage;
        }

        private void ShowLiveTourPage(object sender)
        {
            LiveTourPage liveTourPage = new LiveTourPage(LoggedUser);
            System.Windows.Application.Current.Windows.OfType<MainWindow>().FirstOrDefault().ToursOverviewFrame.Content = liveTourPage;
        }

    }
}
