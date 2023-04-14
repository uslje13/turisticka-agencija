using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Repositories;

namespace SOSTeam.TravelAgency.WPF.Views
{
    /// <summary>
    /// Interaction logic for ShowNotificationsWindow.xaml
    /// </summary>
    public partial class ShowNotificationsWindow : Window
    {
        private NotificationRepository _notificationRepository;
        private GuestReviewRepository _guestReviewRepository;
        public static ObservableCollection<Notification> Notifications { get; set; }
        public Notification SelectedNotification { get; set; }
        public User LoggedInUser { get; set; }

        public ShowNotificationsWindow(User user,NotificationRepository notificationRepository, GuestReviewRepository guestReviewRepository)
        {

            DataContext = this;
            Notifications = new ObservableCollection<Notification>();
            _notificationRepository = notificationRepository;
            _guestReviewRepository = guestReviewRepository;
            LoggedInUser = user;
            FillObservableCollection();

            InitializeComponent();
            _guestReviewRepository = guestReviewRepository;
        }

        private void FillObservableCollection()
        {
            foreach (Notification notification in _notificationRepository.GetAll())
            {
                if (notification.UserId == LoggedInUser.Id)
                {
                    Notifications.Add(notification);
                }
            }
        }

        
        private void DeleteClick(object sender, RoutedEventArgs e)
        {
           /* if (SelectedNotification != null)
            {
                SelectedNotification.Read = true;
                _notificationRepository.Delete(SelectedNotification);
                Update();
            }*/
        }

        private void OpenNotificationClick(object sender, MouseButtonEventArgs e)
        {
            var notification = (sender as ListView).SelectedItem as Notification;

            if (SelectedNotification == null) return;

            if(SelectedNotification.Type == Notification.NotificationType.GUESTREVIEW) 
            {
                CreateGuestReviewWindow createGuestReview = new CreateGuestReviewWindow(LoggedInUser, SelectedNotification.GuestId,_guestReviewRepository);
                SelectedNotification.Read = true;
                _notificationRepository.Update(SelectedNotification);
                createGuestReview.ShowDialog();
                Update();
                
            }
            
        }


        private void MarkAsReadClick(object sender, RoutedEventArgs e)
        {
            if (SelectedNotification != null)
            {
                SelectedNotification.Read = true;
                _notificationRepository.Update(SelectedNotification);
                Update();
            }
        }

        private void Update() 
        {
            Notifications.Clear();
            FillObservableCollection();
        }
    }

}
