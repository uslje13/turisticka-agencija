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

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest1
{
    public class UserProfilleViewModel
    {
        public User LoggedInUser { get; set; }
        public TextBlock userName { get; set; }
        public RelayCommand goToSearchCommand { get; set; }
        public RelayCommand goToRequestsStatus { get; set; }
        public RelayCommand goToInboxCommand { get; set; }
        public RelayCommand SignOutCommand { get; set; }
        public Button InboxButton { get; set; }
        public TextBlock Messages { get; set; }
        public List<CancelAndMarkResViewModel> futuredReservations { get; set; }
        public List<CancelAndMarkResViewModel> finishedReservations { get; set; }
        public List<AccommodationReservation> accommodationReservations { get; set; }
        public List<LocAccommodationViewModel> locAccommodationViewModels { get; set; }
        public AccommodationReservationService accResService { get; set; }
        public AccommodationService AccommodationService { get; set; }
        public List<Accommodation> accommodations { get; set; }
        public LocationService LocationService { get; set; }
        public List<Location> locations { get; set; }
        public int ThisYearCounter { get; set; }
        public TextBlock ReservationsCounter { get; set;  }
        public Window ThisWindow { get; set; }
        public RelayCommand CanceledReservationsCommand { get; set; }
        public int Notifications { get; set; }


        public UserProfilleViewModel(User user, TextBlock uName, Button button, int notifications, TextBlock mess, TextBlock counter, Window window) 
        {
            LoggedInUser = user;
            userName = uName;
            InboxButton = button;
            Messages = mess;
            ThisYearCounter = 0;
            ReservationsCounter = counter;
            ThisWindow = window;
            Notifications = notifications;

            locAccommodationViewModels = new List<LocAccommodationViewModel>();
            futuredReservations = new List<CancelAndMarkResViewModel>();
            finishedReservations = new List<CancelAndMarkResViewModel>();

            accResService = new AccommodationReservationService();
            accommodationReservations = accResService.GetAll();

            AccommodationService = new AccommodationService();
            accommodations = AccommodationService.GetAll();

            LocationService = new LocationService();
            locations = LocationService.GetAll();

            ApplyToCounter(accommodationReservations);
            ControlInboxButton(notifications);
            FillTextBlock(LoggedInUser);
            MergeLocationsAndAccommodations();
            CollectFuturedReservations();
            CollectFinishedReservations();
            AddToBindingList();
            FillCounterTextBlock();
            
            goToSearchCommand = new RelayCommand(ExecuteGoToSearch);
            goToRequestsStatus = new RelayCommand(ExecuteGoToStatuses);
            goToInboxCommand = new RelayCommand(ExecuteInboxShowing);
            SignOutCommand = new RelayCommand(Execute_SigingOut);
            CanceledReservationsCommand = new RelayCommand(Execute_ShowCanceledReservations);
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

        private void CollectFuturedReservations()
        {
            foreach(var lavm in locAccommodationViewModels)
            {
                foreach(var res in accommodationReservations)
                {
                    if(lavm.AccommodationId == res.AccommodationId && res.UserId == LoggedInUser.Id && DateTime.Today.DayOfYear < res.FirstDay.DayOfYear)
                    {
                        CancelAndMarkResViewModel model = new CancelAndMarkResViewModel(lavm.AccommodationName, lavm.LocationCity,
                                                                                        lavm.LocationCountry, res.FirstDay,
                                                                                        res.LastDay, res.Id, lavm.AccommodationId, "", res.ReservationDuration,
                                                                                        lavm.AccommodationType);
                        futuredReservations.Add(model);
                    } 
                }
            }
        }

        private void ApplyToCounter(List<AccommodationReservation> accommodationReservations)
        {
            foreach(var item in  accommodationReservations)
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

        private void AddToBindingList()
        {
            foreach(var lavm in locAccommodationViewModels)
            {
                foreach(var fres in accResService.LoadFinishedReservations())
                {
                    if(lavm.AccommodationId == fres.AccommodationId && fres.UserId == LoggedInUser.Id)
                    {
                        CancelAndMarkResViewModel model = new CancelAndMarkResViewModel(lavm.AccommodationName, lavm.LocationCity,
                                                                                        lavm.LocationCountry, fres.FirstDay,
                                                                                        fres.LastDay, fres.Id, lavm.AccommodationId, "", fres.ReservationDuration,
                                                                                        lavm.AccommodationType);
                        finishedReservations.Add(model);
                    }
                }
            }
        }

        private void ExecuteInboxShowing(object sender)
        {
            GuestInboxWindow newWindow = new GuestInboxWindow(LoggedInUser, ThisWindow);
            if (Notifications == 0)
            {
                MessageBox.Show("     Vaš inboks je prazan!\nNemate nepročitanih poruka.", " ", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
                newWindow.ShowDialog();
        }

        private void ExecuteGoToStatuses(object sender)
        {
            RequestsStatusWindow newWindow = new RequestsStatusWindow(LoggedInUser);
            newWindow.Show();
        }

        private void ExecuteGoToSearch(object sender)
        {
            Window helpWindow = new Window();   
            SearchAccommodationWindow newWindow = new SearchAccommodationWindow(LoggedInUser, helpWindow, ThisWindow);
            newWindow.Show();
        }

        private void Execute_SigingOut(object sender)
        {
            SignInForm form = new SignInForm();
            ThisWindow.Close();
            form.ShowDialog();
        }

        private void FillTextBlock(User user)
        {
            Binding binding = new Binding();
            binding.Source = user.Username;
            userName.SetBinding(TextBlock.TextProperty, binding);
        }

        private void MergeLocationsAndAccommodations()
        {
            locAccommodationViewModels.Clear();
            foreach (var accommodation in accommodations)
            {
                foreach (var location in locations)
                {
                    if (accommodation.LocationId == location.Id)
                    {
                        LocAccommodationViewModel dto = CreateLocAccForm(accommodation, location);

                        locAccommodationViewModels.Add(dto);
                    }
                }
            }
        }

        private LocAccommodationViewModel CreateLocAccForm(Accommodation acc, Location loc)
        {
            int currentGuestNumber = 0;
            foreach (var item in accommodationReservations)
            {
                if (item.AccommodationId == acc.Id)
                {
                    DateTime today = DateTime.Today;
                    int helpVar1 = today.DayOfYear - item.FirstDay.DayOfYear;
                    int helpVar2 = today.DayOfYear - item.LastDay.DayOfYear;
                    if (helpVar1 >= 0 && helpVar2 <= 0)
                    {
                        currentGuestNumber += item.GuestNumber;
                    }
                }
            }
            LocAccommodationViewModel dto = new LocAccommodationViewModel(acc.Id, acc.Name, loc.City, loc.Country, FindAccommodationType(acc),
                                                        acc.MaxGuests, acc.MinDaysStay, currentGuestNumber, false);
            return dto;
        }

        private LocAccommodationViewModel.AccommType FindAccommodationType(Accommodation acc)
        {
            if (acc.Type == Accommodation.AccommodationType.APARTMENT)
                return LocAccommodationViewModel.AccommType.APARTMENT;
            else if (acc.Type == Accommodation.AccommodationType.HOUSE)
                return LocAccommodationViewModel.AccommType.HOUSE;
            else if (acc.Type == Accommodation.AccommodationType.HUT)
                return LocAccommodationViewModel.AccommType.HUT;
            else
                return LocAccommodationViewModel.AccommType.NOTYPE;
        }
    }
}
