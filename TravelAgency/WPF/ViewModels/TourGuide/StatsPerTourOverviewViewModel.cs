using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.Views.TourGuide;

namespace SOSTeam.TravelAgency.WPF.ViewModels.TourGuide
{
    public class StatsPerTourOverviewViewModel : ViewModel
    {
        private ObservableCollection<TourCardViewModel> _tourCards;

        public ObservableCollection<TourCardViewModel> TourCards
        {
            get => _tourCards;
            set
            {
                if (_tourCards != value)
                {
                    _tourCards = value;
                    OnPropertyChanged("TourCards");
                }
            }
        }

        private string _selectedYear;

        public string SelectedYear
        {
            get => _selectedYear;
            set
            {
                if (_selectedYear != value)
                {
                    _selectedYear = value;
                    OnPropertyChanged("SelectedYear");
                }
            }
        }

        private ObservableCollection<string> _availableYears;

        public ObservableCollection<string> AvailableYears
        {
            get => _availableYears;
            set
            {
                if (_availableYears != value)
                {
                    _availableYears = value;
                    OnPropertyChanged("AvailableYears");
                }
            }

        }

        public User LoggedUser { get; set; }

        private readonly TourService _tourService;
        private readonly LocationService _locationService;
        private readonly AppointmentService _appointmentService;
        public RelayCommand YearSelectionChangedCommand { get; set; }
        public RelayCommand ShowTourStatsCommand { get; set; }

        public StatsPerTourOverviewViewModel(User loggedUser)
        {
            LoggedUser = loggedUser;
            _tourService = new TourService();
            _locationService = new LocationService();
            _appointmentService = new AppointmentService();

            TourCards = new ObservableCollection<TourCardViewModel>();
            AvailableYears = new ObservableCollection<string>();

            FindAvailableYears();

            FillObservableCollection();
            YearSelectionChangedCommand = new RelayCommand(ExecuteYearSelectionChanged, CanExecuteMethod);
            ShowTourStatsCommand = new RelayCommand(ShowTourStats, CanExecuteMethod);
        }

        private bool CanExecuteMethod(object parameter)
        {
            return true;
        }

        public void ExecuteYearSelectionChanged(object parameter)
        {
            YearSelectionChanged(parameter, null);
        }

        public void YearSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FillObservableCollection();
        }

        private void FillObservableCollection()
        {
            TourCards.Clear();
            foreach (var appointment in _appointmentService.GetAllFinishedByUserId(LoggedUser.Id))
            {
                foreach (var tour in _tourService.GetAll())
                {
                    if (appointment.TourId == tour.Id && appointment.Start.Year.ToString() == SelectedYear)
                    {
                        var tourCard = new TourCardViewModel();
                        SetTourAndAppointmentFields(tourCard, appointment, tour);
                        tourCard.SetLocation(_locationService.GetById(tour.LocationId));

                        TourCards.Add(tourCard);
                    }
                }
            }
        }

        private void SetTourAndAppointmentFields(TourCardViewModel tourCard, Appointment appointment, Tour tour)
        {
            tourCard.AppointmentId = appointment.Id;
            tourCard.TourId = tour.Id;
            tourCard.LocationId = tour.LocationId;
            tourCard.Start = appointment.Start;
            tourCard.Name = tour.Name;
            tourCard.SetAppointmentStatusAndBackground(appointment);
        }

        private void FindAvailableYears()
        {
            foreach (var appointment in _appointmentService.GetAllByUserId(LoggedUser.Id))
            {
                AvailableYears.Add(appointment.Start.Year.ToString());
            }
            AvailableYears = new ObservableCollection<string>(AvailableYears.Distinct());

            if (AvailableYears.Count > 0)
            {
                SelectedYear = AvailableYears[0];
            }
        }

        private void ShowTourStats(object sender)
        {
            var selectedTourCard = sender as TourCardViewModel;
            StatByTourPage statByTourPage = new StatByTourPage(selectedTourCard);
            System.Windows.Application.Current.Windows.OfType<MainWindow>().FirstOrDefault().ToursOverviewFrame.Content = statByTourPage;
        }

    }
}
