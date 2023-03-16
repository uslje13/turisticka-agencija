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
using TravelAgency.Model;
using TravelAgency.Repository;

namespace TravelAgency.View
{
    /// <summary>
    /// Interaction logic for ShowNotificationsWindow.xaml
    /// </summary>
    public partial class ShowNotificationsWindow : Window
    {
        private NotificationRepository _notificationRepository;
        public static ObservableCollection<Notification> Notifications { get; set; }
        public Notification SelectedNotification { get; set; }
        public User LoggedInUser { get; set; }

        public ShowNotificationsWindow(User user)
        {

            DataContext = this;
            Notifications = new ObservableCollection<Notification>();
            _notificationRepository = new();
            LoggedInUser = user;
            FillObservableCollection();

            InitializeComponent();
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
            if (SelectedNotification != null)
            {
                SelectedNotification.Read = true;
                _notificationRepository.Delete(SelectedNotification);
                Update();
            }
        }

        private void OpenNotificationClick(object sender, MouseButtonEventArgs e)
        {
            var notification = (sender as ListView).SelectedItem as Notification;

            if (SelectedNotification == null) return;

            if(SelectedNotification.Type == Notification.NotificationType.GUESTREVIEW) 
            {
                CreateGuestReview createGuestReview = new CreateGuestReview(LoggedInUser, SelectedNotification.GuestId, null);
                createGuestReview.ShowDialog();
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
