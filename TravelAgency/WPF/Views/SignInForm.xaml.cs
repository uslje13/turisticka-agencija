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
                    ShowToursWindow showToursWindow = new ShowToursWindow(user);
                    showToursWindow.Show();
                    Close();
                }
                else if (user.Password == txtPassword.Password && user.Role == Roles.GUEST1)
                {
                    UserProfilleWindow userProfilleWindow = new UserProfilleWindow(user, TestInboxCharge());
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

        private bool TestInboxCharge()
        {
            AccommodationReservationService resService = new AccommodationReservationService();
            NotificationFromOwnerService notifService = new NotificationFromOwnerService();

            List<AccommodationReservation> allRes = resService.GetAll();
            List<NotificationFromOwner> notifications = notifService.GetAll();
            if (notifications.Count > 0)
                return true;
            foreach (var res in allRes)
            {
                if (!res.ReadMarkNotification)
                {
                    int diff = DateTime.Today.DayOfYear - res.LastDay.DayOfYear;
                    if (diff <= 5 && diff > 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
