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
    /// Interaction logic for EnterReservation.xaml
    /// </summary>
    public partial class EnterReservation : Window
    {
        public DateOnly StartReservation { get; set; }
        public DateOnly EndReservation { get; set; }    

        public EnterReservation()
        {
            InitializeComponent();
        }

        public EnterReservation(AccommodationDTO dto)
        {
            InitializeComponent();
        }
    }
}
