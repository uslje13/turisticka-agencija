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
using TravelAgency.Converter;
using TravelAgency.DTO;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Repositories;

namespace SOSTeam.TravelAgency.WPF.Views
{
    /// <summary>
    /// Interaction logic for ShowAccommodation.xaml
    /// </summary>
    public partial class ShowAccommodationsWindow : Window
    {
        public User LoggedInUser { get; set; }
        private AccommodationRepository _accommodationRepository;
        private GuestReviewRepository _guestReviewRepository;
        private NotificationRepository _notificationRepository;
        private AccommodationReservationRepository _accommodationReservationRepository;

        private LocationConverter _locationConverter;
        public static ObservableCollection<AccommodationDTO> Accommodations { get; set; }
        public AccommodationDTO SelectedAccommodation { get; set; }
        
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public ShowAccommodationsWindow(User user)
        {
            DataContext = this;

            LoggedInUser = user;
            _accommodationRepository = new();
            _guestReviewRepository = new();
            _locationConverter = new();
            _notificationRepository = new();
            _accommodationReservationRepository = new();
            Accommodations = new ObservableCollection<AccommodationDTO>();
            FillObservableCollection(Accommodations);
            
            InitializeComponent();
            Show();
            
            AddNotifications();
            CheckForNotifications();
        }

        private void FillObservableCollection(ObservableCollection<AccommodationDTO> accommodations)
        {
            foreach (Accommodation item in _accommodationRepository.GetAll())
            {
                if (item.OwnerId != LoggedInUser.Id) continue;
                string location = string.Empty;
                if (item.LocationId != -1)
                {
                    location = _locationConverter.GetFullNameById(item.LocationId);
                }
                accommodations.Add(new AccommodationDTO(item.Id,item.Name,location,item.Type,item.MaxGuests,item.MinDaysStay,item.MinDaysForCancelation));
            }
        }

        public void AddNotifications() 
        {
            foreach (var reservation in _accommodationReservationRepository.GetAll())
            {
                foreach (AccommodationDTO accommodation in Accommodations)
                {
                    if(reservation.AccommodationId == accommodation.Id && IsInLastFiveDays(reservation.LastDay) && !IsReviewedGuest(LoggedInUser.Id,reservation.UserId)) 
                    {
                        _notificationRepository.Save(new Notification(
                            LoggedInUser.Id,
                            "Ocenite korisnika",
                            Notification.NotificationType.GUESTREVIEW,
                            false,
                            reservation.UserId
                            ));
                    }
                }
            } 
        }

        private bool IsReviewedGuest(int ownerId,int userId)
        {
            return _guestReviewRepository.ReviewExists(ownerId,userId);
        }

        private bool IsInLastFiveDays(DateTime time) 
        {
            int dayDifference = DateTime.Now.DayOfYear - time.DayOfYear;
            return  dayDifference <= 5 && dayDifference > -1;
        }


        public void CheckForNotifications()
        {
            foreach(var notification in _notificationRepository.GetAll()) 
            {
                if(notification.UserId == LoggedInUser.Id && !notification.Read) 
                {
                    NotificationButtonClick(null, null);
                    return;
                }
            }
        }

        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
            CreateAccommodationWindow createAccommodation = new CreateAccommodationWindow(_accommodationRepository, LoggedInUser);
            createAccommodation.ShowDialog();
            UpdateAccommodations();
        }

        private void DeleteButtonClick(object sender, RoutedEventArgs e)
        {
            if (SelectedAccommodation != null && ConfirmAccommodationDeletion() == MessageBoxResult.Yes)
                _accommodationRepository.Delete(SelectedAccommodation.Id);
            UpdateAccommodations();


        }

        private void NotificationButtonClick(object sender, RoutedEventArgs e)
        {
            ShowNotificationsWindow showNotificationsWindow = new ShowNotificationsWindow(LoggedInUser,_notificationRepository,_guestReviewRepository);
            showNotificationsWindow.ShowDialog();
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
