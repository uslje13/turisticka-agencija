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
            tourCard.Time = appointment.Time;
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

        private void ShowGuestReviews(object sender)
        {
            var selectedTourCard = sender as TourCardViewModel;
            GuestReviewOverviewPage guestReviewOverviewPage = new GuestReviewOverviewPage(selectedTourCard);
            System.Windows.Application.Current.Windows.OfType<MainWindow>().FirstOrDefault().ToursOverviewFrame.Content = guestReviewOverviewPage;
        }


    }
}
