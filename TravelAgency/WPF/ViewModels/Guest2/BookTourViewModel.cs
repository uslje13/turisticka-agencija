using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.Views.Guest2;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

        private RelayCommand _backCommand;
        public RelayCommand BackCommand
        {
            get { return _backCommand; }
            set
            {
                _backCommand = value;
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

        private RelayCommand _helpCommand;

        public RelayCommand HelpCommand
        {
            get { return _helpCommand; }
            set
            {
                _helpCommand = value;
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

        private string _touristNum;
        public string TouristNum
        {
            get { return _touristNum; }
            set
            {
                _touristNum = value;
                ValidateTouristNum();
                OnPropertyChanged(nameof(TouristNum));
            }
        }

        private string _averageAge;
        public string AverageAge
        {
            get { return _averageAge; }
            set
            {
                _averageAge = value;
                ValidateAverageAge();
                OnPropertyChanged(nameof(AverageAge));
            }
        }

        private Regex _ageRegex = new Regex(@"^(?!0*(\.0+)?$)\d+(\.\d+)?$");
        private Regex _touristNumRegex = new Regex(@"^[1-9][0-9]*$");

        private bool _isAverageAgeValid;
        public bool IsAverageAgeValid
        {
            get => _isAverageAgeValid;
            set
            {
                _isAverageAgeValid = value;
                OnPropertyChanged(nameof(IsAverageAgeValid));
            }
        }

        private bool _isTouristNumValid;
        public bool IsTouristNumValid
        {
            get => _isTouristNumValid;
            set
            {
                _isTouristNumValid = value;
                OnPropertyChanged(nameof(IsTouristNumValid));
            }
        }
        private void ValidateAverageAge()
        {
            if (!string.IsNullOrEmpty(AverageAge))
            {
                IsAverageAgeValid = _ageRegex.IsMatch(AverageAge);
            }
        }

        private void ValidateTouristNum()
        {
            if (!string.IsNullOrEmpty(TouristNum))
            {
                IsTouristNumValid = _touristNumRegex.IsMatch(TouristNum);
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
            BackCommand = new RelayCommand(Execute_CancelCommand, CanExecuteMethod);
            HelpCommand = new RelayCommand(Execute_HelpCommand, CanExecuteMethod);
            ReserveCommand = new RelayCommand(Execute_ReserveCommand, CanExecuteMethod);
            FillAppoitmentViewModelList();
            FillVouchersForShowingList();
        }

        private void Execute_HelpCommand(object obj)
        {
            _window.Close();

            var currentApp = System.Windows.Application.Current;

            foreach (Window window in currentApp.Windows)
            {
                if (window is ToursOverviewWindow)
                {
                    PreviousWindowOrPageName.SetPreviousWindowOrPageName(this.GetType().Name);
                    var navigationService = ((ToursOverviewWindow)window).HelpFrame.NavigationService;
                    navigationService.Navigate(new HelpPage(LoggedInUser,Tour.Id));
                    break;
                }
            }
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

            if (IsDataCorrect() == true)
            {
                MessageBoxResult result = ConfirmReservation();
                ReservationConfirmedEvent(result);
            }
        }

        private bool IsDataCorrect()
        {
            bool isCorrect = true;
            if (Selected == null)
            {
                MessageBox.Show("Niste odabrali termin", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
                isCorrect = false;
            }
            else if (!IsTouristNumValid || _touristNum == "" || _touristNum == null)
            {
                MessageBox.Show("Broj turista nije unet u dobrom formatu", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
                isCorrect = false;
            }
            else if (!IsAverageAgeValid || _averageAge == "" || _averageAge == null)
            {
                MessageBox.Show("Prosecan broj godina nije unet u dobrom formatu", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
                isCorrect = false;
            }
            else if (int.Parse(_touristNum) > _availableSlots)
            {
                MessageBox.Show("Ne moze se rezervisati tura, nema dovoljno slobodnih mesta", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
                isCorrect = false;
            }
            return isCorrect;
        }
        private void ReservationConfirmedEvent(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                if (_selectedVoucher != null)
                {
                    _voucherService.UsedUpdate(_selectedVoucher.VoucherId);
                    _reservationService.CreateReservation(_selected, LoggedInUser, int.Parse(_touristNum), float.Parse(_averageAge), _selectedVoucher.VoucherId);
                }
                else
                {
                    _reservationService.CreateReservation(_selected, LoggedInUser, int.Parse(_touristNum), float.Parse(_averageAge));
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
                if(appointment.TourId == Tour.Id && appointment.Start > DateTime.Now)
                {
                    AvailableSlots = Tour.MaxNumOfGuests - appointment.Occupancy;
                    AppoitmentOverviewViewModel viewModel = new AppoitmentOverviewViewModel(appointment.Start, AvailableSlots, Tour.Id);
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
