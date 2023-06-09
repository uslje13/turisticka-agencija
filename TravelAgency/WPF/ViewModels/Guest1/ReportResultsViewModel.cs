using PdfSharp.Drawing;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Drawing.Text;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Drawing;
using System.IO;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest1
{
    public class ReportResultsViewModel
    {
        public string TextBlockText { get; set; }
        public NavigationService NavigationService { get; set; }
        public List<CancelAndMarkResDTO> _presentedReservations { get; set; }
        public RelayCommand GoBackCommand { get; set; }
        public RelayCommand GetPDFCommand { get; set; }

        public ReportResultsViewModel(List<CancelAndMarkResDTO> list, NavigationService service, int type, DateTime fDate, DateTime lDate)
        {
            _presentedReservations = list;
            NavigationService = service;
            TextBlockText = "";

            FillTextBlock(type, fDate, lDate);

            GoBackCommand = new RelayCommand(Execute_GoBack);
            GetPDFCommand = new RelayCommand(Execute_GetPDF);
        }

        private void Execute_GetPDF(object sender)
        {
            using (PdfDocument document = new PdfDocument())
            {
                PdfPage page = document.Pages.Add();
                PdfGraphics graphics = page.Graphics;

                PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 12);

                // Podesite koordinate x i y za početak crtanja teksta
                float x = 40f;
                float y = 40f;

                // Iterirajte kroz vaše podatke i crtajte tekst na svakoj liniji
                foreach (var data in _presentedReservations)
                {
                    graphics.DrawString(TextBlockText, font, PdfBrushes.Black, new PointF(x, y));

                    // Povećajte y koordinatu za sljedeću liniju teksta
                    y += 20f;
                }

                FileStream fileStream = new FileStream("C:\\Users\\korisnik\\Desktop\\SIMS\\turisticka-agencija\\izvjestaj.pdf", FileMode.Create);
                document.Save(fileStream);
            }
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
