using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TravelAgency.Model;
using TravelAgency.Repository;

namespace TravelAgency.View
{
    /// <summary>
    /// Interaction logic for BookTour.xaml
    /// </summary>
    public partial class BookTour : Window
    {
        private string _vacantSeats; // potencijalno promeni naziv prom
        private string _touristNum; // potencijalno promeni naziv prom
        private Tour _selectedTour;

        private readonly TourRepository _tourRepository;
        private readonly LocationRepository _locationRepository;
        public BookTour(Tour selectedTour)
        {
            InitializeComponent();
            int vacantSeats;
            DataContext= this;
            _selectedTour= selectedTour;
            vacantSeats = selectedTour.MaxNumOfGuests - selectedTour.NumOfGuests;
            _vacantSeats = vacantSeats.ToString();
            _tourRepository = new TourRepository();
            _locationRepository = new LocationRepository();
        }

        public string VacantSeats
        {
            get { return _vacantSeats;}
            set
            {
                if(value != _vacantSeats )
                {
                    _vacantSeats = value;
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ReserveClick(object sender, RoutedEventArgs e)
        {
            if (_touristNum == null || _touristNum == "")
            {
                MessageBox.Show("Niste uneli broj osoba prilikom rezervacije");
            }
            else if (int.Parse(_touristNum) > int.Parse(_vacantSeats))
            {
                MessageBox.Show("Ne moze se rezervisati tura, nema dovoljno slobodnih mesta");
            }
            else
            {
                MessageBoxResult result = ConfirmReservation();

                   if(result == MessageBoxResult.Yes)
                   {

                        ShowAndSearchTours.Tours.Clear();
                        _selectedTour.NumOfGuests += int.Parse(_touristNum);
                       _selectedTour = _tourRepository.Update(_selectedTour);
                        ObservableCollection<Tour> Tours = new ObservableCollection<Tour>(_tourRepository.GetAll());
                        foreach(Tour tour in Tours)
                        {
                            ShowAndSearchTours.Tours.Add(tour);
                        }
                        this.Close();
                   }
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

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
