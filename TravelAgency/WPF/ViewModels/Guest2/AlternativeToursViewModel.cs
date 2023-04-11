using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest2
{
    public class AlternativeToursViewModel : ViewModel
    {
        private Window _window;
        public static ObservableCollection<TourViewModel> TourDTOs { get; set; }
        public User LoggedInUser { get; set; }
        public TourViewModel Selected { get; set; }

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

        public AlternativeToursViewModel(TourViewModel tourDTO, User loggedInUser, ObservableCollection<TourViewModel> tourDTOs, Window window)
        {
            TourDTOs = new ObservableCollection<TourViewModel>();
            LoggedInUser = loggedInUser;
            FillDTOList(tourDTO, tourDTOs);
            ReserveCommand = new RelayCommand(Execute_ReserveClick, CanExecuteMethod);
            CancelCommand = new RelayCommand(Execute_CancelClick, CanExecuteMethod);
            _window = window;
        }

        private bool CanExecuteMethod(object parameter)
        {
            return true;
        }
        private static void FillDTOList(TourViewModel tourDTO, ObservableCollection<TourViewModel> tourDTOs)
        {
            foreach (TourViewModel t in tourDTOs)
            {
                if (t.City == tourDTO.City && t.Country == tourDTO.Country && t.Ocupancy != t.MaxNumOfGuests)
                {
                    TourDTOs.Add(t);
                }
            }
        }

        private void Execute_ReserveClick(object sender)
        {
            if (Selected == null)
            {
                MessageBox.Show("Izaberi turu za rezervaciju");
            }
            else
            {
                BookTourWindow bookTourWindow = new BookTourWindow(Selected, LoggedInUser);
                bookTourWindow.Show();
                _window.Close();
            }
        }

        private void Execute_CancelClick(object sender)
        {
            ToursOverviewWindow overview = new ToursOverviewWindow(LoggedInUser);
            overview.Show();
            _window.Close();
        }
    }
}

