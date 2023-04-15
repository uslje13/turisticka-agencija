using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Owner
{
    internal class GuestReviewPageViewModel
    {
        private GuestReviewService _guestReviewService;
        private MainWindowViewModel _mainwindowVM;

        public User LoggedInUser { get; private set; }
        public User Guest { get; private set; }
        public int CleanlinessRating { get;  set; }
        public int RulesRating { get;  set; }
        public string Comment { get;  set; }
        public RelayCommand Cancel { get; private set; }
        public RelayCommand AddReview { get; private set; }
        public GuestReviewPageViewModel(User user, MainWindowViewModel mainWindowVM,User guest)
        {
            LoggedInUser = user;
            Guest = guest;
            _guestReviewService = new();
            _mainwindowVM = mainWindowVM;
            CleanlinessRating = 5;
            RulesRating = 5;
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
            if (Comment == null) Comment = "";
            _guestReviewService.Save(new GuestReview(LoggedInUser.Id, Guest.Id, CleanlinessRating, RulesRating, Comment));


            _mainwindowVM.Execute_NavigationButtonCommand("Home");
            return;
        }

        private void Execute_Cancel(object obj)
        {
            _mainwindowVM.Execute_NavigationButtonCommand("Home");
            return;
        }
    }

    public class StringToIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (value is int intValue)
            {
                return intValue == int.Parse(parameter?.ToString() ?? "0");
            }
            return false;
        }

        

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (value is bool isChecked && isChecked)
            {
                return int.Parse(parameter?.ToString() ?? "0");
            }
            return Binding.DoNothing;
        }

    }
}
