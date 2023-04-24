using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.Views.TourGuide;
using Xceed.Wpf.Toolkit.Core.Converters;
using Color = System.Drawing.Color;
using MessageBox = Xceed.Wpf.Toolkit.MessageBox;

namespace SOSTeam.TravelAgency.WPF.ViewModels.TourGuide
{
    public class HomePageViewModel : ViewModel
    {
        #region Services

        private readonly AppointmentService _appointmentService;
        private readonly ReservationService _reservationService;
        private readonly VoucherService _voucherService;

        #endregion

        #region Commands

        public RelayCommand CancelTourCommand { get; set; }
        public RelayCommand ShowTodayToursCommand { get; set; }

        #endregion


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

        //Ovo ima da leti kada se napravi burger meni
        public RelayCommand ShowFinishedTourReviewCommand { get; set; }
        public RelayCommand ShowStatsMenuCommand { get; set; }

        private readonly User _loggedUser;
        private readonly DispatcherTimer _timer;

        public HomePageViewModel(User loggedUser, DispatcherTimer timer)
        {
            _loggedUser = loggedUser;
            _timer = timer;
            _appointmentService = new AppointmentService();
            _reservationService = new ReservationService();
            _voucherService = new VoucherService();

            _appointmentService.SetExpiredAppointments(loggedUser.Id);

            var tourCardCreator = new TourCardCreatorViewModel();
            _tourCards = tourCardCreator.CreateCards(loggedUser, false);

            _timer.Tick += UpdateTourCards;

            CancelTourCommand = new RelayCommand(CancelTourClick, CanExecuteMethod);
            ShowTodayToursCommand = new RelayCommand(TodayToursClick, CanExecuteMethod);
            ShowFinishedTourReviewCommand = new RelayCommand(ShowFinishedTourReviews, CanExecuteMethod);
            ShowStatsMenuCommand = new RelayCommand(ShowStatsMenu, CanExecuteMethod);
        }

        private void UpdateTourCards(object sender, EventArgs e)
        {
            foreach (var tourCard in TourCards)
            {
                var appointment = _appointmentService.GetById(tourCard.AppointmentId);
                tourCard.SetAppointmentStatusAndBackground(appointment);
            }
        }

        private bool CanExecuteMethod(object parameter)
        {
            return true;
        }

        private void CancelTourClick(object sender)
        {
            var selectedAppointment = sender as TourCardViewModel;
            if(selectedAppointment == null) { return; }

            var messageBoxWindow = CreateMessageBox();

            var result = messageBoxWindow.ShowDialog();

            if (result == true)
            {
                _appointmentService.Delete(selectedAppointment.AppointmentId);
                TourCards.Remove(selectedAppointment);
                _voucherService.GiveVouchers(_reservationService.GetAllByAppointmentId(selectedAppointment.AppointmentId));
            }
        }

        private static MessageBoxWindow CreateMessageBox()
        {
            const string message = "Are you sure you want to cancel the tour?\nIf you cancel the tour," +
                                    " all users with a reservation for this tour will receive a voucher.";

            var messageBoxViewModel = new MessageBoxViewModel("Alert", "/Resources/Icons/warning.png", message);
            var messageBoxWindow = new MessageBoxWindow
            {
                DataContext = messageBoxViewModel
            };
            return messageBoxWindow;
        }

        private void TodayToursClick(object sender)
        {
            TodayToursPage todayToursPage = new TodayToursPage(_loggedUser, _timer);
            System.Windows.Application.Current.Windows.OfType<MainWindow>().FirstOrDefault().ToursOverviewFrame.Content = todayToursPage;
        }


        //Ovo sve leti kada se sredi Burger Menu!!!
        private void ShowFinishedTourReviews(object sender)
        {
            FinishedTourReviewsPage finishedReviewsPage = new FinishedTourReviewsPage(_loggedUser);
            System.Windows.Application.Current.Windows.OfType<MainWindow>().FirstOrDefault().ToursOverviewFrame.Content = finishedReviewsPage;
        }

        private void ShowStatsMenu(object sender)
        {
            StatsMenuPage statsMenuPage = new StatsMenuPage(_loggedUser);
            System.Windows.Application.Current.Windows.OfType<MainWindow>().FirstOrDefault().ToursOverviewFrame.Content = statsMenuPage;
        }
    }
}
