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
        public string SuperGuestBonusPoints { get; set; }
        public string SuperGuestReservations { get; set; }
        public string SuperGuestConclusion { get; set; }
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

            ApplyToThisYearCounter(_accommodationReservations);
            ControlInboxButton(Notifications);
            AddFuturedReservations();
            CollectFinishedReservations();
            AddFinishedReservations();
            FillCounterTextBlock();
            IsFirstLogging();
            CreateSuperGuestAccounts();
            FindSuperGuestInformations();

            ShowMenuCommand = new RelayCommand(Execute_ShowMenu);
            ShowRequestsCommand = new RelayCommand(Execute_ShowStatuses);
            ShowInboxCommand = new RelayCommand(Execute_ShowInbox);
            SignOutCommand = new RelayCommand(Execute_SignOut);
            CanceledQueryCommand = new RelayCommand(Execute_ShowCanceledReservations);
        }

        private void FindSuperGuestInformations()
        {
            SuperGuestService superGuestService = new SuperGuestService();
            List<SuperGuest> superGuests = superGuestService.GetAll();
            foreach(SuperGuest superGuest in superGuests)
            {
                if(superGuest.UserId == LoggedInUser.Id)
                {
                    SuperGuestBonusPoints = "Super gost : " + superGuest.BonusPoints.ToString();
                    if(superGuest.BonusPoints == 1) SuperGuestBonusPoints += " bonus poen";
                    else SuperGuestBonusPoints += " bonus poena";
                    SuperGuestReservations = "Broj rezervacija u prošloj godini: " + superGuest.LastYearReservationsNumber.ToString();
                    Conclude(superGuest.LastYearReservationsNumber);
                    break;
                }
            }
        }

        private void Conclude(int reservationsNumber)
        {
            if (reservationsNumber < 10) SuperGuestConclusion = "U prošloj godini niste ispunili potrebnu normu.";
            else SuperGuestConclusion = "U prošloj godini ste ostvarili status super-gosta.";
        }

        private void ClearSuperGuestCSV()
        {
            SuperGuestService superGuestService = new SuperGuestService();
            List<SuperGuest> _superGuests = superGuestService.GetAll();
            if (_superGuests.Count > 0)
            {
                foreach (var guest in _superGuests)
                {
                    superGuestService.Delete(guest.Id);
                }
            }
        }

        private int FindLastYearReservationsNumber(User user)
        {
            int resNumber = 0;
            foreach (var reservation in _accommodationReservations)
            {
                if (reservation.UserId == user.Id && reservation.FirstDay.Year == DateTime.Today.Year - 1)
                {
                    resNumber++;
                }
            }
            return resNumber;
        }

        private int InvestigateShortTimeCSV(User user)
        {
            AccommodationReservationService reservationService = new AccommodationReservationService();
            List<AccommodationReservation> _processedReservations = reservationService.LoadFromOtherCSV();
            int resNumber = 0;
            foreach(var reservation in _processedReservations)
            {
                if(user.Id == reservation.UserId && reservation.FirstDay.Year == DateTime.Today.Year - 1 && !reservation.DefinitlyChanged)
                {
                    resNumber++;
                }
            }
            return resNumber;
        }

        private int ApplyToBonusCounter(User user, int points)
        {
            foreach (var reservation in _accommodationReservations)
            {
                if (reservation.UserId == user.Id && reservation.FirstDay.Year == DateTime.Today.Year && points > 0)
                {
                    points--;
                }
            }
            return points;
        }

        private void CreateSuperGuestAccounts()
        {
            SuperGuestService superGuestService = new SuperGuestService();
            UserService userService = new UserService();
            List<User> _users = userService.GetAll();
            ClearSuperGuestCSV();

            foreach (User user in _users)
            {
                if(user.Role == Roles.GUEST1)
                {
                    int resNumber = FindLastYearReservationsNumber(user);
                    resNumber += InvestigateShortTimeCSV(user);
                    int points = 0;
                    bool isSuper = false;
                    if (resNumber >= 10)
                    {
                        points = 5;
                        isSuper = true;
                    }
                    points = ApplyToBonusCounter(user, points);
                    SuperGuest superGuest = new SuperGuest(user.Id, user.Username, points, resNumber, isSuper);
                    superGuestService.Save(superGuest);
                }
            }
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
            CounterTextBlock = "Broj rezervacija u ovoj godini : " + ThisYearCounter.ToString();
        }

        private void AddFuturedReservations()
        {
            foreach(var lavm in _locAccommodationViewModels)
            {
                foreach(var res in _accommodationReservations)
                {
                    if(lavm.AccommodationId == res.AccommodationId && res.UserId == LoggedInUser.Id && DateTime.Today < res.FirstDay)
                    {
                        CancelAndMarkResViewModel model = new CancelAndMarkResViewModel(lavm.AccommodationName, lavm.LocationCity, lavm.LocationCountry, res.FirstDay, res.LastDay, res.Id, lavm.AccommodationId, "", res.ReservationDuration, lavm.AccommodationType);
                        _futuredReservations.Add(model);
                    } 
                }
            }
        }

        private void ApplyToThisYearCounter(List<AccommodationReservation> _accommodationReservations)
        {
            foreach(var item in _accommodationReservations)
            {
                if (item.UserId == LoggedInUser.Id && DateTime.Today.Year == item.FirstDay.Year)
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
                if (DateTime.Today > item.LastDay)
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
