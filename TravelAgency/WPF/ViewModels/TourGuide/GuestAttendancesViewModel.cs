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
        private ObservableCollection<GuestAttendanceCardViewModel> _guestCards;

        public ObservableCollection<GuestAttendanceCardViewModel> GuestCards
        {
            get => _guestCards;
            set
            {
                if (_guestCards != value)
                {
                    _guestCards = value;
                    OnPropertyChanged("GuestCards");
                }
            }
        }

        private DateTime _date;

        public DateTime Date
        {
            get => _date;
            set
            {
                if (_date != value)
                {
                    _date = value;
                    OnPropertyChanged("Date");
                }
            }
        }

        private string _tourName;
        public string TourName
        {
            get => _tourName;
            set
            {
                if (_tourName != value)
                {
                    _tourName = value;
                    OnPropertyChanged("TourName");
                }
            }
        }

        private string _checkpointName;
        public string CheckpointName
        {
            get => _checkpointName;
            set
            {
                if (_checkpointName != value)
                {
                    _checkpointName = value;
                    OnPropertyChanged("CheckpointName");
                }
            }
        }


        private readonly GuestAttendanceService _guestAttendanceService;
        private readonly UserService _userService;

        public GuestAttendancesViewModel(CheckpointActivity selectedCheckpointActivity)
        {
            _guestAttendanceService = new GuestAttendanceService();
            _userService = new UserService();
            FillObservableCollection(selectedCheckpointActivity);
        }

        private void FillObservableCollection(CheckpointActivity selectedCheckpointActivity)
        {
            foreach (var guestAttendance in _guestAttendanceService.GetAllByActivityId(selectedCheckpointActivity.Id))
            {
                GuestAttendanceCardViewModel viewModel = new GuestAttendanceCardViewModel();
                viewModel.Name = _userService.GetById(guestAttendance.UserId).Username;
                SetStatusImage(viewModel, guestAttendance);
                GuestCards.Add(viewModel);
            }
        }

        private void SetStatusImage(GuestAttendanceCardViewModel viewModel, GuestAttendance guestAttendance)
        {
            if (guestAttendance.Presence == GuestPresence.YES)
            {
                viewModel.StatusImage = "/Resources/Icons/checkpoint_yes.png";
            }
            else if (guestAttendance.Presence == GuestPresence.NO)
            {
                viewModel.StatusImage = "/Resources/Icons/checkpoint_no.png";
            }
            else
            {
                viewModel.StatusImage = "/Resources/Icons/checkpoint_unknown.png";
            }
        }
    }
}
