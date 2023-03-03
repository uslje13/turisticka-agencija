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
    /// Interaction logic for ShowAndSearchTours.xaml
    /// </summary>
    public partial class ShowAndSearchTours : Window
    {
        public User LogedInUser { get; set; } 
        public ShowAndSearchTours(User user)
        {
            InitializeComponent();
        }
    }
}
