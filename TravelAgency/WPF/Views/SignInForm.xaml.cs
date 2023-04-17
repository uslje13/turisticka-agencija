using System;
using System.Collections.Generic;
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
using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Repositories;
using SOSTeam.TravelAgency.WPF.ViewModels.Guest1;
using SOSTeam.TravelAgency.WPF.Views.Guest1;
using SOSTeam.TravelAgency.WPF.Views.Guest2;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace SOSTeam.TravelAgency.WPF.Views
{
    /// <summary>
    /// Interaction logic for SignInForm.xaml
    /// </summary>
    public partial class SignInForm : Window
    {

        private readonly UserRepository _repository;

        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                if (value != _username)
                {
                    _username = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public SignInForm()
        {
            InitializeComponent();
            DataContext = this;
            _repository = new UserRepository();
        }

        private void SignIn(object sender, RoutedEventArgs e)
        {
            User user = _repository.GetByUsername(Username);
            if (user != null)
            {
                if (user.Password == txtPassword.Password && user.Role == Roles.OWNER)
                {
                    Views.Owner.MainWindow mainPage = new Views.Owner.MainWindow(user);
                    mainPage.Show();
                    Close();
                }
                else if (user.Password == txtPassword.Password && user.Role == Roles.TOURISTGUIDE)
                {
                    //ShowToursWindow showToursWindow = new ShowToursWindow(user);
                    //showToursWindow.Show();
                    //Close();
                }
                else if (user.Password == txtPassword.Password && user.Role == Roles.GUEST1)
                {
                    UserProfilleWindow userProfilleWindow = new UserProfilleWindow(user, TestInboxCharge(user.Id));
                    userProfilleWindow.Show();
                    Close();
                }
                else if (user.Password == txtPassword.Password && user.Role == Roles.GUEST2)
                {
                    // ToursOverviewWindow overview = new ToursOverviewWindow(user);
                    // overview.Show();  
                    // overview.GetAttendanceMessage();
                    Views.Guest2.ToursOverviewWindow overview = new Views.Guest2.ToursOverviewWindow(user);
                    overview.Show();
                    overview.GetAttendanceMessages();
                    Close();
                }
                else
                {
                    MessageBox.Show("Wrong password!");
                }
            }
            else
            {
                MessageBox.Show("Wrong username!");
            }
        }

        private int TestInboxCharge(int loggedInUserId)
        {
            int marksNotifications = TestMarkNotifications(loggedInUserId);
            int ownerNotifications = TestOwnerRequestNotifications(loggedInUserId);
            return marksNotifications + ownerNotifications;
        }

        private int TestMarkNotifications(int loggedInUserId)
        {
            AccommodationReservationService resService = new AccommodationReservationService();
            List<AccommodationReservation> allRes = resService.GetAll();
            int counter = 0;
            foreach (var res in allRes)
            {
                int diff = DateTime.Today.DayOfYear - res.LastDay.DayOfYear;
                bool fullCharge = res.UserId == loggedInUserId && !res.ReadMarkNotification && diff <= 5 && diff > 0;
                if (fullCharge)
                {
                    counter++;
                }
            }
            return counter;
        }

        private int TestOwnerRequestNotifications(int loggedInUserId)
        {
            NotificationFromOwnerService notifService = new NotificationFromOwnerService();
            List<NotificationFromOwner> notifications = notifService.GetAll();
            int counter = 0;
            foreach (var item in notifications)
            {
                if (item.GuestId == loggedInUserId)
                {
                    counter++;
                }
            }
            return counter;
        }
    }
}
