using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Interaction logic for ShowAccommodation.xaml
    /// </summary>
    public partial class ShowAccommodations : Window
    {
        public User LoggedInUser { get; set; }
        private AccommodationRepository _accommodationRepository;
        private GuestReviewRepository _guestReviewRepository;

        private LocationRepository _locationRepository;
        public static ObservableCollection<Accommodation> Accommodations { get; set; }
        public Accommodation SelectedAccommodation { get; set; }
        
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public ShowAccommodations(User user)
        {
            DataContext = this;

            LoggedInUser = user;
            _accommodationRepository = new AccommodationRepository();
            _guestReviewRepository = new GuestReviewRepository();
            _locationRepository = new LocationRepository();
            Accommodations = new ObservableCollection<Accommodation>();
            FillObservableCollection(Accommodations);

            InitializeComponent();
        }

        private void FillObservableCollection(ObservableCollection<Accommodation> accommodations)
        {
            foreach (Accommodation item in _accommodationRepository.GetAll())
            {
                if (item.OwnerId != LoggedInUser.Id) continue;
                if (item.LocationId != -1)
                {
                    item.Location = _locationRepository.GetById(item.LocationId);
                }
                accommodations.Add(item);
            }
        }

        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
            CreateAccommodation createAccommodation = new CreateAccommodation(_accommodationRepository, LoggedInUser);
            createAccommodation.ShowDialog();
            UpdateAccommodations();
        }

        private void DeleteButtonClick(object sender, RoutedEventArgs e)
        {
            if (SelectedAccommodation != null && ConfirmAccommodationDeletion() == MessageBoxResult.Yes)
                _accommodationRepository.Delete(SelectedAccommodation);
            UpdateAccommodations();


        }

        private void ReviewButtonClick(object sender, RoutedEventArgs e)
        {
            CreateGuestReview createGuestReview = new CreateGuestReview(LoggedInUser,2,_guestReviewRepository);
            createGuestReview.ShowDialog();
        }

        private MessageBoxResult ConfirmAccommodationDeletion()
        {

            string sMessageBoxText = $"Da li ste sigurni da želite da obrišete smeštaj?";
            string sCaption = "Brisanje smeštaja";

            MessageBoxButton messageBoxButton = MessageBoxButton.YesNo;
            MessageBoxImage messageBoxIcon = MessageBoxImage.Warning;

            MessageBoxResult result = MessageBox.Show(sMessageBoxText, sCaption, messageBoxButton, messageBoxIcon);
            return result;
        }

        public void UpdateAccommodations() 
        {
            Accommodations.Clear();
            FillObservableCollection(Accommodations);
        }
    }
}
