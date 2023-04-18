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
    public class StatsMenuViewModel
    {
        public RelayCommand ShowToursOverviewStatsCommand { get; set; }

        public RelayCommand ShowStatByToursCommand { get; set; }

        public User LoggedUser { get; set; }

        public StatsMenuViewModel(User loggedUser)
        {
            LoggedUser = loggedUser;
            ShowToursOverviewStatsCommand = new RelayCommand(ShowToursOverviewStats, CanExecuteMethod);
            ShowStatByToursCommand = new RelayCommand(ShowStatByTours, CanExecuteMethod);
        }

        private bool CanExecuteMethod(object parameter)
        {
            return true;
        }

        private void ShowToursOverviewStats(object sender)
        {
            StatsOverviewPage statsOverviewPage = new StatsOverviewPage(LoggedUser);
            System.Windows.Application.Current.Windows.OfType<MainWindow>().FirstOrDefault().ToursOverviewFrame.Content = statsOverviewPage;
        }

        private void ShowStatByTours(object sender)
        {
            StatsByTourPage statsByTourTourPage = new StatsByTourPage(LoggedUser);
            System.Windows.Application.Current.Windows.OfType<MainWindow>().FirstOrDefault().ToursOverviewFrame.Content = statsByTourTourPage;
        }

    }
}
