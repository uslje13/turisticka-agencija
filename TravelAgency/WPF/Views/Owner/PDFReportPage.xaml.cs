using SOSTeam.TravelAgency.WPF.ViewModels.Owner;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SOSTeam.TravelAgency.WPF.Views.Owner
{
    /// <summary>
    /// Interaction logic for PDFReportPage.xaml
    /// </summary>
    public partial class PDFReportPage : Page
    {
        public PDFReportPage(string pdfPath)
        {
            InitializeComponent();
            DataContext = this;

            FileStream stream = new FileStream(pdfPath, FileMode.Open);
            PdfViewer.Load(stream);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            App.OwnerNavigationService.NavigateMainWindow("Accommodation");
        }
    }
}
