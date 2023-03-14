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

namespace TravelAgency.View
{
    /// <summary>
    /// Interaction logic for SearchResults.xaml
    /// </summary>
    public partial class SearchResults : Window
    {
        public ObservableCollection<AccommodationDTO> accommodationDTOs { get; set; }
        public SearchResults()
        {
            InitializeComponent();
        }

        public SearchResults(List<AccommodationDTO> Results)
        {
            InitializeComponent();
            DataContext = this;
            accommodationDTOs = new ObservableCollection<AccommodationDTO>(Results);
        }
    }
}
