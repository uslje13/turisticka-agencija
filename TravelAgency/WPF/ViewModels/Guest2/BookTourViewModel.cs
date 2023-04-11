using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.RepositoryInterfaces;
using System.Windows;
using SOSTeam.TravelAgency.WPF.Views;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest2
{
    public class BookTourViewModel : ViewModel
    {
        private Window _window;
        public User LoggedInUser { get; set; }
        private string _availableSlots;
        private string _touristNum;
        private TourViewModel _selected;

        public static ObservableCollection<Appointment> Appointments;
        private readonly AppointmentService _appointmentService;
        private readonly ReservationService _reservationService;

        private RelayCommand cancelCommand;
        public RelayCommand CancelCommand
        {
            get { return cancelCommand; }
            set
            {
                cancelCommand = value;
            }
        }

        private RelayCommand reserveCommand;

        public RelayCommand ReserveCommand
        {
            get { return reserveCommand; }
            set
            {
                reserveCommand = value;
            }
        }

        public string AvailableSlots
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

        public string TouristNum
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

        public BookTourViewModel(TourViewModel selected, User loggedInUser, Window window)
        {
            LoggedInUser = loggedInUser;
            _selected = selected;
            _availableSlots = (selected.MaxNumOfGuests - selected.Ocupancy).ToString();
            _appointmentService = new AppointmentService();
            _reservationService = new ReservationService();
            Appointments = new ObservableCollection<Appointment>(_appointmentService.GetAll());
            CancelCommand = new RelayCommand(Execute_CancelClick, CanExecuteMethod);
            ReserveCommand = new RelayCommand(Execute_ReserveClick, CanExecuteMethod);
            _window = window;
        }
        private bool CanExecuteMethod(object parameter)
        {
            return true;
        }

        private void Execute_ReserveClick(object sender)
        {
            if (_touristNum == null || _touristNum == "" || int.Parse(_touristNum) == 0)
            {
                MessageBox.Show("Niste uneli broj osoba prilikom rezervacije");
            }
            else if (int.Parse(_touristNum) > int.Parse(_availableSlots))
            {
                MessageBox.Show("Ne moze se rezervisati tura, nema dovoljno slobodnih mesta");
            }
            else
            {
                MessageBoxResult result = ConfirmReservation();

                if (result == MessageBoxResult.Yes)
                {
                    _reservationService.CreateReservation(_selected,LoggedInUser,_touristNum);
                    OpenToursOverviewWindow();
                    _window.Close();
                }
            }
        }

        private void OpenToursOverviewWindow()
        {
            ToursOverviewWindow overview = new ToursOverviewWindow(LoggedInUser);
            overview.Show();
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

        private void Execute_CancelClick(object sender)
        {
            OpenToursOverviewWindow();
            _window.Close();
        }
    }
}
