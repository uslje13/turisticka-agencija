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

        private ObservableCollection<VoucherStatPieViewModel> _statByVoucher;

        public ObservableCollection<VoucherStatPieViewModel> StatByVoucher
        {
            get => _statByVoucher;
            set
            {
                if (_statByVoucher != value)
                {
                    _statByVoucher = value;
                    OnPropertyChanged("StatByVoucher");
                }
            }
        }

        private readonly ReservationService _reservationService;

        public StatByTourViewModel(TourCardViewModel selectedTourCard)
        {
            StatByAgeRange = new ObservableCollection<AttendanceAgeRangeViewModel>();
            StatByVoucher = new ObservableCollection<VoucherStatPieViewModel>();
            _reservationService = new ReservationService();
            CreateStatByAgeRange(selectedTourCard);
            CreateVoucherPie(selectedTourCard);
        }

        private void CreateStatByAgeRange(TourCardViewModel selectedTourCard)
        {
            AttendanceAgeRangeViewModel attendanceFirstRange = new AttendanceAgeRangeViewModel();
            AttendanceAgeRangeViewModel attendanceSecondRange = new AttendanceAgeRangeViewModel();
            AttendanceAgeRangeViewModel attendanceThirdRange = new AttendanceAgeRangeViewModel();
            attendanceFirstRange.AgeGroup = "0-18";
            attendanceSecondRange.AgeGroup = "18-50";
            attendanceThirdRange.AgeGroup = ">50";
            foreach (var reservation in _reservationService.GetAllByAppointmentId(selectedTourCard.AppointmentId))
            {
                if (reservation.Presence)
                {
                    if (reservation.AverageAge <= 18)
                    {
                        attendanceFirstRange.NumOfGuests += reservation.TouristNum;
                    }
                    else if (reservation.AverageAge > 18 && reservation.AverageAge <= 50)
                    {
                        attendanceSecondRange.NumOfGuests += reservation.TouristNum;
                    }
                    else
                    {
                        attendanceThirdRange.NumOfGuests += reservation.TouristNum;
                    }
                }
            }
            StatByAgeRange.Add(attendanceFirstRange);
            StatByAgeRange.Add(attendanceSecondRange);
            StatByAgeRange.Add(attendanceThirdRange);
        }

        private void CreateVoucherPie(TourCardViewModel selectedTourCard)
        {
            VoucherStatPieViewModel withVoucher = new VoucherStatPieViewModel();
            VoucherStatPieViewModel withoutVoucher = new VoucherStatPieViewModel();
            withVoucher.Type = "With voucher";
            withoutVoucher.Type = "Without voucher";
            foreach (var reservation in _reservationService.GetAllByAppointmentId(selectedTourCard.AppointmentId))
            {
                if (reservation.Presence)
                {
                    if (reservation.VoucherId == -1)
                    {
                        withVoucher.NumOfGuests += reservation.TouristNum;
                    }
                    else if (reservation.AverageAge > 18 && reservation.AverageAge <= 50)
                    {
                        withVoucher.NumOfGuests += reservation.TouristNum;
                    }
                }
            }
            StatByVoucher.Add(withVoucher);
            StatByVoucher.Add(withoutVoucher);
        }

    }
}
