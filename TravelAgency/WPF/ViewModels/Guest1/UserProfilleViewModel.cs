using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;
using System.Windows.Controls;
using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.WPF.Views;
using SOSTeam.TravelAgency.WPF.Views.Guest1;
using System.Windows.Media;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;
using System.Collections.ObjectModel;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest1
{
    public class UserProfilleViewModel
    {
        public User LoggedInUser { get; set; }
        public Window ThisWindow { get; set; }
        public int ThisYearCounter { get; set; }
        public int Notifications { get; set; }
        public string UsernameTextBlock { get; set; }
        public string MessageNumberTextBlock { get; set; }
        public string CounterTextBlock { get; set; }
        public bool FirstLogging { get; set; }
        public List<CancelAndMarkResViewModel> _futuredReservations { get; set; }
        public List<CancelAndMarkResViewModel> _finishedReservations { get; set; }
        public List<AccommodationReservation> _accommodationReservations { get; set; }
        public ObservableCollection<LocAccommodationViewModel> _locAccommodationViewModels { get; set; }
        public RelayCommand ShowMenuCommand { get; set; }
        public RelayCommand ShowRequestsCommand { get; set; }
        public RelayCommand ShowInboxCommand { get; set; }
        public RelayCommand SignOutCommand { get; set; }
        public RelayCommand CanceledQueryCommand { get; set; }


        public UserProfilleViewModel(User user, int notifications, Window window) 
        {
            LoggedInUser = user;
            UsernameTextBlock = user.Username;
            ThisYearCounter = 0;
            ThisWindow = window;
            Notifications = notifications;
            FirstLogging = false;

            _futuredReservations = new List<CancelAndMarkResViewModel>();
            _finishedReservations = new List<CancelAndMarkResViewModel>();

            AccommodationReservationService accResService = new AccommodationReservationService();
            _accommodationReservations = accResService.GetAll();

            AccommodationService AccommodationService = new AccommodationService();
            _locAccommodationViewModels = AccommodationService.CreateAllDTOForms();

            ApplyToCounter(_accommodationReservations);
            ControlInboxButton(Notifications);
            AddFuturedReservations();
            CollectFinishedReservations();
            AddFinishedReservations();
            FillCounterTextBlock();
            IsFirstLogging();

            ShowMenuCommand = new RelayCommand(Execute_ShowMenu);
            ShowRequestsCommand = new RelayCommand(Execute_ShowStatuses);
            ShowInboxCommand = new RelayCommand(Execute_ShowInbox);
            SignOutCommand = new RelayCommand(Execute_SignOut);
            CanceledQueryCommand = new RelayCommand(Execute_ShowCanceledReservations);
        }

        private void IsFirstLogging()
        {
            if(Notifications == 0 && _futuredReservations.Count == 0 && _finishedReservations.Count == 0)
            {
                FirstLogging = true;
                //MessageBox.Show("Dobrodosli u aplikaciju.");
            }
        }

        private void Execute_ShowCanceledReservations(object sender)
        {
            RequestsStatusWindow newWindow = new RequestsStatusWindow(LoggedInUser, ThisWindow, Notifications, true);
            ThisWindow.Close();
            newWindow.ShowDialog();
        }

        private void FillCounterTextBlock()
        {
            CounterTextBlock = "Broj rezervacija u protekloj godini : " + ThisYearCounter.ToString();
        }

        private void AddFuturedReservations()
        {
            foreach(var lavm in _locAccommodationViewModels)
            {
                foreach(var res in _accommodationReservations)
                {
                    if(lavm.AccommodationId == res.AccommodationId && res.UserId == LoggedInUser.Id && DateTime.Today.DayOfYear < res.FirstDay.DayOfYear)
                    {
                        CancelAndMarkResViewModel model = new CancelAndMarkResViewModel(lavm.AccommodationName, lavm.LocationCity, lavm.LocationCountry, res.FirstDay, res.LastDay, res.Id, lavm.AccommodationId, "", res.ReservationDuration, lavm.AccommodationType);
                        _futuredReservations.Add(model);
                    } 
                }
            }
        }

        private void ApplyToCounter(List<AccommodationReservation> _accommodationReservations)
        {
            foreach(var item in _accommodationReservations)
            {
                bool thisYear = false;
                int diff = DateTime.Today.DayOfYear;

                if (DateTime.Today.Year == item.FirstDay.Year) thisYear = true;
                if(item.UserId == LoggedInUser.Id && thisYear && item.FirstDay.DayOfYear <= DateTime.Today.DayOfYear)
                {
                    ThisYearCounter++;
                }
                else if(item.UserId == LoggedInUser.Id && !thisYear && item.FirstDay.DayOfYear >= diff)
                {
                    ThisYearCounter++;
                }
            }
        }

        private void ControlInboxButton(int notifications)
        {
            if (notifications > 0)
            {
                MessageNumberTextBlock = "   " + notifications.ToString();
            }
        }

        private void CollectFinishedReservations()
        {
            AccommodationReservationService service = new AccommodationReservationService();
            List<AccommodationReservation> localList = service.LoadFinishedReservations();

            if(localList.Count > 0)
            {
                foreach (var item in localList)
                {
                    service.DeleteFromFinsihedCSV(item);
                }
            }

            foreach (var item in service.GetAll())
            {
                if (DateTime.Today.DayOfYear > item.LastDay.DayOfYear)
                {
                    service.SaveFinishedReservation(item);
                }
            }
        }

        private void AddFinishedReservations()
        {
            AccommodationReservationService accResService = new AccommodationReservationService();
            foreach(var lavm in _locAccommodationViewModels)
            {
                foreach(var fres in accResService.LoadFinishedReservations())
                {
                    if(lavm.AccommodationId == fres.AccommodationId && fres.UserId == LoggedInUser.Id)
                    {
                        CancelAndMarkResViewModel model = new CancelAndMarkResViewModel(lavm.AccommodationName, lavm.LocationCity, lavm.LocationCountry, fres.FirstDay, fres.LastDay, fres.Id, lavm.AccommodationId, "", fres.ReservationDuration, lavm.AccommodationType);
                        _finishedReservations.Add(model);
                    }
                }
            }
        }

        private void Execute_ShowInbox(object sender)
        {
            GuestInboxWindow newWindow = new GuestInboxWindow(LoggedInUser, ThisWindow, Notifications);
            if (Notifications == 0)
            {
                MessageBox.Show("     Vaš inboks je prazan!\nNemate nepročitanih poruka.", " ", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                newWindow.ShowDialog();
            }
        }

        private void Execute_ShowStatuses(object sender)
        {
            RequestsStatusWindow newWindow = new RequestsStatusWindow(LoggedInUser, ThisWindow, Notifications, false);
            ThisWindow.Close();
            newWindow.ShowDialog();
        }

        private void Execute_ShowMenu(object sender)
        {
            Window helpWindow = new Window();   
            SearchAccommodationWindow newWindow = new SearchAccommodationWindow(LoggedInUser, helpWindow, ThisWindow, Notifications);
            ThisWindow.Close();
            newWindow.ShowDialog();
        }

        private void Execute_SignOut(object sender)
        {
            SignInForm form = new SignInForm();
            ThisWindow.Close();
            form.ShowDialog();
        }
    }
}
