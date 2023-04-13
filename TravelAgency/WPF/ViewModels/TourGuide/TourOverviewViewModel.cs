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
                if (_toursForCards != value)
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
                if (_selectedTourCard != value)
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
            foreach (var appointment in _appointmentService.GetAllByUserId(loggedUser.Id))
            {
                foreach (var tour in _tourService.GetAll())
                {
                    if (appointment.TourId == tour.Id)
                    {
                        TourCardOverviewViewModel viewModel = new TourCardOverviewViewModel();

                        SetTourAndAppointmentFields(viewModel, appointment, tour);

                        SetLocationField(tour, viewModel);

                        SetImageField(tour, viewModel);

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
            var selectedAppointment = sender as TourCardOverviewViewModel;
            _appointmentService.Delete(selectedAppointment.AppointmentId);
            _toursForCards.Remove(selectedAppointment);
        }

        private void SetImageField(Tour tour, TourCardOverviewViewModel viewModel)
        {
            foreach (var image in _imageService.GetAllForTours())
            {
                if (image.Cover && image.EntityId == tour.Id)
                {
                    viewModel.ImageUrl = image.Url;
                    break;
                }
            }
        }

        private void SetLocationField(Tour tour, TourCardOverviewViewModel viewModel)
        {
            foreach (var location in _locationService.GetAll())
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
            if (!appointment.Started && !appointment.Finished)
            {
                viewModel.Status = "Not started";
            }
            else if (appointment.Started && !appointment.Finished)
            {
                viewModel.Status = "Active";
            }
            else if (appointment.Started && appointment.Finished)
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
