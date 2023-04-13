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
using System.Runtime.InteropServices;

namespace SOSTeam.TravelAgency.Application.Services
{
    public class AccommodationReservationService
    {

        public struct ReservationsInformations
                {
                    public List<ChangedReservationRequest> requests { get; set; }
                    public List<WantedNewDate> newDates { get; set; }

                    public ReservationsInformations()
                    {
                        requests = new List<ChangedReservationRequest>();
                        newDates = new List<WantedNewDate>();
                    }

                    public ReservationsInformations(List<ChangedReservationRequest> requests, List<WantedNewDate> newDates)
                    {
                        this.requests = requests;
                        this.newDates = newDates;
                    }
                }

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
        public AccommodationReservation selectedReservationCopy { get; set; }


        public AccommodationReservationService(LocAccommodationViewModel dto, User user, DateTime fDay, DateTime lDay, int days, bool isEnteredOfChange, ChangedReservationRequest request, AccommodationReservation reservation)
        {
            DTO = dto;
            FirstDate = fDay;
            LastDate = lDay;
            DaysDuration = days;
            LoggedInUser = user;
            IsEnteredOfChange = isEnteredOfChange;
            selectedReservation = request;
            selectedReservationCopy = reservation;

            accommodationReservations = _accReservationRepository.GetAll();
        }

        public AccommodationReservationService(AccReservationViewModel item, User user, int forwadedGuestNumber)
        {
            forwardedItem = item;
            LoggedInUser = user;
            guestNumber = forwadedGuestNumber;

            accommodationReservations = _accReservationRepository.GetAll();
        }

        public AccommodationReservationService(AccReservationViewModel item, User user, int forwadedGuestNumber, ChangedReservationRequest SelectedReservation, AccommodationReservation reservation)
        {
            forwardedItem = item;
            LoggedInUser = user;
            guestNumber = forwadedGuestNumber;
            selectedReservation = SelectedReservation;
            selectedReservationCopy = reservation;

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
            accommodationReservations = _accReservationRepository.GetAll();
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

        public void SaveToOtherCSV(AccommodationReservation reservation)
        {
            _accReservationRepository.SaveToOtherCSV(reservation);
        }

        public void DeleteFromOtherCSV(AccommodationReservation reservation)
        {
            _accReservationRepository.DeleteFromOtherCSV(reservation);
        }

        public List<AccommodationReservation> LoadFromOtherCSV()
        {
            return _accReservationRepository.LoadFromOtherCSV();
        }

        public void Save(AccommodationReservation accommodationReservation)
        {
            _accReservationRepository.Save(accommodationReservation);
        }

        public void SaveChangeAcceptedReservation(AccommodationReservation reservation)
        {
            _accReservationRepository.SaveChangeAcceptedReservation(reservation);
        }

        public void Update(AccommodationReservation accommodationReservation)
        {
            _accReservationRepository.Update(accommodationReservation);
        }

        private void AddReservation(DateTime start, DateTime end, int guests, int days, int accId)
        {
            AccommodationReservation reservation = new AccommodationReservation(start, end, days, guests, accId, LoggedInUser.Id);
            _accReservationRepository.Save(reservation);
            MessageBox.Show("Uspješno rezervisano.");
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
                                                      forwardedItem.AccommodationMaxGuests, forwardedItem.CurrentGuestNumber, 
                                                      LoggedInUser.Id, selectedReservation.reservationId);

            _wantedNewDateRepository.Save(wanted);
            selectedReservation.NewFirstDay = forwardedItem.ReservationFirstDay;
            selectedReservation.NewLastDay = forwardedItem.ReservationLastDay;
            selectedReservation.status = ChangedReservationRequest.Status.ON_HOLD;
            selectedReservation.ownerComment = "Komentar nije dostupan";
            _changedResRequestRepositroy.Save(selectedReservation);
        }

        public void /*ReservationsInformations*/ SendRequestToOwner(int ownerId)
        {
            List<ChangedReservationRequest> processedReservations = _changedResRequestRepositroy.GetAll();
            List<WantedNewDate> wantedDates = _wantedNewDateRepository.GetAll();

            ReservationsInformations reservationsInformations = new ReservationsInformations();
            foreach(var item in processedReservations)
            {
                Accommodation accommodation = _accommodationRepository.GetById(item.AccommodationId);
                foreach(var item2 in wantedDates)
                {
                    if(accommodation.Id == item2.wantedDate.AccommodationId && item.UserId == item2.UserId && item.reservationId == item2.OldReservationId && item.status == ChangedReservationRequest.Status.ON_HOLD)
                    {
                        if (accommodation.OwnerId == ownerId)
                        {
                            //reservationsInformations.requests.Add(item);
                            //reservationsInformations.newDates.Add(item2);
                            AnswerToGuestWindow newWindow = new AnswerToGuestWindow(item2, item);
                            newWindow.ShowDialog();
                        }
                    }
                }
            }

            //return reservationsInformations;
        }

        public void acceptReservationChanges(WantedNewDate newReservation, ChangedReservationRequest oldReservation)
        {
            AccommodationReservation reservation = new AccommodationReservation(newReservation.wantedDate.ReservationFirstDay, newReservation.wantedDate.ReservationLastDay,
                                                                                newReservation.wantedDate.ReservationDuration, newReservation.wantedDate.AccommodationMaxGuests,
                                                                                newReservation.wantedDate.AccommodationId, oldReservation.UserId);
            _accReservationRepository.SaveChangeAcceptedReservation(reservation);
            _changedResRequestRepositroy.Delete(oldReservation.Id);
            ChangedReservationRequest processedReservation = new ChangedReservationRequest(oldReservation.reservationId, oldReservation.AccommodationId,
                                                                                            oldReservation.AccommodationName, oldReservation.City, oldReservation.Country,
                                                                                            oldReservation.OldFirstDay, oldReservation.OldLastDay,
                                                                                            oldReservation.GuestNumber, oldReservation.UserId);
            processedReservation.NewFirstDay = newReservation.wantedDate.ReservationFirstDay;
            processedReservation.NewLastDay = newReservation.wantedDate.ReservationLastDay;
            processedReservation.status = ChangedReservationRequest.Status.ACCEPTED;
            processedReservation.ownerComment = "Rezervacija je uspješno pomjerena.";
            _changedResRequestRepositroy.Save(processedReservation);

            _wantedNewDateRepository.Delete(newReservation.Id);
            MessageBox.Show("Zahtjev je prihvaćen.");
        }
        
        public void declineReservationChanges(string ownerComment, WantedNewDate newReservation, ChangedReservationRequest oldReservation)
        {
            if(ownerComment.Equals(""))
            {
                MessageBox.Show("Obzirom da odbijate zahtjev, morate ostaviti komentar.");
            }
            else
            {
                List<AccommodationReservation> helpList = _accReservationRepository.LoadFromOtherCSV();
                AccommodationReservation reservation = new AccommodationReservation();
                foreach (var item in helpList)
                {
                    if (item.FirstDay == oldReservation.OldFirstDay && item.LastDay == oldReservation.OldLastDay && item.Id == oldReservation.reservationId)
                    {
                        reservation = item;
                        _accReservationRepository.DeleteFromOtherCSV(reservation);
                        break;
                    }
                }

                _accReservationRepository.Save(reservation);
                _changedResRequestRepositroy.Delete(oldReservation.Id);
                ChangedReservationRequest processedReservation = new ChangedReservationRequest(oldReservation.reservationId, oldReservation.AccommodationId,
                                                                                                oldReservation.AccommodationName, oldReservation.City, oldReservation.Country,
                                                                                                oldReservation.OldFirstDay, oldReservation.OldLastDay,
                                                                                                oldReservation.GuestNumber, oldReservation.UserId);
                processedReservation.NewFirstDay = newReservation.wantedDate.ReservationFirstDay;
                processedReservation.NewLastDay = newReservation.wantedDate.ReservationLastDay;
                processedReservation.status = ChangedReservationRequest.Status.REFUSED;
                processedReservation.ownerComment = ownerComment;
                _changedResRequestRepositroy.Save(processedReservation);

                _wantedNewDateRepository.Delete(newReservation.Id);
                MessageBox.Show("Zahtjev je odbijen.");
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
