using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Owner
{
    internal class GuestReviewPageViewModel
    {
        private GuestReview _guestReviewService;
        private MainWindowViewModel _mainwindowVM;

        public User LoggedInUser { get; private set; }
        public User Guest { get; private set; }
        public RelayCommand Cancel { get; private set; }
        public RelayCommand AddReview { get; private set; }
        public GuestReviewPageViewModel(User user, MainWindowViewModel mainWindowVM,User guest)
        {
            LoggedInUser = user;
            Guest = guest;
            _guestReviewService = new();
            _mainwindowVM = mainWindowVM;
            AddReview = new RelayCommand(Execute_AddReview, CanExecuteAddReview);
            Cancel = new RelayCommand(Execute_Cancel, CanExecuteCancel);
        }

        private bool CanExecuteAddReview(object obj)
        {
            return true;
        }
        private bool CanExecuteCancel(object obj)
        {
            return true;
        }

        private void Execute_AddReview(object obj)
        {
            
            


            _mainwindowVM.Execute_NavigationButtonCommand("Home");
            return;
        }

        private void Execute_Cancel(object obj)
        {
            _mainwindowVM.Execute_NavigationButtonCommand("Home");
            return;
        }
    }
}
