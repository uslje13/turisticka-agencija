using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.Views.Guest2;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest2
{
    public class CreateTourRequestViewModel : ViewModel
    {
        public OrdinaryToursPageViewModel OrdinaryToursPageViewModel { get; set; }

        private CreateTourRequestWindow _window;
        public User LoggedInUser { get; set; }
        public ObservableCollection<RequestViewModel> TourRequests { get; set; }
        public RequestViewModel TourRequest { get; set; }

        private RelayCommand _reviewCommand;
        public RelayCommand ReviewCommand
        {
            get { return _reviewCommand; }
            set
            {
                _reviewCommand = value;
            }
        }

        private RelayCommand _closeCommand;

        public RelayCommand CloseCommand
        {
            get { return _closeCommand; }
            set
            {
                _closeCommand = value;
            }
        }

        public CreateTourRequestViewModel(User loggedInUser,CreateTourRequestWindow window,OrdinaryToursPageViewModel ordinaryToursPageViewModel) 
        {
            OrdinaryToursPageViewModel = ordinaryToursPageViewModel;
            _window= window;
            LoggedInUser= loggedInUser;
            TourRequest= new RequestViewModel();
            TourRequests = new ObservableCollection<RequestViewModel>();
            ReviewCommand = new RelayCommand(Execute_ReviewCommand, CanExecuteMethod);
            CloseCommand = new RelayCommand(Execute_CloseCommand,CanExecuteMethod);
        }

        private void Execute_CloseCommand(object obj)
        {
            var currentApp = System.Windows.Application.Current;

            foreach (Window window in currentApp.Windows)
            {
                if (window is CreateTourRequestWindow)
                {
                    window.Close();
                }
            }
        }

        private void Execute_ReviewCommand(object obj)
        {
            TourRequest.StartEndDateRange = TourRequest.MaintenanceStartDate.ToString() + " - " + TourRequest.MaintenanceEndDate.ToString();
            TourRequest.LocationFullName = TourRequest.City + ", " + TourRequest.Country;
            if (IsDataCorrect())
            {
                TourRequests.Add(TourRequest);
                var navigationService = _window.TourReviewFrame.NavigationService;
                navigationService.Navigate(new TourRequestReviewPage(LoggedInUser, TourRequests, OrdinaryToursPageViewModel));
            }          
        }

        private bool IsDataCorrect()
        {
            bool isCorrect = true;
            if (string.IsNullOrEmpty(TourRequest.City) || string.IsNullOrEmpty(TourRequest.Country) || string.IsNullOrEmpty(TourRequest.Language) || string.IsNullOrEmpty(TourRequest.Description) || string.IsNullOrEmpty(TourRequest.MaintenanceStartDate) || string.IsNullOrEmpty(TourRequest.MaintenanceEndDate))
            {
                MessageBox.Show("Nisu uneti svi podaci", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
                isCorrect = false;
            }
            else if(TourRequest.MaxNumOfGuests <= 0)
            {
                MessageBox.Show("Broj turista ne moze biti manji ili jednak nuli", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
                isCorrect = false;
            }
            else if(DateTime.Parse(TourRequest.MaintenanceStartDate) > DateTime.Parse(TourRequest.MaintenanceEndDate))
            {
                MessageBox.Show("Datum zavrsetka je pre datuma pocetka", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
                isCorrect = false;
            }
            return isCorrect;   
        }

        private bool CanExecuteMethod(object parameter)
        {
            return true;
        }
    }
}
