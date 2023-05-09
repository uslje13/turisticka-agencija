﻿using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.Views.Guest1;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest1
{
    public class ReportFiltersViewModel
    {
        public User LoggedInUser { get; set; }
        public string SelectedOption { get; set; }
        public DateTime FirstDate { get; set; }
        public DateTime LastDate { get; set; }
        public ObservableCollection<string> _comboBoxOptions { get; set; }
        public List<CancelAndMarkResViewModel> _presentedReservations { get; set; }
        public Frame ThisFrame { get; set; }
        public RelayCommand ProcessReportCommand { get; set; }

        public ReportFiltersViewModel(User user, Frame frame)
        {
            LoggedInUser = user;
            ThisFrame = frame;
            
            _comboBoxOptions = new ObservableCollection<string> { "Zakazane", "Otkazane" };
            SelectedOption = _comboBoxOptions[1];
            FirstDate = DateTime.Now;
            LastDate = DateTime.Now;
            
            _presentedReservations = new List<CancelAndMarkResViewModel>();

            ProcessReportCommand = new RelayCommand(Execute_ProcessReport);
        }
        
        private void Execute_ProcessReport(object sender)
        {
            if (LastDate.DayOfYear < FirstDate.DayOfYear)
            {
                MessageBox.Show("Datumi opsega nisu izabrani validno!");
            }
            else
            {
                int k = FindSelectedType();
                if (k == 0) LoadFuturedReservations();
                else LoadCanceledReservations();
                var navigationService = ThisFrame.NavigationService;
                navigationService.Navigate(new ReportResultsPage(_presentedReservations, ThisFrame, k, FirstDate, LastDate));
            }
        }
        
        private int FindSelectedType()
        {
            if (SelectedOption.Equals(_comboBoxOptions[0])) return 0;
            else return 1;
        }

        private void LoadFuturedReservations()
        {
            AccommodationService accommodationService = new AccommodationService();
            AccommodationReservationService reservationService = new AccommodationReservationService();
            ObservableCollection<LocAccommodationViewModel> _locAccommodationViewModels = accommodationService.CreateAllDTOForms();
            List<AccommodationReservation> _accommodationReservations = reservationService.GetAll();

            _presentedReservations.Clear();
            foreach (var lavm in _locAccommodationViewModels)
            {
                foreach (var res in _accommodationReservations)
                {
                    if (IsValidForList(0, lavm, res))
                    { 
                        CancelAndMarkResViewModel model = new CancelAndMarkResViewModel(lavm.AccommodationName, lavm.LocationCity, lavm.LocationCountry, res.FirstDay, res.LastDay, res.Id, lavm.AccommodationId, "", res.ReservationDuration, lavm.AccommodationType);
                        _presentedReservations.Add(model);
                    }
                }
            }
        }

        private bool IsValidForList(int type, LocAccommodationViewModel lavm, AccommodationReservation res)
        {
            if(type == 0)
            {
                return lavm.AccommodationId == res.AccommodationId && res.UserId == LoggedInUser.Id && DateTime.Today.DayOfYear < res.FirstDay.DayOfYear && res.FirstDay.DayOfYear >= FirstDate.DayOfYear && res.LastDay.DayOfYear <= LastDate.DayOfYear;
            }
            else
            {
                return lavm.AccommodationId == res.AccommodationId && res.UserId == LoggedInUser.Id && res.FirstDay.DayOfYear >= FirstDate.DayOfYear && res.LastDay.DayOfYear <= LastDate.DayOfYear;
            }
        }

        private void LoadCanceledReservations()
        {
            AccommodationService accommodationService = new AccommodationService();
            AccommodationReservationService reservationService = new AccommodationReservationService();
            ObservableCollection<LocAccommodationViewModel> _locAccommodationViewModels = accommodationService.CreateAllDTOForms();
            List<AccommodationReservation> _canceledReservations = reservationService.LoadCanceledReservations();

            _presentedReservations.Clear();
            foreach (var lavm in _locAccommodationViewModels)
            {
                foreach (var res in _canceledReservations)
                {
                    if (IsValidForList(1, lavm, res))
                    {
                        CancelAndMarkResViewModel model = new CancelAndMarkResViewModel(lavm.AccommodationName, lavm.LocationCity, lavm.LocationCountry, res.FirstDay, res.LastDay, res.Id, lavm.AccommodationId, "", res.ReservationDuration, lavm.AccommodationType);
                        _presentedReservations.Add(model);
                    }
                }
            }
        }
    }
}