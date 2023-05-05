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
    public class HelpPageViewModel : ViewModel
    {
        private static HelpPage _page;
        public static int TourId { get; set; }
        public static User LoggedInUser { get; set; }

        private RelayCommand _backCommand;
        public RelayCommand BackCommand
        {
            get { return _backCommand; }
            set
            {
                _backCommand = value;
            }
        }

        public HelpPageViewModel(HelpPage page,User loggedInUser,int tourId) 
        {
            _page= page;
            BackCommand = new RelayCommand(Execute_BackCommand, CanExecuteMethod);       
            LoggedInUser= loggedInUser;
            TourId=tourId;
        }

        private void Execute_BackCommand(object obj)
        {
            string previousWindowOrPageName = PreviousWindowOrPageName.GetPreviousWindowOrPageName();

            // Navigate back to the previous window or page based on the name
            if (previousWindowOrPageName == typeof(MainViewModel).Name)
            {

                ToursOverviewWindow window = new ToursOverviewWindow(LoggedInUser);
                window.Show();
                Window.GetWindow(_page).Close();
            }
            else if (previousWindowOrPageName == typeof(MyToursPageViewModel).Name)
            {              
                var navigationService = _page.MyToursFrame.NavigationService;
                navigationService.Navigate(new MyToursPage(LoggedInUser));
            }
            else if (previousWindowOrPageName == typeof(VouchersWindowViewModel).Name)
            {
                VouchersWindow window = new VouchersWindow(LoggedInUser);
                ToursOverviewWindow mainWindow = new ToursOverviewWindow(LoggedInUser);
                Window.GetWindow(_page).Close();
                mainWindow.Show();
                window.ShowDialog();
            }
            else if (previousWindowOrPageName == typeof(NotificationsWindowViewModel).Name)
            {
                NotificationsWindow window = new NotificationsWindow(LoggedInUser);
                ToursOverviewWindow mainWindow = new ToursOverviewWindow(LoggedInUser);
                Window.GetWindow(_page).Close();
                mainWindow.Show();
                window.ShowDialog();
            }
            else if (previousWindowOrPageName == typeof(BookTourViewModel).Name)
            {
                BookTourWindow window = new BookTourWindow(TourId,LoggedInUser);
                ToursOverviewWindow mainWindow = new ToursOverviewWindow(LoggedInUser);
                Window.GetWindow(_page).Close();
                mainWindow.Show();
                window.ShowDialog();
            }
        }

        private bool CanExecuteMethod(object parameter)
        {
            return true;
        }
    }
}
