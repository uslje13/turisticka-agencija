using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.Views.Owner;
using SOSTeam.TravelAgency.WPF.Views.TourGuide;
using GuestReviewPage = SOSTeam.TravelAgency.WPF.Views.TourGuide.GuestReviewPage;
using MainWindow = SOSTeam.TravelAgency.WPF.Views.TourGuide.MainWindow;

namespace SOSTeam.TravelAgency.WPF.ViewModels.TourGuide
{
    public class GuestReviewsOverviewViewModel : ViewModel
    {
        private ObservableCollection<GuestReviewCardViewModel> _guestReviewCards;

        public ObservableCollection<GuestReviewCardViewModel> GuestReviewCards
        {
            get => _guestReviewCards;
            set
            {
                if (_guestReviewCards != value)
                {
                    _guestReviewCards = value;
                    OnPropertyChanged("GuestReviewCards");
                }
            }
        }

        public TourCardViewModel SelectedTour { get; set; }

        public string TourName { get; set; }
        public DateTime Date { get; set; }

        private readonly TourReviewService _tourReviewService;
        private readonly TourService _tourService;
        private readonly  UserService _userService;

        public RelayCommand ShowReviewDetailsCommand { get; set; }

        public GuestReviewsOverviewViewModel(TourCardViewModel selectedTour)
        {
            _tourReviewService = new TourReviewService();
            _userService = new UserService();
            _tourService = new TourService();
            SelectedTour = selectedTour;
            GuestReviewCards = new ObservableCollection<GuestReviewCardViewModel>();
            TourName = _tourService.GetById(SelectedTour.TourId).Name;
            Date = new DateTime(SelectedTour.Date.Year, SelectedTour.Date.Month, SelectedTour.Date.Day,
                                SelectedTour.Time.Hour, SelectedTour.Time.Minute, SelectedTour.Time.Second);
            ShowReviewDetailsCommand = new RelayCommand(ShowGuestReviewDetails, CanExecuteMethod);

            FillObservableCollection();
        }

        private bool CanExecuteMethod(object parameter)
        {
            return true;
        }

        private void FillObservableCollection()
        {
            foreach (var tourReview in _tourReviewService.GetAllByAppointmentId(SelectedTour.AppointmentId))
            {
                var guestReviewCard = new GuestReviewCardViewModel
                {
                    ReviewId = tourReview.Id,
                    AppointmentId = SelectedTour.AppointmentId,
                    UserId = tourReview.UserId,
                    Date = SelectedTour.Date,
                    AvgGrade = Math.Round(FindAvgGrade(tourReview), 1) + "/5.0",
                    KnowledgeGrade = tourReview.GuideKnowledge,
                    LanguageGrade = tourReview.GuideKnowledge,
                    InterestingGrade = tourReview.InterestRating,
                    Comment = tourReview.Comment,
                    GuestName = _userService.GetById(tourReview.UserId).Username,
                    TourName = _tourService.GetById(SelectedTour.TourId).Name,
                };
                SetReportedReviewImage(guestReviewCard, tourReview);
                GuestReviewCards.Add(guestReviewCard);
            }
        }

        private void SetReportedReviewImage(GuestReviewCardViewModel guestReviewCard, TourReview tourReview)
        {
            if (tourReview.Reported)
            {
                guestReviewCard.ReportedImage = "/Resources/Icons/reported.png";
            }
        }

        private double FindAvgGrade(TourReview tourReview)
        {
            double avgGrade = 0;
            avgGrade += tourReview.GuideKnowledge;
            avgGrade += tourReview.GuideLanguage;
            avgGrade += tourReview.InterestRating;

            return avgGrade / 3;
        }

        private void ShowGuestReviewDetails(object sender)
        {
            var selectedAppointment = sender as GuestReviewCardViewModel;
            GuestReviewPage guestReviewPage = new GuestReviewPage(selectedAppointment);
            System.Windows.Application.Current.Windows.OfType<MainWindow>().FirstOrDefault().ToursOverviewFrame.Content = guestReviewPage;
        }

    }
}
