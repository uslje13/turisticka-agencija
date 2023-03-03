using System.Windows;
using TravelAgency.View;

namespace TravelAgency
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            ToursOverview tourOverview = new ToursOverview();
            tourOverview.Show();
        }
    }
}
