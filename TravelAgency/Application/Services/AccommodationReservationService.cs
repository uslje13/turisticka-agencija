using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Domain.RepositoryInterfaces;
using SOSTeam.TravelAgency.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOSTeam.TravelAgency.Repositories;
using System.Windows;
using SOSTeam.TravelAgency.WPF.ViewModels.Guest1;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;
using SOSTeam.TravelAgency.WPF.Views;
using System.Windows.Controls;
using SOSTeam.TravelAgency.WPF.Views.Guest1;

namespace SOSTeam.TravelAgency.Application.Services
{
    public class AccommodationReservationService
    {
        public DateTime FirstDate { get; set; }
        public DateTime LastDate { get; set; }
        public int DaysDuration { get; set; }
        public LocAccommodationViewModel DTO { get; set; }

        private readonly IAccReservationRepository _accReservationRepository = Injector.CreateInstance<IAccReservationRepository>();
        private readonly IAccommodationRepository _accommodationRepository = Injector.CreateInstance<IAccommodationRepository>();
        private readonly IChangedResRequestRepositroy _changedResRequestRepositroy = Injector.CreateInstance<IChangedResRequestRepositroy>();
        private readonly IWantedNewDateRepository _wantedNewDateRepository = Injector.CreateInstance<IWantedNewDateRepository>();
        public AccReservationViewModel forwardedItem { get; set; }
        public User LoggedInUser { get; set; }
        public List<AccommodationReservation> accommodationReservations { get; set; }
        public int guestNumber { get; set; }
        public bool IsEnteredOfChange { get; set; }
        public ChangedReservationRequest selectedReservation { get; set; }


        public AccommodationReservationService(LocAccommodationViewModel dto, User user, DateTime fDay, DateTime lDay, int days, bool isEnteredOfChange, ChangedReservationRequest request)
        {
            DTO = dto;
            FirstDate = fDay;
            LastDate = lDay;
            DaysDuration = days;
            LoggedInUser = user;
            IsEnteredOfChange = isEnteredOfChange;
            selectedReservation = request;
        }

        public AccommodationReservationService(AccReservationViewModel item, User user, int forwadedGuestNumber)
        {
            forwardedItem = item;
            LoggedInUser = user;
            guestNumber = forwadedGuestNumber;

            accommodationReservations = _accReservationRepository.GetAll();
        }

        public AccommodationReservationService(AccReservationViewModel item, User user, int forwadedGuestNumber, ChangedReservationRequest SelectedReservation)
        {
            forwardedItem = item;
            LoggedInUser = user;
            guestNumber = forwadedGuestNumber;
            selectedReservation = SelectedReservation;

            accommodationReservations = _accReservationRepository.GetAll();
        }

        public AccommodationReservationService() 
        { 
            
        }
        public void Delete(int id)
        {
            _accReservationRepository.Delete(id);
        }

        public List<AccommodationReservation> GetAll()
        {
            return _accReservationRepository.GetAll();
        }

        public AccommodationReservation GetById(int id)
        {
            return _accReservationRepository.GetById(id);
        }

        public void Save(AccommodationReservation accommodationReservation)
        {
            _accReservationRepository.Save(accommodationReservation);
        }

        public void Update(AccommodationReservation accommodationReservation)
        {
            _accReservationRepository.Update(accommodationReservation);
        }

        private void AddReservation(DateTime start, DateTime end, int guests, int days, int accId)
        {
            AccommodationReservation reservation = new AccommodationReservation(start, end, days, guests, accId, LoggedInUser.Id);
            AccommodationReservationRepository reservationRepository = new AccommodationReservationRepository();
            reservationRepository.Save(reservation);
            MessageBox.Show("Uspešno rezervisano.");
        }

        public void ExecuteReserveAccommodation()
        {
            int appropriateGuestNumber = FindAppropriateGuestsNumber();

            if (guestNumber > 0)
            {
                int helpVar = appropriateGuestNumber + guestNumber;
                if (helpVar > forwardedItem.AccommodationMaxGuests)
                {
                    MessageBox.Show("Prekoračen je maksimalni broj gostiju za ovaj smeštaj. Pokušajte ponovo.");
                }
                else
                {
                    AddReservation(forwardedItem.ReservationFirstDay, forwardedItem.ReservationLastDay, guestNumber,
                                    forwardedItem.ReservationDuration, forwardedItem.AccommodationId);
                }
            }
            else
            {
                MessageBox.Show("Ne možete izvršiti rezervaciju za 0 osoba.");
            }
        }

        public void ExecuteSendRequestForChange()
        {
            int appropriateGuestNumber = FindAppropriateGuestsNumber();

            if (guestNumber > 0)
            {
                int helpVar = appropriateGuestNumber + guestNumber;
                if (helpVar > forwardedItem.AccommodationMaxGuests)
                {
                    MessageBox.Show("Prekoračen je maksimalni broj gostiju za ovaj smeštaj. Pokušajte ponovo.");
                }
                else
                {
                    Accommodation acc = _accommodationRepository.GetById(forwardedItem.AccommodationId);
                    ProcessReservation();
                    //SendRequestToOwner(acc.OwnerId);
                    MessageBox.Show("Zahtjev za pomjeranje rezervacije je uspješno poslat vlasniku.");
                }
            }
            else
            {
                MessageBox.Show("Ne možete izvršiti rezervaciju za 0 osoba.");
            }
        }

        private void ProcessReservation()
        {
            WantedNewDate wanted = new WantedNewDate(forwardedItem.AccommodationId, forwardedItem.AccommodationName,
                                                      forwardedItem.AccommodationMinDaysStay, forwardedItem.ReservationFirstDay,
                                                      forwardedItem.ReservationLastDay, forwardedItem.ReservationDuration,
                                                      forwardedItem.AccommodationMaxGuests, forwardedItem.CurrentGuestNumber);

            _wantedNewDateRepository.Save(wanted);
            selectedReservation.status = ChangedReservationRequest.Status.ON_HOLD;
            selectedReservation.ownerComment = "Komentar nije dostupan";
            _changedResRequestRepositroy.Save(selectedReservation);
        }

        public void SendRequestToOwner(int ownerId)
        {
            List<ChangedReservationRequest> list = _changedResRequestRepositroy.GetAll();
            List<WantedNewDate> wantedDates = _wantedNewDateRepository.GetAll();
            foreach(var item in list)
            {
                Accommodation accommodation = _accommodationRepository.GetById(item.AccommodationId);
                foreach(var item2 in wantedDates)
                {
                    if(accommodation.Id == item2.wantedDate.AccommodationId)
                    {
                        if (accommodation.OwnerId == ownerId)
                        {
                            AnswerToGuestWindow newWindow = new AnswerToGuestWindow(item2, item);
                            newWindow.ShowDialog();
                        }
                    }
                }
            }
        }

        private int FindAppropriateGuestsNumber()
        {
            int appropriateGuestNumber = 0;
            foreach (var item in accommodationReservations)
            {
                if (item.AccommodationId == forwardedItem.AccommodationId)
                {
                    DateTime today = DateTime.Today;
                    int helpVar1 = today.DayOfYear - forwardedItem.ReservationFirstDay.DayOfYear;
                    int helpVar2 = today.DayOfYear - forwardedItem.ReservationLastDay.DayOfYear;
                    if (helpVar1 >= 0 && helpVar2 <= 0)
                    {
                        appropriateGuestNumber += item.GuestNumber;
                    }
                }
            }

            return appropriateGuestNumber;
        }

        public void ExecuteSearchingDates()
        {
            bool validDates = CheckDates(FirstDate, LastDate);
            bool validDays = CheckDays();
            if (validDates && validDays)
            {
                ShowAvailableDatesWindow availableDates = new ShowAvailableDatesWindow(DTO, FirstDate, LastDate, DaysDuration, LoggedInUser, IsEnteredOfChange, selectedReservation);
                availableDates.Show();
            }
            else if (!validDates)
            {
                MessageBox.Show("Nevalidan odabir datuma. Pokušajte ponovo.");
            }
            else if (!validDays)
            {
                MessageBox.Show("Unešeni broj dana boravka je manji od minimalnog za izabrani smeštaj.");
            }
        }

        private bool CheckDays()
        {
            int check = DTO.AccommodationMinDaysStay;
            if (DaysDuration >= check) return true;
            else return false;
        }

        private bool CheckDates(DateTime start, DateTime end)
        {
            if (start.Year < end.Year) return true;
            else if (start.Year == end.Year)
            {
                if (start.Month < end.Month) return true;
                else if (start.Month == end.Month)
                {
                    if (start.Day <= end.Day) return true;
                    else return false;
                }
                else return false;
            }
            else return false;
        }
    }
}
