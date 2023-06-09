using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest1
{
    public class ReportResultsViewModel
    {
        public string TextBlockText { get; set; }
        public NavigationService NavigationService { get; set; }
        public List<CancelAndMarkResDTO> _presentedReservations { get; set; }
        public RelayCommand GoBackCommand { get; set; }

        public ReportResultsViewModel(List<CancelAndMarkResDTO> list, NavigationService service, int type, DateTime fDate, DateTime lDate)
        {
            _presentedReservations = list;
            NavigationService = service;
            TextBlockText = "";

            FillTextBlock(type, fDate, lDate);

            GoBackCommand = new RelayCommand(Execute_GoBack);
        }

        private void FillTextBlock(int type, DateTime fDate, DateTime lDate)
        {
            if(type == 0)
            {
                TextBlockText = "Prikaz zakaznih rezervacija u periodu od " + fDate.ToShortDateString() + " do " + lDate.ToShortDateString();
            }
            else
            {
                TextBlockText = "Prikaz otkazanih rezervacija u periodu od " + fDate.ToShortDateString() + " do " + lDate.ToShortDateString();
            }
        }

        public void Execute_GoBack(object sender)
        {
            NavigationService.GoBack();
        }
    }
}
