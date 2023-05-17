using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.Views.Guest1;
using SOSTeam.TravelAgency.WPF.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows;
using System.Reflection.Metadata;
using SOSTeam.TravelAgency.Application.Services;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest1
{
    public class RequestsStatusViewModel
    {
        public User LoggedInUser { get; set; }
        public Window ThisWindow { get; set; }
        public Window UserProfilleWindow { get; set; }
        public Frame ThisFrame { get; set; }
        public string UsernameTextBlock { get; set; }
        public int Notifications { get; set; }
        public bool Report { get; set; }
        public string WindowNameTextBlock { get; set; }
        public RelayCommand NavigationButtonCommand { get; set; }

        public RequestsStatusViewModel(User user, Window window, Frame frame, Window profille, int notifications, bool report)
        {
            LoggedInUser = user;
            ThisWindow = window;
            ThisFrame = frame;
            UsernameTextBlock = user.Username;
            UserProfilleWindow = profille;
            Notifications = notifications;
            Report = report;

            FillTitleBlock();

            NavigationButtonCommand = new RelayCommand(Execute_NavigationButtonCommand);
        }
        
        private void FillTitleBlock()
        {
            if (!Report) WindowNameTextBlock = "Pregled zahtjeva";
            else WindowNameTextBlock = "Izvještaj";
        }
        
        public void SetStartupPage()
        {
            var navigationService = ThisFrame.NavigationService;
            if (!Report)
            {
                navigationService.Navigate(new AllStatusesPage(LoggedInUser, ThisFrame));
            }
            else
            {
                navigationService.Navigate(new ReportFiltersPage(LoggedInUser, ThisFrame));
            }
        }
        
        public void Execute_NavigationButtonCommand(object parameter)
        {
            string nextPage = parameter.ToString();
            var navigationService = ThisFrame.NavigationService;

            switch (nextPage)
            {
                case "Profille":
                    NotificationFromOwnerService service = new NotificationFromOwnerService();
                    UserProfilleWindow newWindow = new UserProfilleWindow(LoggedInUser, service.TestInboxCharge(LoggedInUser.Id));
                    ThisWindow.Close();
                    newWindow.ShowDialog();
                    break;
                case "Search":
                    break;
                case "Bid":
                    break;
                case "Whatever":
                    break;
                case "LogOut":
                    SignInForm form = new SignInForm();
                    ThisWindow.Close();
                    UserProfilleWindow.Close();
                    form.ShowDialog();
                    break;
                default:
                    break;
            }
            return;
        }
    }
}
