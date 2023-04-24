using SOSTeam.TravelAgency.Application.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.WPF.ViewModels.TourGuide
{
    public class StatByTourViewModel : ViewModel
    {
        private ObservableCollection<AttendanceAgeRangeViewModel> _statByAgeRange;

        public ObservableCollection<AttendanceAgeRangeViewModel> StatByAgeRange
        {
            get => _statByAgeRange;
            set
            {
                if (_statByAgeRange != value)
                {
                    _statByAgeRange = value;
                    OnPropertyChanged("StatByAgeRange");
                }
            }
        }

        public string PercentOfGuestsWithVoucher { get; set; }
        public string PercentOfGuestsWithoutVoucher { get; set; }


        public string TourName { get; set; }
        public DateTime Date { get; set; }

        private readonly TourStatsService _tourStatsService;

        public StatByTourViewModel(TourCardViewModel selectedTourCard)
        {
            StatByAgeRange = new ObservableCollection<AttendanceAgeRangeViewModel>();

            TourName = selectedTourCard.Name;
            Date = selectedTourCard.Start;

            _tourStatsService = new TourStatsService();

            var percentWithVoucher = _tourStatsService
                .GetPercentsOfGuestAttendancesVoucher(selectedTourCard.AppointmentId).Item1;
            var percentWithoutVoucher =
                _tourStatsService.GetPercentsOfGuestAttendancesVoucher(selectedTourCard.AppointmentId).Item2;

            PercentOfGuestsWithVoucher = Math.Round(percentWithVoucher, 2) * 100 + "%";
            PercentOfGuestsWithoutVoucher = Math.Round(percentWithoutVoucher, 2) * 100 + "%";

            CreateStatByAgeRange(selectedTourCard);
        }

        private void CreateStatByAgeRange(TourCardViewModel selectedTourCard)
        {
            var numOfGuestsByAgeGroup = _tourStatsService.CountNumOfGuestsByAgeGroup(selectedTourCard.AppointmentId);

            var attendanceFirstRange = new AttendanceAgeRangeViewModel
            {
                AgeGroup = "0-18",
                NumOfGuests = numOfGuestsByAgeGroup.Item1
            };
            var attendanceSecondRange = new AttendanceAgeRangeViewModel
            {
                AgeGroup = "18-50",
                NumOfGuests = numOfGuestsByAgeGroup.Item2
            };
            var attendanceThirdRange = new AttendanceAgeRangeViewModel
            {
                AgeGroup = ">50",
                NumOfGuests = numOfGuestsByAgeGroup.Item3
            };

            StatByAgeRange.Add(attendanceFirstRange);
            StatByAgeRange.Add(attendanceSecondRange);
            StatByAgeRange.Add(attendanceThirdRange);
        }

    }
}
