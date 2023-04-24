using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.Views.TourGuide;

namespace SOSTeam.TravelAgency.WPF.ViewModels.TourGuide
{
    public class MainWindowViewModel
    {
        private readonly DispatcherTimer _timer;
        public DispatcherTimer Timer => _timer;

        private readonly User _loggedUser;

        #region Services

        private readonly AppointmentService _appointmentService;

        #endregion

        #region Commands
        public RelayCommand ShowCreateTourFormCommand { get; set; }
        public RelayCommand ShowHomePageCommand { get; set; }
        public RelayCommand ShowLiveTourCommand { get; set; }

        #endregion

        public MainWindowViewModel(User loggedUser)
        {
            _loggedUser = loggedUser;
            _appointmentService = new AppointmentService();

            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMinutes(1)
            };
            Timer.Tick += UpdateAppointments;
            Timer.Start();

            ShowCreateTourFormCommand = new RelayCommand(ShowCreateTourForm, CanExecuteMethod);
            ShowHomePageCommand = new RelayCommand(ShowHomePage, CanExecuteMethod);
            ShowLiveTourCommand = new RelayCommand(ShowLiveTourPage, CanExecuteMethod);
        }

        private void UpdateAppointments(object sender, EventArgs e)
        {
            _appointmentService.SetExpiredAppointments(_loggedUser.Id);
        }

        private bool CanExecuteMethod(object parameter)
        {
            return true;
        }

        private void ShowCreateTourForm(object sender)
        {
            CreateTourPage createTourPage = new CreateTourPage(_loggedUser);
            System.Windows.Application.Current.Windows.OfType<MainWindow>().FirstOrDefault().ToursOverviewFrame.Content = createTourPage;
        }

        private void ShowHomePage(object sender)
        {
            HomePage homePage = new HomePage(_loggedUser, Timer);
            System.Windows.Application.Current.Windows.OfType<MainWindow>().FirstOrDefault().ToursOverviewFrame.Content = homePage;
        }

        private void ShowLiveTourPage(object sender)
        {
            LiveTourPage liveTourPage = new LiveTourPage(_loggedUser);
            System.Windows.Application.Current.Windows.OfType<MainWindow>().FirstOrDefault().ToursOverviewFrame.Content = liveTourPage;
        }

    }
}
