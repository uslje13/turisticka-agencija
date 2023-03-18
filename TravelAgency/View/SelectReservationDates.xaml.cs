using System;
using System.Collections.Generic;
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
    /// Interaction logic for SelectReservationDates.xaml
    /// </summary>
    public partial class SelectReservationDates : Window
    {
        public List<AccReservationDTO> suggestCatalog { get; set; }
        public AccReservationDTO selectedCatalogItem { get; set; }
        public User LoggedInUser { get; set; }

        public SelectReservationDates()
        {
            InitializeComponent();
        }

        public SelectReservationDates(List<AccReservationDTO> list, User user)
        {
            InitializeComponent();
            DataContext = this;
            suggestCatalog = list;
            LoggedInUser = user;
        }

        private void ContinueReservation(object sender, RoutedEventArgs e)
        {
            if(selectedCatalogItem == null)
            {
                MessageBox.Show("Odaberite smeštaj koji želite rezervisati.");
            } 
            else
            {
                EnterGuestNumber newWindow = new EnterGuestNumber(selectedCatalogItem, LoggedInUser);
                newWindow.Show();
                Close();
            }
        }
    }
}
