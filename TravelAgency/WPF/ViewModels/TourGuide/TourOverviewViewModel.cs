using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Domain.Models;

namespace SOSTeam.TravelAgency.WPF.ViewModels.TourGuide
{
    public class TourOverviewViewModel : ViewModel
    {
        public TourService _tourService;
        public LocationService _locationService;
        public AppointmentService _appointmentService;
        public ImageService _imageService;

        private ObservableCollection<TourCardOverviewViewModel> _toursForCards;

        public ObservableCollection<TourCardOverviewViewModel> ToursForCards
        {
            get => _toursForCards;
            set
            {
                if (!value.Equals(_toursForCards))
                {
                    _toursForCards = value;
                    OnPropertyChanged("ToursForCards");
                }
            }
        }

        public TourOverviewViewModel(User loggedUser)
        {
            _toursForCards = new ObservableCollection<TourCardOverviewViewModel>();
            _tourService = new TourService();
            _locationService = new LocationService();
            _appointmentService = new AppointmentService();
            _imageService = new ImageService();
            
            FillObservableCollection(loggedUser);
        }

        public void FillObservableCollection(User loggedUser)
        {
            //TourCardOverviewViewModel viewModel = new TourCardOverviewViewModel();
            List<Tour> tours = _tourService.GetAll();
            List<Location> locations = _locationService.GetAll();
            List<Appointment> appointments = _appointmentService.GetAllByUserId(loggedUser.Id);
            List<Image> images = _imageService.GetAllForTours();

            //Prvo popunim appointmente
            foreach (var appointment in appointments)
            {
                TourCardOverviewViewModel viewModel = new TourCardOverviewViewModel();
                //Onda u appointmente dodam ime ture
                foreach (var tour in tours)
                {
                    if (appointment.TourId == tour.Id)
                    {
                        viewModel.AppointmentId = appointment.Id;
                        viewModel.Date = appointment.Date;
                        viewModel.Started = appointment.Started;
                        viewModel.Finished = appointment.Finished;
                        viewModel.Name = tour.Name;
                        viewModel.TourId = tour.Id;
                        //A onda dodam lokaciju
                        foreach (var location in locations)
                        {
                            if (location.Id == tour.LocationId)
                            {
                                viewModel.Location = location.City + ", " + location.Country;
                                viewModel.LocationId = location.Id;
                            }
                        }

                        foreach (var image in images)
                        {
                            if (image.EntityId == tour.Id)
                            {
                                viewModel.ImageUrl = image.Url;
                            }
                        }
                        ToursForCards.Add(viewModel);
                    }
                }
            }
        }

    }
}
