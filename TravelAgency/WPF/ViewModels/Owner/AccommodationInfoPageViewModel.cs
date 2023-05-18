﻿using LiveCharts;
using LiveCharts.Wpf;
using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Owner
{
    public class AccommodationInfoPageViewModel : ViewModel
    {
        public User LoggedInUser { get; private set; }
        public Accommodation Accommodation { get; private set; }
        public string Location { get; private set; }
        public string StatsLabel { get; private set; }
        public Image Image { get; private set; }
        private ObservableCollection<ReservationViewModel> _reservations;
        public ObservableCollection<ReservationViewModel> Reservations
        {
            get { return _reservations; }
            set
            {
                _reservations = value;
                OnPropertyChanged("Reservations");
            }
        }

        private SeriesCollection _reservationSeries;
        public SeriesCollection ReservationSeries
        {
            get { return _reservationSeries; }
            set
            {
                _reservationSeries = value;
                OnPropertyChanged("ReservationSeries");
            }
        }
        public string[] TimeLabels { get; set; }
        public RelayCommand NavigatePhotos { get; private set; }
        public RelayCommand NavigateYears { get; private set; }
        public RelayCommand ToggleChart { get; private set; }

        private int _slidingGridHeight;
        public int SlidingGridHeight
        {
            get { return _slidingGridHeight; }
            set
            {
                _slidingGridHeight = value;
                OnPropertyChanged("SlidingGridHeight");
            }
        }

        private int _slidingGridYear;
        public int SlidingGridYear
        {
            get { return _slidingGridYear; }
            set
            {
                _slidingGridYear = value;
                OnPropertyChanged("SlidingGridYear");
            }
        }

        private List<Image> _images;
        private LocationService _locationService;
        private ImageService _imageService;
        private AccommodationStatsService _accommodationStatsService;
        private AccommodationReservationService _accommodationReservationService;

        private int _imageIndex;





        public AccommodationInfoPageViewModel(User user, Accommodation accommodation)
        {
            LoggedInUser = user;
            Accommodation = accommodation;
            _locationService = new();
            _accommodationStatsService = new(user.Id);
            _imageService = new();
            _accommodationReservationService = new();

            GetReservations(accommodation);
            GetImages(accommodation);
            SetupYearReservationChart();
            SlidingGridHeight = 0;
            StatsLabel = "Statistika po godinama";
            SlidingGridYear = DateTime.Now.Year;
            NavigatePhotos = new RelayCommand(Execute_NavigatePhotos, CanExecuteNavigatePhotos);
            NavigateYears = new RelayCommand(Execute_NavigateYears, CanExecuteToggleChart);
            ToggleChart = new RelayCommand(Execute_ToggleChart, CanExecuteToggleChart);
            Location = _locationService.GetFullName(_locationService.GetById(Accommodation.LocationId));
        }

        private void Execute_NavigateYears(object obj)
        {
            string direction = obj.ToString();
            if (direction.Equals("Left")) SlidingGridYear--;
            else SlidingGridYear++;
            SetupMonthReservationChart();
        }

        private void Execute_ToggleChart(object obj)
        {
            if (TimeLabels.Length == 12)
            {
                SetupYearReservationChart();
                StatsLabel = "Statistika po godinama";
                SlidingGridHeight = 0;
            }
            else
            {
                SetupMonthReservationChart();
                StatsLabel = "Statistika po mesecima";
                SlidingGridHeight = 100;
            }
            OnPropertyChanged("StatsLabel");




        }

        private bool CanExecuteToggleChart(object obj)
        {
            return true;
        }

        private void GetReservations(Accommodation accommodation)
        {
            var reservations = _accommodationReservationService.GetAll().Where(t => t.AccommodationId == accommodation.Id && t.LastDay > DateTime.Now && t.FirstDay < DateTime.Now.AddMonths(3));
            Reservations = new();
            foreach (var reservation in reservations)
            {
                Reservations.Add(new ReservationViewModel(reservation.FirstDay.ToString("dd-MM-yyyy"), reservation.LastDay.ToString("dd-MM-yyyy")));
            }
        }

        private void GetImages(Accommodation accommodation)
        {
            _images = _imageService.GetAllForAccommodations().Where(t => t.EntityId == accommodation.Id).ToList();
            if (_images == null)
            {
                Image = new Image();
                Image.Path = "/Resources/Images/UnknownPhoto.png";
            }
            else
            {
                _images = _images.OrderByDescending(t => t.Cover).ToList();
                Image = _images.First();
                _imageIndex = 0;
            }
        }

        private bool CanExecuteNavigatePhotos(object obj)
        {
            return _images.Count > 1;
        }

        private void Execute_NavigatePhotos(object obj)
        {
            string direction = obj.ToString();
            if (direction.Equals("Left"))
            {
                _imageIndex = _imageIndex == 0 ? _images.Count - 1 : _imageIndex - 1;
                Image = _images[_imageIndex];
            }
            else
            {
                _imageIndex = _imageIndex == _images.Count - 1 ? 0 : _imageIndex + 1;
                Image = _images[_imageIndex];
            }
            OnPropertyChanged("Image");
        }

        private void SetupYearReservationChart()
        {
            var regular = _accommodationStatsService.GetOccupationInYears(DateTime.Today, 5, Accommodation.Id);
            regular.Reverse();

            var moved = _accommodationStatsService.GetReservationMovesInYears(DateTime.Today, 5, Accommodation.Id);
            moved.Reverse();

            var changed = _accommodationStatsService.GetCancelationInYears(DateTime.Today, 5, Accommodation.Id);
            changed.Reverse();

            var renovation = _accommodationStatsService.GetRenovationRecommendationInYears(DateTime.Today, 5, Accommodation.Id);
            renovation.Reverse();
            // Set up data series
            var regularSeries = new StackedColumnSeries { Title = "Rezervacije", Values = new ChartValues<int>(regular) };
            var movedSeries = new StackedColumnSeries { Title = "Pomeranja", Values = new ChartValues<int>(moved) };
            var changedSeries = new StackedColumnSeries { Title = "Otkazivanja", Values = new ChartValues<int>(changed) };
            var renovationSeries = new StackedColumnSeries { Title = "Predloga za renoviranje", Values = new ChartValues<int>(renovation) };


            // Set up series collection
            ReservationSeries = new SeriesCollection { regularSeries, movedSeries, changedSeries, renovationSeries };

            var temp = new List<string>();
            for (int i = 0; i < moved.Count; i++)
            {
                temp.Add(DateTime.Today.AddYears(-i).ToString("yyyy"));
            }
            temp.Reverse();
            TimeLabels = temp.ToArray();
            OnPropertyChanged("TimeLabels");

        }

        private void SetupMonthReservationChart()
        {
            var year = new DateTime(SlidingGridYear, 1, 1);

            var regular = _accommodationStatsService.GetOccupationInMonths(year, Accommodation.Id);

            var moved = _accommodationStatsService.GetReservationMovesInMonths(year, Accommodation.Id);

            var changed = _accommodationStatsService.GetCancelationInMonths(year, Accommodation.Id);

            var renovation = _accommodationStatsService.GetReservationMovesInMonths(year, Accommodation.Id);

            var regularSeries = new StackedColumnSeries { Title = "Rezervacije", Values = new ChartValues<int>(regular) };
            var movedSeries = new StackedColumnSeries { Title = "Pomeranja", Values = new ChartValues<int>(moved) };
            var changedSeries = new StackedColumnSeries { Title = "Otkazivanja", Values = new ChartValues<int>(changed) };
            var renovationSeries = new StackedColumnSeries { Title = "Predloga za renoviranje", Values = new ChartValues<int>(renovation) };


            ReservationSeries = new SeriesCollection { regularSeries, movedSeries, changedSeries, renovationSeries };

            var temp = new List<string>();
            for (int i = 0; i < 12; i++)
            {
                temp.Add(year.AddMonths(1 + i).ToString("MMMM"));
            }

            TimeLabels = temp.ToArray();
            OnPropertyChanged("TimeLabels");
        }
    }

    public class ReservationViewModel
    {
        public string FirstDay { get; set; }
        public string LastDay { get; set; }
        public ReservationViewModel(string firstDay, string lastDay)
        {
            FirstDay = firstDay;
            LastDay = lastDay;
        }
    }
}
