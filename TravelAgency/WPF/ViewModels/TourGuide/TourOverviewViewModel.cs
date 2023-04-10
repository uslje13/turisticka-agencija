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

        public TourOverviewViewModel()
        {
            _toursForCards = new ObservableCollection<TourCardOverviewViewModel>();

            _tourService = new TourService();
            _locationService = new LocationService();
            _appointmentService = new AppointmentService();
            _imageService = new ImageService();
            
            FillObservableCollection();
        }



        public void FillObservableCollection()
        {
            TourCardOverviewViewModel viewModel = new TourCardOverviewViewModel();
            List<Tour> tours = _tourService.GetAll();
            List<Location> locations = _locationService.GetAll();
            List<Appointment> appointments = _appointmentService.GetAll();
            List<Image> images = _imageService.GetAll();

            //Prvo popunim appointmente
            foreach (var appointment in appointments)
            {
                viewModel.AppointmentId = appointment.Id;
                viewModel.Date = appointment.Date;
                viewModel.Started = appointment.Started;
                viewModel.Finished = appointment.Finished;
                viewModel.AppointmentId = appointment.Id;

                //Onda u appointmente dodam ime ture
                foreach (var tour in tours)
                {
                    if (appointment.TourId == tour.Id)
                    {
                        viewModel.Name = tour.Name;
                        viewModel.TourId = tour.Id;
                        //A onda dodam lokaciju
                        foreach (var location in locations)
                        {
                            if (location.Id == tour.LocationId)
                            {
                                viewModel.Location = location.City + " ," + location.Country;
                                viewModel.LocationId = location.Id;
                            }
                        }
                    }
                }
                ToursForCards.Add(viewModel);
            }
        }

    }
}
