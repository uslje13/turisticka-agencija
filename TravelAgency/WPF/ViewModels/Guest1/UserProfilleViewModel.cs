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
        public int ThisYearCounter { get; set; }
        public int Notifications { get; set; }
        public TextBlock UserName { get; set; }
        public TextBlock Messages { get; set; }
        public TextBlock ReservationsCounter { get; set; }
        public Window ThisWindow { get; set; }
        public List<CancelAndMarkResViewModel> _futuredReservations { get; set; }
        public List<CancelAndMarkResViewModel> _finishedReservations { get; set; }
        public List<AccommodationReservation> _accommodationReservations { get; set; }
        public ObservableCollection<LocAccommodationViewModel> _locAccommodationViewModels { get; set; }
        public RelayCommand ShowMenuCommand { get; set; }
        public RelayCommand ShowRequestsCommand { get; set; }
        public RelayCommand ShowInboxCommand { get; set; }
        public RelayCommand SignOutCommand { get; set; }
        public RelayCommand CanceledQueryCommand { get; set; }


        public UserProfilleViewModel(User user, TextBlock uName, int notifications, TextBlock mess, TextBlock counter, Window window) 
        {
            LoggedInUser = user;
            UserName = uName;
            Messages = mess;
            ThisYearCounter = 0;
            ReservationsCounter = counter;
            ThisWindow = window;
            Notifications = notifications;

            _futuredReservations = new List<CancelAndMarkResViewModel>();
            _finishedReservations = new List<CancelAndMarkResViewModel>();

            AccommodationReservationService accResService = new AccommodationReservationService();
            _accommodationReservations = accResService.GetAll();

            AccommodationService AccommodationService = new AccommodationService();
            _locAccommodationViewModels = AccommodationService.CreateAllDTOForms();

            ApplyToCounter(_accommodationReservations);
            ControlInboxButton(Notifications);
            FillUsernameTextBlock(LoggedInUser);
            AddFuturedReservations();
            CollectFinishedReservations();
            AddFinishedReservations();
            FillCounterTextBlock();

            ShowMenuCommand = new RelayCommand(Execute_ShowMenu);
            ShowRequestsCommand = new RelayCommand(Execute_ShowStatuses);
            ShowInboxCommand = new RelayCommand(Execute_ShowInbox);
            SignOutCommand = new RelayCommand(Execute_SignOut);
            CanceledQueryCommand = new RelayCommand(Execute_ShowCanceledReservations);
        }

        private void Execute_ShowCanceledReservations(object sender)
        {

        }

        private void FillCounterTextBlock()
        {
            Binding binding = new Binding();
            binding.Source = "Broj rezervacija u ovoj godini : " + ThisYearCounter.ToString();
            ReservationsCounter.SetBinding(TextBlock.TextProperty, binding);
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
                if(item.UserId == LoggedInUser.Id && DateTime.Today.Year == item.FirstDay.Year)
                {
                    ThisYearCounter++;
                }
            }
        }

        private void ControlInboxButton(int notifications)
        {
            if (notifications > 0)
            {
                Binding binding = new Binding();
                binding.Source = "   " + notifications.ToString();
                Messages.SetBinding(TextBlock.TextProperty, binding);
                Messages.Foreground = new SolidColorBrush(Colors.DarkGoldenrod);
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
            RequestsStatusWindow newWindow = new RequestsStatusWindow(LoggedInUser, ThisWindow, Notifications);
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

        private void FillUsernameTextBlock(User user)
        {
            Binding binding = new Binding();
            binding.Source = user.Username;
            UserName.SetBinding(TextBlock.TextProperty, binding);
        }
    }
}
