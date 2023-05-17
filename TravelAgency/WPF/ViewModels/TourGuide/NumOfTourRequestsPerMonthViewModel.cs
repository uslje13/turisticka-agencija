using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.WPF.ViewModels.TourGuide
{
    public class NumOfTourRequestsPerMonthViewModel : ViewModel
    {
        private string _month;

        public string Month
        {
            get => _month;
            set
            {
                if (_month != value)
                {
                    _month = value;
                    OnPropertyChanged("Month");
                }
            }
        }

        private int _numOfRequestsPerMonth;

        public int NumOfRequestsPerMonth
        {
            get => _numOfRequestsPerMonth;
            set
            {
                if (_numOfRequestsPerMonth != value)
                {
                    _numOfRequestsPerMonth = value;
                    OnPropertyChanged("NumOfRequestsPerMonth");
                }
            }
        }

        public NumOfTourRequestsPerMonthViewModel(string month, int numOfRequests)
        {
            _month = month;
            _numOfRequestsPerMonth = numOfRequests;
        }
    }
}
