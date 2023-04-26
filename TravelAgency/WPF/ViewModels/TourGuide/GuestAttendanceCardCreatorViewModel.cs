using SOSTeam.TravelAgency.Application.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.WPF.ViewModels.TourGuide
{
    public class GuestAttendanceCardCreatorViewModel
    {
        private readonly GuestAttendanceService _guestAttendanceService;
        private readonly UserService _userService;

        public GuestAttendanceCardCreatorViewModel()
        {
            _guestAttendanceService = new GuestAttendanceService();
            _userService = new UserService();
        }
        public ObservableCollection<GuestAttendanceCardViewModel> CreateCards(CheckpointCardViewModel selectedCheckpointActivity)
        {
            var guestAttendanceCards = new ObservableCollection<GuestAttendanceCardViewModel>();
            foreach (var guestAttendance in _guestAttendanceService.GetAllByActivityId(selectedCheckpointActivity.ActivityId))
            {
                var guestAttendanceCard = new GuestAttendanceCardViewModel
                {
                    Name = _userService.GetById(guestAttendance.UserId).Username
                };
                guestAttendanceCard.SetStatusImageAndBackground(guestAttendance);
                guestAttendanceCards.Add(guestAttendanceCard);
            }
            return guestAttendanceCards;
        }
    }
}
