using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest2
{
    public class AppoitmentOverviewViewModel : ViewModel
    {
        public DateOnly Date { get; set; }
        public TimeOnly Time { get; set; }
        public int TourId { get; set; }

        private string _start;

        public string Start
        {
            get { return _start; }
            set
            {
                if (value != _start)
                {
                    _start = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _availableSlots;

        public int AvailableSlots
        {
            get { return _availableSlots; }
            set
            {
                if (value != _availableSlots)
                {
                    _availableSlots = value;
                    OnPropertyChanged();
                }
            }
        }

        public AppoitmentOverviewViewModel(DateOnly date, TimeOnly time, int availableSlots, int tourId)
        {
            AvailableSlots = availableSlots;
            Start = date.ToString() + " " + time.ToString();
            TourId = tourId;
            Date = date;
            Time = time;
        }
    }
}
