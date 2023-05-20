using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using SOSTeam.TravelAgency.WPF.Views.TourGuide;
using System.Linq;
using SOSTeam.TravelAgency.WPF.ViewModels.TourGuide;
using System.Windows.Controls;
using System.Windows.Threading;
using SOSTeam.TravelAgency.Domain.Models;
using System.Windows.Navigation;
using System.Reflection.Metadata;
using SOSTeam.TravelAgency.WPF.Views.TourGuide;

namespace SOSTeam.TravelAgency.WPF.Navigation
{
    public class TourGuideNavigationService
    {
        private MainWindow _mainWindow;
        public MainWindow MainWindow { get => _mainWindow; set => _mainWindow = value; }

        private MainWindowViewModel _mainWindowViewModel;
        private Stack<Page> _previousPages;
        public Stack<Page> PreviousPages
        {
            get => _previousPages; set => _previousPages = value;
        }

        private readonly TourCardCreatorViewModel _tourCardCreatorViewModel;
        public TourGuideNavigationService()
        {
            _mainWindow = SetMainWindow();
            _mainWindowViewModel = (MainWindowViewModel)_mainWindow.DataContext;
            _previousPages = new Stack<Page>();
            _tourCardCreatorViewModel = new TourCardCreatorViewModel();
        }

        private MainWindow SetMainWindow()
        {
            var currentWindow = System.Windows.Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
            
            var mainWindow = (MainWindow)currentWindow;

            return mainWindow;
        }

        public void SetMainFrame(string frameName, User loggedUser)
        {
            if (frameName == "CreateTourPage")
            {
                _mainWindowViewModel.Title = "Create tour";
                MainWindow.DataContext = _mainWindowViewModel;
                _mainWindow.MainFrame.Content = new CreateTourPage(loggedUser);
            }
            else if (frameName == "HomePage")
            {
                _mainWindowViewModel.Title = "Home";
                MainWindow.DataContext = _mainWindowViewModel;
                _mainWindow.MainFrame.Content = new HomePage(loggedUser, _mainWindowViewModel.Timer);
            }
            else if (frameName == "LiveTour")
            {
                _mainWindowViewModel.Title = "Live tour";
                MainWindow.DataContext = _mainWindowViewModel;
                _mainWindow.MainFrame.Content = new LiveTourPage(loggedUser);
            }
            else if (frameName == "Reviews")
            {
                _mainWindowViewModel.Title = "Reviews";
                MainWindow.DataContext = _mainWindowViewModel;
                _mainWindow.MainFrame.Content = new GuestReviewsByTourPage(loggedUser);
            }
            else if (frameName == "Stats")
            {
                _mainWindowViewModel.Title = "Stats";
                MainWindow.DataContext = _mainWindowViewModel;
                _mainWindow.MainFrame.Content = new StatsMenuPage(loggedUser);
            }
            else if(frameName == "Requests")
            {
                _mainWindowViewModel.Title = "Requests";
                MainWindow.DataContext = _mainWindowViewModel;
                _mainWindow.MainFrame.Content = new RequestMenuPage();
            }
            else if (frameName == "RegularRequests")
            {
                _mainWindowViewModel.Title = "Requests";
                MainWindow.DataContext = _mainWindowViewModel;
                _mainWindow.MainFrame.Content = new TourRequestPage();
            }
            else if (frameName == "TodayTours")
            {
                _mainWindowViewModel.Title = "Today tours";
                MainWindow.DataContext = _mainWindowViewModel;
                _mainWindow.MainFrame.Content = new TodayToursPage(loggedUser, _mainWindowViewModel.Timer);
            }
            else if (frameName == "AddCheckpoints")
            {
                _mainWindowViewModel.Title = "Create tour";
                MainWindow.DataContext = _mainWindowViewModel;
                var createTourPage = MainWindow.MainFrame.Content as CreateTourPage;
                var createTourViewModel = (CreateTourViewModel)createTourPage.DataContext;
                _mainWindow.MainFrame.Content = new AddCheckpointsPage(createTourViewModel.CheckpointCards);
            }
            else if (frameName == "AddAppointments")
            {
                _mainWindowViewModel.Title = "Create tour";
                MainWindow.DataContext = _mainWindowViewModel;
                var createTourPage = MainWindow.MainFrame.Content as CreateTourPage;
                var createTourViewModel = (CreateTourViewModel)createTourPage.DataContext;
                _mainWindow.MainFrame.Content = new AddAppointmentsPage(createTourViewModel.AppointmentCards);
            }
            else if (frameName == "TourGallery")
            {
                _mainWindowViewModel.Title = "Create tour";
                MainWindow.DataContext = _mainWindowViewModel;
                var createTourPage = MainWindow.MainFrame.Content as CreateTourPage;
                var createTourViewModel = (CreateTourViewModel)createTourPage.DataContext;
                _mainWindow.MainFrame.Content = new TourGalleryPage(createTourViewModel.Images);
            }
            else if(frameName == "GuestAttendances")
            {
                _mainWindowViewModel.Title = "Guest attendances";
                MainWindow.DataContext = _mainWindowViewModel;
                var liveTourPage = MainWindow.MainFrame.Content as LiveTourPage;
                var liveTourViewModel = (LiveTourViewModel)liveTourPage.DataContext;
                _mainWindow.MainFrame.Content = new GuestAttendancePage(
                    liveTourViewModel.SelectedCheckpointActivityCard, liveTourViewModel.TourName,
                    liveTourViewModel.Date);
            }
            else if(frameName == "AcceptTourRequestForm")
            {
                _mainWindowViewModel.Title = "Accept tour";
                MainWindow.DataContext = _mainWindowViewModel;
                var tourRequestsPage = MainWindow.MainFrame.Content as TourRequestPage;
                var tourRequestsViewModel = (TourRequestViewModel)tourRequestsPage.DataContext;
                _mainWindow.MainFrame.Content = new AcceptTourRequestPage(tourRequestsViewModel.SelectedTourRequestCard.Id);
            }

        }

        public void AddPreviousPage()
        {
            _previousPages.Push(_mainWindow.MainFrame.Content as Page);
        }

        public void GoBack(string buttonName)
        {
            var navigationService = _mainWindow.MainFrame.NavigationService;

            switch (buttonName)
            {
                case "Back":
                    
                    if (_mainWindow.MainFrame.Content is HomePage)
                    {
                        YesNoMessageBox yesNoMessageBox = CreateYesNoMessageBox("Are you sure you want to exit the application?");
                        var result = yesNoMessageBox.ShowDialog();
                        if (result == true)
                        {
                            _mainWindow.Close();
                        }
                    }
                    else
                    {
                        var previousPage = PreviousPages.Pop();
                        navigationService.GoBack();


                        if (previousPage is HomePage)
                        {
                             var homePage = (HomePage)previousPage;
                             var homePageViewModel = homePage.DataContext as HomePageViewModel;
                             homePageViewModel.TourCards = _tourCardCreatorViewModel.CreateCards(homePageViewModel.LoggedUser, CreationType.ALL);
                            _mainWindowViewModel.Title = "Home";
                            _mainWindow.DataContext = _mainWindowViewModel;
                        }

                        if (previousPage is StatsMenuPage)
                        {
                            _mainWindowViewModel.Title = "Stats";
                            _mainWindow.DataContext = _mainWindowViewModel;
                        }

                        if (previousPage is RequestMenuPage)
                        {
                            _mainWindowViewModel.Title = "Requests";
                            _mainWindow.DataContext = _mainWindowViewModel;
                        }

                        if (previousPage is TourRequestPage)
                        {
                            _mainWindowViewModel.Title = "Requests";
                            _mainWindow.DataContext = _mainWindowViewModel;
                        }

                        if (previousPage is LiveTourPage)
                        {
                            _mainWindowViewModel.Title = "Live tour";
                            _mainWindow.DataContext = _mainWindowViewModel;
                        }

                        if (previousPage is GuestReviewsByTourPage)
                        {
                            _mainWindowViewModel.Title = "Reviews";
                            _mainWindow.DataContext = _mainWindowViewModel;
                        }

                        if (previousPage is CreateTourPage)
                        {
                            _mainWindowViewModel.Title = "Create tour";
                            _mainWindow.DataContext = _mainWindowViewModel;
                        }

                        if (previousPage is TodayToursPage)
                        {
                            var todayToursPage = (TodayToursPage)previousPage;
                            var todayToursPageViewModel = todayToursPage.DataContext as TodayToursViewModel;
                            todayToursPageViewModel.TodayTourCards = _tourCardCreatorViewModel.CreateCards(todayToursPageViewModel.LoggedUser, CreationType.TODAY);
                            _mainWindowViewModel.Title = "Today tours";
                            _mainWindow.DataContext = _mainWindowViewModel;
                        }
                        
                        if (previousPage is CreateTourPage)
                        {
                            _mainWindowViewModel.Title = "Create tour";
                            _mainWindow.DataContext = _mainWindowViewModel;
                        }

                        if (previousPage is AddCheckpointsPage)
                        {
                            _mainWindowViewModel.Title = "Create tour";
                            _mainWindow.DataContext = _mainWindowViewModel;
                        }
                        if (previousPage is AddAppointmentsPage)
                        {
                            _mainWindowViewModel.Title = "Create tour";
                            _mainWindow.DataContext = _mainWindowViewModel;
                        }
                        if (previousPage is TourGalleryPage)
                        {
                            _mainWindowViewModel.Title = "Create tour";
                            _mainWindow.DataContext = _mainWindowViewModel;
                        }

                        if (previousPage is GuestAttendancePage)
                        {
                            _mainWindowViewModel.Title = "Guest attendances";
                            _mainWindow.DataContext = _mainWindowViewModel;
                        }

                        if (previousPage is TourRequestPage)
                        {
                            _mainWindowViewModel.Title = "Tour requests";
                            _mainWindow.DataContext = _mainWindowViewModel;
                        }

                    }

                    break;
            }
        }

        public void CreateOkMessageBox(string message, string title = "Alert",
            string imagePath = "/Resources/Icons/warning.png")
        {
            var messageBoxViewModel = new OkMessageBoxViewModel(title, imagePath, message);
            var okMessageBox = new OkMessageBox();
            okMessageBox.DataContext = messageBoxViewModel;
            okMessageBox.ShowDialog();
        }

        private YesNoMessageBox CreateYesNoMessageBox(string message, string title = "Alert", string imagePath = "/Resources/Icons/warning.png")
        {
            var messageBoxViewModel = new YesNoMessageBoxViewModel(title, imagePath, message);
            var messageBoxWindow = new YesNoMessageBox
            {
                DataContext = messageBoxViewModel
            };
            return messageBoxWindow;
        }

        public bool? GetMessageBoxResult(string massage)
        {
            var messageBox = CreateYesNoMessageBox(massage);
            return messageBox.ShowDialog();
        }
    }
}
