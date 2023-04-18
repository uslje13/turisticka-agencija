using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.Views.TourGuide;

namespace SOSTeam.TravelAgency.WPF.ViewModels.TourGuide
{
    public class HomePageViewModel : ViewModel
    {
        private readonly TourService _tourService;
        private readonly LocationService _locationService;
        private readonly AppointmentService _appointmentService;
        private readonly ImageService _imageService;
        private readonly ReservationService _reservationService;
        private readonly VoucherService _voucherService;

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

        public RelayCommand CancelTourCommand { get; set; }
        public RelayCommand ShowTodayToursCommand { get; set; }

        //Ovo ima da leti kada se napravi burger meni
        public RelayCommand ShowFinishedTourReviewCommand { get; set; }
        public RelayCommand ShowStatsMenuCommand { get; set; }

        public User LoggedUser { get; set; }

        public HomePageViewModel(User loggedUser)
        {
            LoggedUser = loggedUser;
            TourCards = new ObservableCollection<TourCardViewModel>();
            _tourService = new TourService();
            _locationService = new LocationService();
            _appointmentService = new AppointmentService();
            _imageService = new ImageService();
            _reservationService = new ReservationService();
            _voucherService = new VoucherService();

            CancelTourCommand = new RelayCommand(CancelTourClick, CanExecuteMethod);
            ShowTodayToursCommand = new RelayCommand(TodayToursClick, CanExecuteMethod);
            ShowFinishedTourReviewCommand = new RelayCommand(ShowFinishedTourReviews, CanExecuteMethod);
            ShowStatsMenuCommand = new RelayCommand(ShowStatsMenu, CanExecuteMethod);

            SetExpiredAppointments();
            FillObservableCollection();
        }

        private bool CanExecuteMethod(object parameter)
        {
            return true;
        }

        private void SetExpiredAppointments()
        {
            foreach (var appointment in _appointmentService.GetAllByUserId(LoggedUser.Id))
            {
                if (appointment.IsExpired(_tourService.GetById(appointment.TourId).Duration))
                {
                    appointment.Finished = true;
                    _appointmentService.Update(appointment);
                }
            }
        }

        private void FillObservableCollection()
        {
            foreach (var appointment in _appointmentService.GetAllByUserId(LoggedUser.Id))
            {
                foreach (var tour in _tourService.GetAll())
                {
                    if (appointment.TourId == tour.Id)
                    {
                        var tourCard = new TourCardViewModel();

                        SetTourAndAppointmentFields(tourCard, appointment, tour);

                        SetLocationField(tour, tourCard);

                        SetImageField(tour, tourCard);

                        tourCard.SetCanCancel(appointment);

                        TourCards.Add(tourCard);
                    }
                }
            }
        }

        private void SetImageField(Tour tour, TourCardViewModel tourCard)
        {
            Image? image = _imageService.GetTourCover(tour.Id);
            tourCard.SetCoverImage(image);
        }

        private void SetLocationField(Tour tour, TourCardViewModel tourCard)
        {
            var location = _locationService.GetById(tour.LocationId);
            tourCard.SetLocation(location);
        }

        private void SetTourAndAppointmentFields(TourCardViewModel tourCard, Appointment appointment, Tour tour)
        {
            tourCard.AppointmentId = appointment.Id;
            tourCard.Date = appointment.Date;
            tourCard.Time = appointment.Time;
            tourCard.TourId = tour.Id;
            tourCard.Name = tour.Name;
            tourCard.SetAppointmentStatus(appointment);
        }

        private void CancelTourClick(object sender)
        {
            var selectedAppointment = sender as TourCardViewModel;
            _appointmentService.Delete(selectedAppointment.AppointmentId);
            TourCards.Remove(selectedAppointment);
            _voucherService.GiveVouchers(_reservationService.GetAllByAppointmentId(selectedAppointment.AppointmentId));
        }

        private void TodayToursClick(object sender)
        {
            TodayToursPage todayToursPage = new TodayToursPage(LoggedUser);
            System.Windows.Application.Current.Windows.OfType<MainWindow>().FirstOrDefault().ToursOverviewFrame.Content = todayToursPage;
        }


        //Ovo sve leti kada se sredi Burger Menu!!!
        private void ShowFinishedTourReviews(object sender)
        {
            FinishedTourReviewsPage finishedReviewsPage = new FinishedTourReviewsPage(LoggedUser);
            System.Windows.Application.Current.Windows.OfType<MainWindow>().FirstOrDefault().ToursOverviewFrame.Content = finishedReviewsPage;
        }

        private void ShowStatsMenu(object sender)
        {
            TourStatsPage tourStatsPage = new TourStatsPage(LoggedUser);
            System.Windows.Application.Current.Windows.OfType<MainWindow>().FirstOrDefault().ToursOverviewFrame.Content = tourStatsPage;
        }
    }
}
