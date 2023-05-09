using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.Views.Guest2;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest2
{
    public class OrdinaryToursPageViewModel
    {
        public static User LoggedInUser { get; set; }
        public static ObservableCollection<RequestViewModel> TourRequests { get; set; }

        private readonly TourRequestService _tourRequestService;

        private RelayCommand _statisticsCommand;

        public RelayCommand StatisticsCommand
        {
            get { return _statisticsCommand; }
            set
            {
                _statisticsCommand = value;
            }
        }

        private RelayCommand _createCommand;

        public RelayCommand CreateCommand
        {
            get { return _createCommand; }
            set
            {
                _createCommand = value;
            }
        }

        public OrdinaryToursPageViewModel(User loggedInUser)
        {
            LoggedInUser= loggedInUser;
            _tourRequestService = new TourRequestService();
            StatisticsCommand = new RelayCommand(Execute_StatisticsCommand, CanExecuteMethod);
            CreateCommand = new RelayCommand(Execute_CreateCommand,CanExecuteMethod);
            TourRequests = new ObservableCollection<RequestViewModel>();
            FillTourRequests();
        }

        private void FillTourRequests()
        {
            foreach(var request in _tourRequestService.GetAll())
            {
                TourRequests.Add(new RequestViewModel(LoggedInUser.Id, request.City, request.Country, request.Description, request.Language, request.MaxNumOfGuests, request.MaintenanceStartDate, request.MaintenanceEndDate, request.Status));
            }
        }

        private void Execute_StatisticsCommand(object obj)
        {
            throw new NotImplementedException();
        }

        private void Execute_CreateCommand(object obj)
        {
            CreateTourRequestWindow window = new CreateTourRequestWindow();
            window.ShowDialog();
        }

        private bool CanExecuteMethod(object parameter)
        {
            return true;
        }
    }
}
