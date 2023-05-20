using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.Views;
using SOSTeam.TravelAgency.WPF.Views.Guest1;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Xml.Linq;
using Xceed.Wpf.AvalonDock.Controls;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest1
{
    public class GuestInboxViewModel
    {
        public User LoggedInUser { get; set; }
        public int Notifications { get; set; }
        public Window ThisWindow { get; set; }
        public Window UserProfilleWindow { get; set; }
        public ObservableCollection<CancelAndMarkResViewModel> _reservationsForMark { get; set; }
        public List<CancelAndMarkResViewModel> _changedReservations { get; set; }
        public ObservableCollection<CancelAndMarkResViewModel> _ratingsFromOwner { get; set; }
        public RelayCommand ShowMenuCommand { get; set; }
        public RelayCommand GoBackCommand { get; set; }
        public RelayCommand MarkAccommodationCommand { get; set; }

        public GuestInboxViewModel(User user, Window window, ListBox list, ListBox newList, ListBox ratings, Window userProfille, int notifications)
        {
            LoggedInUser = user;
            ThisWindow = window;
            UserProfilleWindow = userProfille;
            Notifications = notifications;

            _reservationsForMark = new ObservableCollection<CancelAndMarkResViewModel>();
            _changedReservations = new List<CancelAndMarkResViewModel>();
            _ratingsFromOwner = new ObservableCollection<CancelAndMarkResViewModel>();

            ShowOwnerAnswers();
            ShowGuestRatings();
            PrepareMarkReservationList();
            ShowMarkingNotifications();

            list.ItemsSource = _reservationsForMark;
            newList.ItemsSource = _changedReservations;
            ratings.ItemsSource = _ratingsFromOwner;

            ShowMenuCommand = new RelayCommand(Execute_ShowMenu);
            GoBackCommand = new RelayCommand(Execute_GoBack);
            MarkAccommodationCommand = new RelayCommand(Execute_MarkAccommodation);
        }

        private void Execute_GoBack(object sender)
        {
            ThisWindow.Close();
        }

        private void Execute_ShowMenu(object sender)
        {
            SearchAccommodationWindow newWindow = new SearchAccommodationWindow(LoggedInUser, ThisWindow, UserProfilleWindow, Notifications);
            ThisWindow.Close();
            UserProfilleWindow.Close();
            newWindow.Show();
        }

        private void PrepareMarkReservationList()
        {
            AccommodationService accommodationService = new AccommodationService();
            ObservableCollection<LocAccommodationViewModel> _locAccommodationViewModels = accommodationService.CreateAllDTOForms();
            
            AccommodationReservationService reservationService = new AccommodationReservationService();
            List<AccommodationReservation> _accommodationReservations = reservationService.GetAll();

            foreach (var reserv in _accommodationReservations)
            {
                int diff = DateTime.Today.DayOfYear - reserv.LastDay.DayOfYear;
                foreach (var lavm in _locAccommodationViewModels)
                {
                    if (IsValidCandidate(reserv, diff, lavm))
                    {
                        CancelAndMarkResViewModel crModel = new CancelAndMarkResViewModel(lavm.AccommodationName, lavm.LocationCity, lavm.LocationCountry, reserv.FirstDay, reserv.LastDay, reserv.Id, lavm.AccommodationId);
                        _reservationsForMark.Add(crModel);
                           
                    }
                }
            }
        }

        private bool IsValidCandidate(AccommodationReservation reserv, int diff, LocAccommodationViewModel lavm)
        {
            return reserv.AccommodationId == lavm.AccommodationId && diff > 0 && !reserv.ReadMarkNotification && reserv.UserId == LoggedInUser.Id;
        }

        private void Execute_MarkAccommodation(object sender)
        {
            CancelAndMarkResViewModel? selected = sender as CancelAndMarkResViewModel;
            MarkAccommodationWindow newWindow = new MarkAccommodationWindow(LoggedInUser, selected, _reservationsForMark, _ratingsFromOwner);
            newWindow.ShowDialog();
        }

        private void ShowMarkingNotifications()
        {
            AccommodationReservationService service = new AccommodationReservationService();
            List<AccommodationReservation> finishedReservations = service.LoadFinishedReservations();
            foreach (var item in finishedReservations)
            {
                int diff = DateTime.Today.DayOfYear - item.LastDay.DayOfYear;
                if (diff > 5)
                {
                    RemoveDeadlineReservation(item.Id);
                } 
                else
                {
                    SetMarkingDays(diff, item.Id);
                }
            }
        }

        private void SetMarkingDays(int days, int resId)
        {
            foreach (var item in _reservationsForMark)
            {
                if (resId == item.ReservationId)
                {
                    int diff = 6 - days;
                    if(diff == 1)
                    {
                        item.NotificationShape += diff.ToString() + " dan. Link za ocjenjivanje :";
                    }
                    else
                    {
                        item.NotificationShape += diff.ToString() + " dana. Link za ocjenjivanje :";
                    }
                    break;
                }
            }
        }

        private void RemoveDeadlineReservation(int resId)
        {
            List<CancelAndMarkResViewModel> local = new List<CancelAndMarkResViewModel>();
            foreach(var item in _reservationsForMark)
            {
                local.Add(item);
            }
            foreach(var item in local)
            {
                if(resId == item.ReservationId)
                {
                    _reservationsForMark.Remove(item);
                    break;
                }
            }
        }

        private void ShowOwnerAnswers()
        {
            NotificationFromOwnerService service = new NotificationFromOwnerService();
            UserService userService = new UserService();
            List<NotificationFromOwner> localList = service.GetAll();

            if (localList.Count > 0)
            {
                foreach (var item in localList)
                {
                    if (item.GuestId == LoggedInUser.Id)
                    {
                        User user = userService.GetById(item.OwnerId);
                        string shape = "Vlasnik " + user.Username + " je odgovorio na Vaš zahtjev za pomjeranje rezervacije u smještaju " +
                                        item.AccommodationName + " sa: " + item.Answer + ".";
                        CancelAndMarkResViewModel model = new CancelAndMarkResViewModel();
                        model.NotificationShape = shape;
                        _changedReservations.Add(model);
                        service.Delete(item.Id);
                    }
                }
            }
        }

        private void ShowGuestRatings()
        {
            GuestReviewService reviewService = new GuestReviewService();
            UserService userService = new UserService();
            AccommodationService accommodationService = new AccommodationService();
            AccommodationReservationService reservationService = new AccommodationReservationService();

            List<GuestReview> guestReviews = reviewService.GetAll();

            if(guestReviews.Count > 0)
            {
                foreach(var item in guestReviews)
                {
                    AccommodationReservation reservation = reservationService.GetById(item.ReservationId);
                    if (item.GuestId == LoggedInUser.Id && reservation.ReadMarkNotification)
                    {
                        User owner = userService.GetById(item.OwnerId);
                        Accommodation accommodation = accommodationService.GetById(item.AccommodationId);
                        string shape = "Vlasnik " + owner.Username + " je ocijenio Vašu čistoću u smještaju " + accommodation.Name + 
                                       " u periodu od " + reservation.FirstDay.ToShortDateString() + " do " + reservation.LastDay.ToShortDateString() + 
                                       " ocjenom " + item.CleanlinessGrade + ", a poštovanje pravila sa " + item.RespectGrade + ". ";
                        if (item.Comment.Equals("")) shape += "Nije ostavljao dodatni komentar.";
                        else shape += "Ostavio je i dodatni komentar: " + item.Comment;
                        CancelAndMarkResViewModel model = new CancelAndMarkResViewModel();
                        model.NotificationShape = shape;
                        _ratingsFromOwner.Add(model);
                    }
                }
            }
        }
    }
}
