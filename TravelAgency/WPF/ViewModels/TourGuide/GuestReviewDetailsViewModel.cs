﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;

namespace SOSTeam.TravelAgency.WPF.ViewModels.TourGuide
{
    public class GuestReviewDetailsViewModel : ViewModel
    {
        public GuestReviewCardViewModel SelectedReview { get; set; }

        private ObservableCollection<GuestAttendanceCardViewModel> _guestAttendanceCards;

        public ObservableCollection<GuestAttendanceCardViewModel> GuestAttendanceCards
        {
            get => _guestAttendanceCards;
            set
            {
                if (_guestAttendanceCards != value)
                {
                    _guestAttendanceCards = value;
                    OnPropertyChanged("GuestAttendanceCards");
                }
            }
        }

        private readonly CheckpointActivityService _checkpointActivityService;
        private readonly GuestAttendanceService _guestAttendanceService;
        private readonly CheckpointService _checkpointService;
        private readonly TourReviewService _tourReviewService;

        public RelayCommand ReportReviewCommand { get; set; }

        public GuestReviewDetailsViewModel(GuestReviewCardViewModel selectedReview)
        {
            SelectedReview = selectedReview;
            _checkpointActivityService = new CheckpointActivityService();
            _guestAttendanceService = new GuestAttendanceService();
            _checkpointService = new CheckpointService();
            _tourReviewService = new TourReviewService();
            GuestAttendanceCards = new ObservableCollection<GuestAttendanceCardViewModel>();
            ReportReviewCommand = new RelayCommand(ReportComment, CanExecuteMethod);

            FillObservableCollection();
        }

        private bool CanExecuteMethod(object parameter)
        {
            return true;
        }

        private void FillObservableCollection()
        {
            foreach (var checkpointActivity in _checkpointActivityService.GetAllByAppointmentId(SelectedReview.AppointmentId))
            {
                foreach (var guestAttendance in _guestAttendanceService.GetByUserId(SelectedReview.UserId))
                {
                    if (checkpointActivity.Id == guestAttendance.CheckpointActivityId)
                    {
                        var guestAttendanceCard = new GuestAttendanceCardViewModel
                        {
                            Name = _checkpointService.GetById(checkpointActivity.CheckpointId).Name
                        };
                        guestAttendanceCard.SetStatusImage(guestAttendance);
                        GuestAttendanceCards.Add(guestAttendanceCard);
                    }
                }
            }
        }

        private void ReportComment(object sender)
        {
            TourReview tourReview = _tourReviewService.GetById(SelectedReview.ReviewId);
            tourReview.Reported = true;
            _tourReviewService.Update(tourReview);
        }

    }
}
