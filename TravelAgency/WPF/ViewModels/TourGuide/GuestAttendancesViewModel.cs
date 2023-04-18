using SOSTeam.TravelAgency.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOSTeam.TravelAgency.Application.Services;

namespace SOSTeam.TravelAgency.WPF.ViewModels.TourGuide
{
    public class GuestAttendancesViewModel : ViewModel
    {
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

        public DateTime? Date { get; set; }

        public string TourName { get; set; }

        public string CheckpointName { get; set; }

        private readonly GuestAttendanceService _guestAttendanceService;
        private readonly UserService _userService;

        public GuestAttendancesViewModel(CheckpointCardViewModel selectedCheckpointActivityCard, string tourName, DateTime? date)
        {
            _guestAttendanceService = new GuestAttendanceService();
            GuestAttendanceCards = new ObservableCollection<GuestAttendanceCardViewModel>();
            _userService = new UserService();
            TourName = tourName;
            Date = date;
            CheckpointName = selectedCheckpointActivityCard.Name;

            FillObservableCollection(selectedCheckpointActivityCard);
        }

        private void FillObservableCollection(CheckpointCardViewModel selectedCheckpointActivity)
        {
            foreach (var guestAttendance in _guestAttendanceService.GetAllByActivityId(selectedCheckpointActivity.ActivityId))
            {
                var guestAttendanceCard = new GuestAttendanceCardViewModel
                {
                    Name = _userService.GetById(guestAttendance.UserId).Username
                };
                guestAttendanceCard.SetStatusImage(guestAttendance);
                GuestAttendanceCards.Add(guestAttendanceCard);
            }
        }
    }
}
