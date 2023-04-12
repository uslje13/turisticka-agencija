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
                if (!_toursForCards.Equals(value))
                {
                    _toursForCards = value;
                    OnPropertyChanged("ToursForCards");
                }
            }
        }

        private TourCardOverviewViewModel _selectedTourCard;

        public TourCardOverviewViewModel SelectedTourCard
        {
            get => _selectedTourCard;
            set
            {
                if (!_selectedTourCard.Equals(value))
                {
                    _selectedTourCard = value;
                    OnPropertyChanged("SelectedTourCard");
                }
            }
        }

        public RelayCommand CancelTourCommand { get; set; }

        public TourOverviewViewModel(User loggedUser)
        {
            InitializeServices();
            FillObservableCollection(loggedUser);
        }

        private void InitializeServices()
        {
            _toursForCards = new ObservableCollection<TourCardOverviewViewModel>();
            _tourService = new TourService();
            _locationService = new LocationService();
            _appointmentService = new AppointmentService();
            _imageService = new ImageService();

            CancelTourCommand = new RelayCommand(CancelTourClick, CanExecuteMethod);
        }

        public void FillObservableCollection(User loggedUser)
        {
            var tours = CreateEntitiesList(loggedUser, out var locations, out var appointments, out var images);

            foreach (var appointment in appointments)
            {
                foreach (var tour in tours)
                {
                    if (appointment.TourId == tour.Id)
                    {
                        TourCardOverviewViewModel viewModel = new TourCardOverviewViewModel();

                        SetTourAndAppointmentFields(viewModel, appointment, tour);

                        SetLocationField(locations, tour, viewModel);

                        SetImageField(images, tour, viewModel);

                        CanCancelAppointment(appointment, viewModel);

                        ToursForCards.Add(viewModel);
                    }
                }
            }
        }
        private bool CanExecuteMethod(object parameter)
        {
            return true;
        }

        private void CancelTourClick(object sender)
        {
            var selectedData = sender as TourCardOverviewViewModel;
            _appointmentService.Delete(selectedData.AppointmentId);
            _toursForCards.Remove(selectedData);
        }

        private List<Tour> CreateEntitiesList(User loggedUser, out List<Location> locations, out List<Appointment> appointments, out List<Image> images)
        {
            List<Tour> tours = _tourService.GetAll();
            locations = _locationService.GetAll();
            appointments = _appointmentService.GetAllByUserId(loggedUser.Id);
            images = _imageService.GetAllForTours();
            return tours;
        }

        private static void SetImageField(List<Image> images, Tour tour, TourCardOverviewViewModel viewModel)
        {
            foreach (var image in images)
            {
                if (image.Cover && image.EntityId == tour.Id)
                {
                    viewModel.ImageUrl = image.Url;
                    break;
                }
            }
        }

        private static void SetLocationField(List<Location> locations, Tour tour, TourCardOverviewViewModel viewModel)
        {
            foreach (var location in locations)
            {
                if (location.Id == tour.LocationId)
                {
                    viewModel.Location = location.City + ", " + location.Country;
                    viewModel.LocationId = location.Id;
                    break;
                }
            }
        }

        private static void SetTourAndAppointmentFields(TourCardOverviewViewModel viewModel, Appointment appointment, Tour tour)
        {
            viewModel.AppointmentId = appointment.Id;
            viewModel.Date = appointment.Date;
            SetAppointmentStatus(viewModel, appointment);
            viewModel.TourId = tour.Id;
        }

        private static void SetAppointmentStatus(TourCardOverviewViewModel viewModel, Appointment appointment)
        {
            if (appointment.Started == false && appointment.Finished == false)
            {
                viewModel.Status = "Not started";
            }
            else if (appointment.Started == true && appointment.Finished == false)
            {
                viewModel.Status = "Active";
            }
            else if (appointment.Started == true && appointment.Finished == true)
            {
                viewModel.Status = "Finished";
            }
        }

        public void CanCancelAppointment(Appointment appointment, TourCardOverviewViewModel viewModel)
        {
            DateTime now = DateTime.Now;
            DateTime start = new DateTime(appointment.Date.Year, appointment.Date.Month, appointment.Date.Day,
                                          appointment.Time.Hour, appointment.Time.Minute, appointment.Time.Second);
            TimeSpan timeDifference = start - now;

            if (timeDifference.TotalHours < 48)
            {
                viewModel.CancelImage = "/Resources/Icons/cancel_light.png";
                viewModel.CanCancel = false;

            }
            else
            {
                viewModel.CancelImage = "/Resources/Icons/cancel.png";
                viewModel.CanCancel = true;
            }
        }
    }
}
