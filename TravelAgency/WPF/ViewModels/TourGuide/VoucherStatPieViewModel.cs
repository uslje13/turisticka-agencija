using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.WPF.ViewModels.TourGuide
{
    public class VoucherStatPieViewModel
    {
        public string Type { get; set; }
        public int NumOfGuests { get; set; }

        public VoucherStatPieViewModel()
        {
            NumOfGuests = 0;
        }
    }
}
