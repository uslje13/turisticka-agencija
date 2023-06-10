using SOSTeam.TravelAgency.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using Syncfusion.PdfViewer;
using Syncfusion.Pdf.Graphics;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest1
{
    public class ShowPDFViewModel
    {
        public NavigationService NavigationService { get; set; }
        public RelayCommand GoBackCommand { get; set; }
        public ShowPDFViewModel(NavigationService service)
        {
            NavigationService = service;

            GoBackCommand = new RelayCommand(Execute_GoBack);
        }

        private void Execute_GoBack(object sender)
        {
            NavigationService.GoBack();
        }
    }
}
