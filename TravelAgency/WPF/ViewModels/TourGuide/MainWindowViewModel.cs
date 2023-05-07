using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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

        public RelayCommand NavigationButtonCommand { get; set; }

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

            NavigationButtonCommand = new RelayCommand(ExecuteNavigationButtonCommand, CanExecuteMethod);
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
            System.Windows.Application.Current.Windows.OfType<MainWindow>().FirstOrDefault().MainFrame.Content = createTourPage;
        }

        private void ShowHomePage(object sender)
        {
            HomePage homePage = new HomePage(_loggedUser, Timer);
            System.Windows.Application.Current.Windows.OfType<MainWindow>().FirstOrDefault().MainFrame.Content = homePage;
        }

        private void ShowLiveTourPage(object sender)
        {
            LiveTourPage liveTourPage = new LiveTourPage(_loggedUser);
            System.Windows.Application.Current.Windows.OfType<MainWindow>().FirstOrDefault().MainFrame.Content = liveTourPage;
        }

        private void ExecuteNavigationButtonCommand(object parameter)
        {
            Window currentWindow = System.Windows.Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
            MainWindow mainWindow = (MainWindow)currentWindow;
            var button = parameter as string;
            var navigationService = mainWindow.MainFrame.NavigationService;

            switch (button)
            {
                case "Back":
                    if (mainWindow.MainFrame.Content is AddCheckpointsPage)
                    {
                        navigationService.GoBack();
                    }
                    if (mainWindow.MainFrame.Content is AddAppointmentsPage)
                    {
                        navigationService.GoBack();
                    }
                    if (mainWindow.MainFrame.Content is HomePage)
                    {
                        MessageBoxWindow messageBox = CreateMessageBox();
                        var result = messageBox.ShowDialog();
                        if (result == true)
                        {
                            mainWindow.Close();
                        }
                    }

                    if (mainWindow.MainFrame.Content is TourGalleryPage)
                    {
                        navigationService.GoBack();
                    }
                    break;
            }
        }

        private MessageBoxWindow CreateMessageBox()
        {
            const string message = "Are you sure you want to exit the application?";

            var messageBoxViewModel = new MessageBoxViewModel("Alert", "/Resources/Icons/warning.png", message);
            var messageBoxWindow = new MessageBoxWindow
            {
                DataContext = messageBoxViewModel
            };
            return messageBoxWindow;
        }

    }
}
