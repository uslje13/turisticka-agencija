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
using System.ComponentModel;
using static System.Net.Mime.MediaTypeNames;
using System.Runtime.Intrinsics.X86;
using System.Windows.Documents;
using System.Windows.Shapes;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest1
{
    public class UserProfilleViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
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
        public string SuperGuestInformation { get; set; }
        public bool FirstLogging { get; set; }
        
        private bool isPopupOpen;
        public bool IsPopupOpen
        {
            get { return isPopupOpen; }
            set
            {
                isPopupOpen = value;
                OnPropertyChaged("IsPopupOpen");
            }
        }
        private bool commandsEnabled;
        public bool CommandsEnabled
        {
            get { return commandsEnabled; }
            set
            {
                commandsEnabled = value;
                OnPropertyChaged("CommandsEnabled");
            }
        }
        public List<CancelAndMarkResViewModel> _futuredReservations { get; set; }
        public List<CancelAndMarkResViewModel> _finishedReservations { get; set; }
        public List<AccommodationReservation> _accommodationReservations { get; set; }
        public ObservableCollection<LocAccommodationViewModel> _locAccommodationViewModels { get; set; }
        public RelayCommand ShowMenuCommand { get; set; }
        public RelayCommand ShowRequestsCommand { get; set; }
        public RelayCommand ShowInboxCommand { get; set; }
        public RelayCommand SignOutCommand { get; set; }
        public RelayCommand CanceledQueryCommand { get; set; }
        public RelayCommand SuperGuestInfoCommand { get; set; }


        public UserProfilleViewModel(User user, int notifications, Window window) 
        {
            LoggedInUser = user;
            UsernameTextBlock = user.Username;
            ThisYearCounter = 0;
            ThisWindow = window;
            Notifications = notifications;
            FirstLogging = false;
            IsPopupOpen = false;
            CommandsEnabled = true;

            _futuredReservations = new List<CancelAndMarkResViewModel>();
            _finishedReservations = new List<CancelAndMarkResViewModel>();

            AccommodationReservationService accResService = new AccommodationReservationService();
            _accommodationReservations = accResService.GetAll();

            AccommodationService AccommodationService = new AccommodationService();
            _locAccommodationViewModels = AccommodationService.CreateAllDTOForms();

            FillSuperGuestInfoPopup();
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
            SuperGuestInfoCommand = new RelayCommand(Execute_OpenPopup);
        }

        private void FillSuperGuestInfoPopup()
        {
            SuperGuestInformation = "Gost može postati super-gost ako u prethodnoj godini ima bar 10 rezervacija.\n"
                                  + "Super-gost titula traje godinu dana i prestaje da važi ako gost\n"
                                  + "ne bude ponovo zadovoljio uslov od 10 rezervacija.\n"
                                  + "Super-gost dobija 5 bonus poena koje može potrošiti u tekućoj godini,\n"
                                  + "nakon čega se bodovi resetuju na 0 (ne mogu se akumulirati).\n"
                                  + "Prilikom svake naredne rezervacije se troši jedan bonus poen što donosi popuste,\n"
                                  + "što znači da će super-gost imati 5 rezervacija sa popustom.";
        }

        private void Execute_OpenPopup(object sender)
        {
            if (!IsPopupOpen)
            {
                IsPopupOpen = true;
                CommandsEnabled = false;
            }
            else
            {
                IsPopupOpen = false;
                CommandsEnabled = true;
            }
        }

        protected void OnPropertyChaged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void FindSuperGuestInformations()
        {
            SuperGuestService superGuestService = new SuperGuestService();
            List<SuperGuest> superGuests = superGuestService.GetAll();
            foreach(SuperGuest superGuest in superGuests)
            {
                if(superGuest.UserId == LoggedInUser.Id)
                {
                    DisplaySuperGuestStatus(superGuest);
                    break;
                }
            }
        }

        private void DisplaySuperGuestStatus(SuperGuest superGuest)
        {
            SuperGuestBonusPoints = "Super gost : " + superGuest.BonusPoints.ToString();
            if (superGuest.BonusPoints == 1) SuperGuestBonusPoints += " bonus poen";
            else SuperGuestBonusPoints += " bonus poena";
            SuperGuestReservations = "Broj rezervacija u prošloj godini : " + superGuest.LastYearReservationsNumber.ToString();
            if (superGuest.LastYearReservationsNumber < 10) SuperGuestConclusion = "U prošloj godini niste ispunili potrebnu normu.";
            else SuperGuestConclusion = "U prošloj godini ste ostvarili status super-gosta.";
        }

        private void CreateSuperGuestAccounts()
        {
            AccommodationReservationService reservationService = new AccommodationReservationService();
            SuperGuestService superGuestService = new SuperGuestService();
            UserService userService = new UserService();
            superGuestService.ClearSuperGuestCSV();
            List<User> _users = userService.GetAll();

            foreach (User user in _users)
            {
                if(user.Role == Roles.GUEST1)
                {
                    int resNumber = reservationService.FindLastYearReservationsNumber(user);
                    int points = superGuestService.InitializeBonusPoints(resNumber);
                    bool isSuper = superGuestService.IntializeSuperStatus(resNumber);
                    points = superGuestService.CalculateUnusedBonusPoints(user, points);
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
