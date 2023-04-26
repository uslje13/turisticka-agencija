using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Domain.Models;
using System.Collections.ObjectModel;


namespace SOSTeam.TravelAgency.WPF.ViewModels.TourGuide
{
    public class TourCardCreatorViewModel
    {
        #region Services

        private readonly TourService _tourService;
        private readonly LocationService _locationService;
        private readonly AppointmentService _appointmentService;
        private readonly ImageService _imageService;

        #endregion

        public TourCardCreatorViewModel()
        {
            _tourService = new TourService();
            _locationService = new LocationService();
            _appointmentService = new AppointmentService();
            _imageService = new ImageService();
        }

        public ObservableCollection<TourCardViewModel> CreateCards(User loggedUser, bool isToday)
        {
            var appointments = isToday ? _appointmentService.GetTodayAppointmentsByUserId(loggedUser.Id) : _appointmentService.GetAllByUserId(loggedUser.Id);

            var tourCards = new ObservableCollection<TourCardViewModel>();
            foreach (var appointment in appointments)
            {
                foreach (var tour in _tourService.GetAll())
                {
                    if (appointment.TourId == tour.Id)
                    {
                        var tourCard = new TourCardViewModel();

                        SetTourAndAppointment(tourCard, appointment, tour);

                        SetLocation(tour, tourCard);

                        SetCoverImage(tour, tourCard);

                        SetCanStartAppointment(tourCard, appointment, loggedUser);

                        tourCard.SetCanCancel(appointment);

                        tourCards.Add(tourCard);
                    }
                }
            }
            return tourCards;
        }

        private void SetCoverImage(Tour tour, TourCardViewModel tourCard)
        {
            var image = _imageService.GetTourCover(tour.Id);
            tourCard.SetCoverImage(image);
        }

        private void SetLocation(Tour tour, TourCardViewModel tourCard)
        {
            var location = _locationService.GetById(tour.LocationId);
            tourCard.SetLocation(location);
        }

        private void SetTourAndAppointment(TourCardViewModel tourCard, Appointment appointment, Tour tour)
        {
            tourCard.AppointmentId = appointment.Id;
            tourCard.Start = appointment.Start;
            tourCard.TourId = tour.Id;
            tourCard.Name = tour.Name;
            tourCard.SetAppointmentStatusAndBackground(appointment);
        }

        private void SetCanStartAppointment(TourCardViewModel tourCard, Appointment appointment, User loggedUser)
        {
            var activeAppointment = _appointmentService.GetActiveByUserId(loggedUser.Id);
            tourCard.SetCanStart(appointment, activeAppointment);
        }
    }
}
