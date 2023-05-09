using SOSTeam.TravelAgency.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest1
{
    public class ReportResultsViewModel
    {
        public string TextBlockText { get; set; }
        public Frame ThisFrame { get; set; }
        public List<CancelAndMarkResViewModel> _presentedReservations { get; set; }
        public RelayCommand GoBackCommand { get; set; }

        public ReportResultsViewModel(List<CancelAndMarkResViewModel> list, Frame frame, int type, DateTime fDate, DateTime lDate)
        {
            _presentedReservations = list;
            ThisFrame = frame;
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
            var navigationService = ThisFrame.NavigationService;
            navigationService.GoBack();
        }
    }
}
