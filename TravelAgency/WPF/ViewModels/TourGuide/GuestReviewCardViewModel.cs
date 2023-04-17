using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.WPF.ViewModels.TourGuide
{
    public class GuestReviewCardViewModel
    {
        public int ReviewId { get; set; }
        public int AppointmentId { get; set; }
        public int UserId { get; set; }
        public string GuestName { get; set; }
        public string TourName { get; set; }
        public string AvgGrade { get; set; }
        public double KnowledgeGrade { get; set; }
        public int LanguageGrade { get; set; }
        public int InterestingGrade { get; set; }
        public DateOnly Date { get; set; }
        public string Comment { get; set; }
        public string ReportedImage { get; set; }

        public GuestReviewCardViewModel()
        {
            GuestName = string.Empty;
            TourName = string.Empty;
            AvgGrade = string.Empty;
            ReportedImage = string.Empty;
        }
    }
}
