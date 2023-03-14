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

namespace TravelAgency.View
{
    /// <summary>
    /// Interaction logic for SearchWindow.xaml
    /// </summary>
    public partial class SearchWindow : Window
    {
        private string[] EnabledAccommodationTypes = { "APARTMAN", "KUĆA", "KOLIBA" };
        public string searchedAccName;
        public string searchedAccCity;
        public string searchedAccCountry;
        public int searchedAccGuestsNumber;
        public int searchedAccDaysNumber;

        public SearchWindow()
        {
            DataContext = this;
            //comboBox.ItemsSource = EnabledAccommodationTypes;
            InitializeComponent();
        }

        private void SearchAccommodationClick(object sender, RoutedEventArgs e)
        {
            string lowerName = name.Text.ToLower();
            string lowerCity = city.Text.ToLower();
            string lowerCountry = country.Text.ToLower();
            string accommodationTypes;
            //int parsedGuestsNumber = int.Parse(guestsNumber);
            //int parsedDaysNumber = int.Parse(daysNumber);
        }

        private void CancelAccommodationClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
