using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest2
{
    public class BookTourViewModel : ViewModel
    {
        private Window _window;
        public User LoggedInUser { get; set; }
        public Tour Tour { get; set; }
        private int _availableSlots;

        private VouchersViewModel _selectedVoucher;

        public VouchersViewModel SelectedVoucher
        {
            get { return _selectedVoucher; }
            set
            {
                _selectedVoucher = value;
                OnPropertyChanged("SelectedVoucher");
            }
        }

        private AppoitmentOverviewViewModel _selected;

        public AppoitmentOverviewViewModel Selected
        {
            get { return _selected; }
            set
            {
                _selected = value;
                OnPropertyChanged("Selected");
            }
        }

        public static ObservableCollection<Appointment> Appointments;
        private readonly AppointmentService _appointmentService;
        private readonly ReservationService _reservationService;
        private readonly TourService _tourService;
        private readonly VoucherService _voucherService;

        public static ObservableCollection<AppoitmentOverviewViewModel> AppoitmentOverviewViewModels { get; set; }
        public static ObservableCollection<VouchersViewModel> Vouchers { get; set; }

        private RelayCommand _cancelCommand;
        public RelayCommand CancelCommand
        {
            get { return _cancelCommand; }
            set
            {
                _cancelCommand = value;
            }
        }

        private RelayCommand _reserveCommand;

        public RelayCommand ReserveCommand
        {
            get { return _reserveCommand; }
            set
            {
                _reserveCommand = value;
            }
        }
        public int AvailableSlots
        {
            get { return _availableSlots; }
            set
            {
                if (value != _availableSlots)
                {
                    _availableSlots = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _touristNum;
        public int TouristNum
        {
            get { return _touristNum; }
            set
            {
                if (value != _touristNum)
                {
                    _touristNum = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _averageAge;
        public string AverageAge
        {
            get { return _averageAge; }
            set
            {
                if (value != _averageAge)
                {
                    _averageAge = value;
                    OnPropertyChanged();
                }
            }
        }

        public BookTourViewModel(int id, User loggedInUser,Window window)
        {
            LoggedInUser = loggedInUser;
            _appointmentService = new AppointmentService();
            _reservationService = new ReservationService();
            _tourService = new TourService();
            _voucherService = new VoucherService();
            Appointments = new ObservableCollection<Appointment>(_appointmentService.GetAll());
            AppoitmentOverviewViewModels = new ObservableCollection<AppoitmentOverviewViewModel>();
            Vouchers = new ObservableCollection<VouchersViewModel>();
            Tour = _tourService.FindTourById(id);
            _window = window;
            CancelCommand = new RelayCommand(Execute_CancelCommand, CanExecuteMethod);
            ReserveCommand = new RelayCommand(Execute_ReserveCommand, CanExecuteMethod);
            FillAppoitmentViewModelList();
            FillVouchersForShowingList();
        }

        private bool CanExecuteMethod(object parameter)
        {
            return true;
        }

        private void Execute_CancelCommand(object sender)
        {
            _window.Close();
        }
        private void Execute_ReserveCommand(object sender)
        {
            if(Selected == null)
            {
                MessageBox.Show("Niste odabrali termin");
            }
            else if (_touristNum == null || _touristNum == 0 || _averageAge == null)
            {
                MessageBox.Show("Niste popunili potrebne podatke");
            }
            else if (_touristNum > _availableSlots)
            {
                MessageBox.Show("Ne moze se rezervisati tura, nema dovoljno slobodnih mesta");
            }
            else
            {
                MessageBoxResult result = ConfirmReservation();
                ReservationConfirmedEvent(result);
            }
        }

        private void ReservationConfirmedEvent(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                if (_selectedVoucher != null)
                {
                    _voucherService.UsedUpdate(_selectedVoucher.VoucherId);
                    _reservationService.CreateReservation(_selected, LoggedInUser, _touristNum, float.Parse(_averageAge), _selectedVoucher.VoucherId);
                }
                else
                {
                    _reservationService.CreateReservation(_selected, LoggedInUser, _touristNum, float.Parse(_averageAge));
                }
                _window.Close();
            }
        }

        private MessageBoxResult ConfirmReservation()
        {
            string sMessageBoxText = $"Da li ste sigurni da želite da rezervisete turu";
            string sCaption = "Porvrda rezervacije";

            MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
            MessageBoxImage icnMessageBox = MessageBoxImage.Warning;

            MessageBoxResult result = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
            return result;
        }

        public void FillAppoitmentViewModelList()
        {
            foreach(Appointment appointment in Appointments)
            {
                if(appointment.TourId == Tour.Id && appointment.Date > DateOnly.FromDateTime(DateTime.Today))
                {
                    AvailableSlots = Tour.MaxNumOfGuests - appointment.Occupancy;
                    AppoitmentOverviewViewModel viewModel = new AppoitmentOverviewViewModel(appointment.Date, appointment.Time, AvailableSlots, Tour.Id);
                    AppoitmentOverviewViewModels.Add(viewModel);
                }
            }
        }

        private void FillVouchersForShowingList()
        {
            foreach (Voucher voucher in _voucherService.GetAll())
            {
                if (voucher.ExpiryDate > DateOnly.FromDateTime(DateTime.Today) && voucher.Used == false)
                {
                    Vouchers.Add(new VouchersViewModel(voucher.Id, voucher.ExpiryDate));
                }
            }
        }
    }
}
