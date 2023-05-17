using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.WPF.ViewModels.TourGuide
{
    public class NumOfTourRequestsPerYearViewModel : ViewModel
    {
        private string _year;

        public string Year
        {
            get => _year;
            set
            {
                if (_year != value)
                {
                    _year = value;
                    OnPropertyChanged("Year");
                }
            }
        }

        private int _numOfRequestsPerYear;

        public int NumOfRequestsPerYear
        {
            get => _numOfRequestsPerYear;
            set
            {
                if (_numOfRequestsPerYear != value)
                {
                    _numOfRequestsPerYear = value;
                    OnPropertyChanged("NumOfRequestsPerYear");
                }
            }
        }

        private ObservableCollection<NumOfTourRequestsPerMonthViewModel> _numOfRequestsPerMonths;

        public ObservableCollection<NumOfTourRequestsPerMonthViewModel> NumOfRequestsPerMonths
        {
            get => _numOfRequestsPerMonths;
            set
            {
                if (_numOfRequestsPerMonths != value)
                {
                    _numOfRequestsPerMonths = value;
                    OnPropertyChanged("NumOfRequestsPerMonths");
                }
            }
        }

        public NumOfTourRequestsPerYearViewModel(string year, int numOfRequests)
        {
            _year = year;
            _numOfRequestsPerYear = numOfRequests;
            _numOfRequestsPerMonths = new ObservableCollection<NumOfTourRequestsPerMonthViewModel>
            {
                new NumOfTourRequestsPerMonthViewModel("January", 1),
                new NumOfTourRequestsPerMonthViewModel("February", 2),
                new NumOfTourRequestsPerMonthViewModel("Mart", 3),
                new NumOfTourRequestsPerMonthViewModel("April", 4),
                new NumOfTourRequestsPerMonthViewModel("May", 5),
                new NumOfTourRequestsPerMonthViewModel("Jun", 6),
                new NumOfTourRequestsPerMonthViewModel("July", 7),
                new NumOfTourRequestsPerMonthViewModel("August", 8),
                new NumOfTourRequestsPerMonthViewModel("September", 9),
                new NumOfTourRequestsPerMonthViewModel("October", 10),
                new NumOfTourRequestsPerMonthViewModel("November", 11),
                new NumOfTourRequestsPerMonthViewModel("December", 12)
            };
        }
    }
}
