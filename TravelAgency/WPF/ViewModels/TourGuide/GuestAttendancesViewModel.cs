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

        private DateTime? _date;

        public DateTime? Date
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
                GuestAttendanceCardViewModel viewModel = new GuestAttendanceCardViewModel();
                viewModel.Name = _userService.GetById(guestAttendance.UserId).Username;
                SetStatusImage(viewModel, guestAttendance);
                GuestAttendanceCards.Add(viewModel);
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
