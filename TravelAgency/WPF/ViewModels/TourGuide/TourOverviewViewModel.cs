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
    public class TourOverviewViewModel : ViewModel
    {
        private readonly TourService _tourService;
        private readonly LocationService _locationService;
        private readonly AppointmentService _appointmentService;
        private readonly ImageService _imageService;
        private readonly ReservationService _reservationService;
        private readonly VoucherService _voucherService;

        private ObservableCollection<TourCardViewModel> _toursForCards;

        public ObservableCollection<TourCardViewModel> ToursForCards
        {
            get => _toursForCards;
            set
            {
                if (_toursForCards != value)
                {
                    _toursForCards = value;
                    OnPropertyChanged("ToursForCards");
                }
            }
        }


        public RelayCommand CancelTourCommand { get; set; }
        public RelayCommand ShowTodayToursCommand { get; set; }

        //Temp RelayCommand ---> Burger Menu
        public RelayCommand ShowFinishedTourReviewCommand { get; set; }
        public RelayCommand ShowStatsMenuCommand { get; set; }

        public User LoggedUser { get; set; }

        public TourOverviewViewModel(User loggedUser)
        {
            ToursForCards = new ObservableCollection<TourCardViewModel>();
            _tourService = new TourService();
            _locationService = new LocationService();
            _appointmentService = new AppointmentService();
            _imageService = new ImageService();
            _reservationService = new ReservationService();
            _voucherService = new VoucherService();
            LoggedUser = loggedUser;

            CancelTourCommand = new RelayCommand(CancelTourClick, CanExecuteMethod);
            ShowTodayToursCommand = new RelayCommand(TodayToursClick, CanExecuteMethod);
            ShowFinishedTourReviewCommand = new RelayCommand(ShowFinishedTourReviews, CanExecuteMethod);
            ShowStatsMenuCommand = new RelayCommand(ShowStatsMenu, CanExecuteMethod);

            SetExpiredAppointments();
            FillObservableCollection();
        }

        private void SetExpiredAppointments()
        {
            foreach (var appointment in _appointmentService.GetAllByUserId(LoggedUser.Id))
            {
                if (IsTimeExpired(appointment))
                {
                    appointment.Finished = true;
                    _appointmentService.Update(appointment);
                }
            }
        }

        private bool IsTimeExpired(Appointment appointment)
        {
            var tour = _tourService.FindTourById(appointment.TourId);
            var duration = TimeSpan.FromHours(tour.Duration);
            var start = new DateTime(appointment.Date.Year, appointment.Date.Month, appointment.Date.Day,
                                          appointment.Time.Hour, appointment.Time.Minute, appointment.Time.Second);
            var end = start + duration;

            return DateTime.Now > end;
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

                        CanCancelAppointment(appointment, tourCard);

                        ToursForCards.Add(tourCard);
                    }
                }
            }
        }

        private void SetImageField(Tour tour, TourCardViewModel tourCard)
        {
            foreach (var image in _imageService.GetAllForTours())
            {
                if (image.Cover && image.EntityId == tour.Id)
                {
                    tourCard.CoverImagePath = image.Path;
                    break;
                }
            }
        }

        private void SetLocationField(Tour tour, TourCardViewModel tourCard)
        {
            foreach (var location in _locationService.GetAll())
            {
                if (location.Id == tour.LocationId)
                {
                    tourCard.Location = location.City + ", " + location.Country;
                    tourCard.LocationId = location.Id;
                    break;
                }
            }
        }

        private void SetTourAndAppointmentFields(TourCardViewModel tourCard, Appointment appointment, Tour tour)
        {
            tourCard.AppointmentId = appointment.Id;
            tourCard.Date = appointment.Date;
            tourCard.TourId = tour.Id;
            tourCard.Name = tour.Name;
            SetAppointmentStatus(tourCard, appointment);
        }

        private void SetAppointmentStatus(TourCardViewModel tourCard, Appointment appointment)
        {
            if (!appointment.Started && !appointment.Finished)
            {
                tourCard.Status = "Not started";
            }
            else if (appointment.Started && !appointment.Finished)
            {
                tourCard.Status = "Active";
            }
            else if (appointment.Started && appointment.Finished)
            {
                tourCard.Status = "Finished";
            }
            else if (!appointment.Started && appointment.Finished)
            {
                tourCard.Status = "Expired";
            }
        }

        private void CanCancelAppointment(Appointment appointment, TourCardViewModel tourCard)
        {
            DateTime now = DateTime.Now;
            DateTime start = new DateTime(appointment.Date.Year, appointment.Date.Month, appointment.Date.Day,
                                          appointment.Time.Hour, appointment.Time.Minute, appointment.Time.Second);
            TimeSpan timeDifference = start - now;

            if (timeDifference.TotalHours < 48)
            {
                tourCard.CancelImage = "/Resources/Icons/cancel_light.png";
                tourCard.CanCancel = false;

            }
            else
            {
                tourCard.CancelImage = "/Resources/Icons/cancel.png";
                tourCard.CanCancel = true;
            }
        }
        private bool CanExecuteMethod(object parameter)
        {
            return true;
        }

        private void CancelTourClick(object sender)
        {
            var selectedAppointment = sender as TourCardViewModel;
            _appointmentService.Delete(selectedAppointment.AppointmentId);
            ToursForCards.Remove(selectedAppointment);
            GiveVouchers(selectedAppointment);
        }

        private void GiveVouchers(TourCardViewModel canceledAppointment)
        {
            var vouchers = new List<Voucher>();
            foreach (var reservation in _reservationService.GetAllByAppointmentId(canceledAppointment.AppointmentId))
            {
                var voucher = CreateVoucher(reservation);
                vouchers.Add(voucher);
            }
            if (vouchers.Count > 0)
            {
                _voucherService.SaveAll(vouchers);
            }
            
        }

        private Voucher CreateVoucher(Reservation reservation)
        {
            var voucher = new Voucher
            {
                UserId = reservation.UserId,
                ExpiryDate = DateOnly.FromDateTime(DateTime.Now.AddMonths(6)),
                Used = false
            };
            return voucher;
        }

        
        private void TodayToursClick(object sender)
        {
            TodayToursPage todayToursPage = new TodayToursPage(LoggedUser);
            System.Windows.Application.Current.Windows.OfType<MainWindow>().FirstOrDefault().ToursOverviewFrame.Content = todayToursPage;
        }

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
