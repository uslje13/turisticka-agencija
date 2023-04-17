using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.Views.Guest1;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Xml.Linq;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest1
{
    public class GuestInboxViewModel
    {
        public User LoggedInUser { get; set; }
        public RelayCommand MarkAccommodationCommand { get; set; }
        public List<CancelAndMarkResViewModel> reservationsForMark { get; set; }
        public AccommodationReservationService accResService { get; set; }
        public List<AccommodationReservation> accommodationReservations { get; set; }
        public List<LocAccommodationViewModel> locAccommodationViewModels { get; set; }
        public AccommodationService AccommodationService { get; set; }
        public List<Accommodation> accommodations { get; set; }
        public LocationService LocationService { get; set; }
        public List<Location> locations { get; set; }
        public Window ThisWindow { get; set; }

        public GuestInboxViewModel(User user, Window window)
        {
            LoggedInUser = user;
            ThisWindow = window;

            reservationsForMark = new List<CancelAndMarkResViewModel>();
            locAccommodationViewModels = new List<LocAccommodationViewModel>();

            accResService = new AccommodationReservationService();
            accommodationReservations = accResService.GetAll();

            AccommodationService = new AccommodationService();
            accommodations = AccommodationService.GetAll();

            LocationService = new LocationService();
            locations = LocationService.GetAll();

            ShowNotificationsFromOwner();
            MergeLocationsAndAccommodations();
            PrepareMarkReservationList();
            ShowMarkingNotifications();

            MarkAccommodationCommand = new RelayCommand(ExecuteAccommodationMarking);
        }

        private void PrepareMarkReservationList()
        {
            foreach (var reserv in accommodationReservations)
            {
                int diff = DateTime.Today.DayOfYear - reserv.LastDay.DayOfYear;
                foreach (var lavm in locAccommodationViewModels)
                {
                    if (IsValidCandidateForMarkList(reserv, diff, lavm))
                    {
                        CancelAndMarkResViewModel crModel = new CancelAndMarkResViewModel(lavm.AccommodationName, lavm.LocationCity,
                                                                                          lavm.LocationCountry, reserv.FirstDay, reserv.LastDay,
                                                                                          reserv.Id, lavm.AccommodationId);
                        reservationsForMark.Add(crModel);
                           
                    }
                }
            }
        }

        private bool IsValidCandidateForMarkList(AccommodationReservation reserv, int diff, LocAccommodationViewModel lavm)
        {
            return reserv.AccommodationId == lavm.AccommodationId && diff > 0 && !reserv.ReadMarkNotification && reserv.UserId == LoggedInUser.Id;
        }

        private void ExecuteAccommodationMarking(object sender)
        {
            CancelAndMarkResViewModel? selected = sender as CancelAndMarkResViewModel;
            MarkAccommodationWindow newWindow = new MarkAccommodationWindow(LoggedInUser, selected);
            newWindow.ShowDialog();
            ThisWindow.Close();
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
                    RemoveDeadlineEndedReservation(item.Id);
                } 
                else
                {
                    SetDaysForMarking(diff, item.Id);
                }
            }
        }

        private void SetDaysForMarking(int days, int resId)
        {
            foreach (var item in reservationsForMark)
            {
                if (resId == item.ReservationId)
                {
                    int diff = 6 - days;
                    item.DaysForMarking = diff.ToString() + " dana";
                    break;
                }
            }
        }

        private void RemoveDeadlineEndedReservation(int resId)
        {
            List<CancelAndMarkResViewModel> local = new List<CancelAndMarkResViewModel>();
            foreach(var item in reservationsForMark)
            {
                local.Add(item);
            }
            foreach(var item in local)
            {
                if(resId == item.ReservationId)
                {
                    reservationsForMark.Remove(item);
                    break;
                }
            }
        }

        private void ShowNotificationsFromOwner()
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
                        MessageBox.Show("Vlasnik " + user.Username + " je odgovorio na Vaš zahtjev za pomjeranje rezervacije u smještaju " +
                                        item.AccommodationName + " sa: " + item.Answer + ".");
                        service.Delete(item.Id);
                    }
                }
            }
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
                                                        acc.MaxGuests, acc.MinDaysStay, currentGuestNumber,false);
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
