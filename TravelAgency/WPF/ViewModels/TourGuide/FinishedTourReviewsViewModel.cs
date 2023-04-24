using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.Views.TourGuide;

namespace SOSTeam.TravelAgency.WPF.ViewModels.TourGuide
{
    public class FinishedTourReviewsViewModel : ViewModel
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

        public RelayCommand ShowGuestReviewsCommand { get; set; }

        public User LoggedUser { get; set; }

        private readonly TourService _tourService;
        private readonly LocationService _locationService;
        private readonly AppointmentService _appointmentService;
        private readonly ImageService _imageService;

        public FinishedTourReviewsViewModel(User loggedUser)
        {
            LoggedUser = loggedUser;
            TourCards = new ObservableCollection<TourCardViewModel>();
            _tourService = new TourService();
            _locationService = new LocationService();
            _appointmentService = new AppointmentService();
            _imageService = new ImageService();

            ShowGuestReviewsCommand = new RelayCommand(ShowGuestReviews, CanExecuteMethod);

            FillObservableCollection();

        }

        private bool CanExecuteMethod(object parameter)
        {
            return true;
        }

        private void FillObservableCollection()
        {
            foreach (var appointment in _appointmentService.GetAllFinishedByUserId(LoggedUser.Id))
            {
                foreach (var tour in _tourService.GetAll())
                {
                    if (appointment.TourId == tour.Id)
                    {
                        var tourCard = new TourCardViewModel();

                        SetTourAndAppointmentFields(tourCard, appointment, tour);

                        SetLocationField(tour, tourCard);

                        SetImageField(tour, tourCard);

                        TourCards.Add(tourCard);
                    }
                }
            }
        }

        private void SetImageField(Tour tour, TourCardViewModel tourCard)
        {
            var coverImage = _imageService.GetTourCover(tour.Id);
            tourCard.SetCoverImage(coverImage);
        }

        private void SetLocationField(Tour tour, TourCardViewModel tourCard)
        {
            var location = _locationService.GetById(tour.LocationId);
            if(location == null) { return; }
            tourCard.SetLocation(location);
        }

        private void SetTourAndAppointmentFields(TourCardViewModel tourCard, Appointment appointment, Tour tour)
        {
            tourCard.AppointmentId = appointment.Id;
            tourCard.Start = appointment.Start;
            tourCard.TourId = tour.Id;
            tourCard.Name = tour.Name;
            tourCard.SetAppointmentStatusAndBackground(appointment);
        }

        private void ShowGuestReviews(object sender)
        {
            var selectedTourCard = sender as TourCardViewModel;
            GuestReviewOverviewPage guestReviewOverviewPage = new GuestReviewOverviewPage(selectedTourCard);
            System.Windows.Application.Current.Windows.OfType<MainWindow>().FirstOrDefault().ToursOverviewFrame.Content = guestReviewOverviewPage;
        }
    }
}
