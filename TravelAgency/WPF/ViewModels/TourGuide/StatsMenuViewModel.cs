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
        public RelayCommand ShowTourRequestStatsCommand { get; set; }

        public User LoggedUser { get; set; }

        public StatsMenuViewModel(User loggedUser)
        {
            LoggedUser = loggedUser;
            ShowToursOverviewStatsCommand = new RelayCommand(ShowToursOverviewStats, CanExecuteMethod);
            ShowStatByToursCommand = new RelayCommand(ShowStatByTours, CanExecuteMethod);
            ShowTourRequestStatsCommand = new RelayCommand(ShowTourRequestStats, CanExecuteMethod);
        }

        private bool CanExecuteMethod(object parameter)
        {
            return true;
        }

        private void ShowToursOverviewStats(object sender)
        {
            StatsOverviewPage statsOverviewPage = new StatsOverviewPage(LoggedUser);
            System.Windows.Application.Current.Windows.OfType<MainWindow>().FirstOrDefault().MainFrame.Content = statsOverviewPage;
        }

        private void ShowStatByTours(object sender)
        {
            StatsByTourOverviewPage statsByTourOverviewTourOverviewPage = new StatsByTourOverviewPage(LoggedUser);
            System.Windows.Application.Current.Windows.OfType<MainWindow>().FirstOrDefault().MainFrame.Content = statsByTourOverviewTourOverviewPage;
        }

        private void ShowTourRequestStats(object sender)
        {
            TourRequestStatsPage tourRequestStatsPage = new TourRequestStatsPage();
            System.Windows.Application.Current.Windows.OfType<MainWindow>().FirstOrDefault().MainFrame.Content = tourRequestStatsPage;
        }

    }
}
