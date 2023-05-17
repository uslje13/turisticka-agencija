using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.WPF.ViewModels.TourGuide
{
    public class TourRequestStatsViewModel : ViewModel
    {
        private bool _isLanguageSelected;
        public bool IsLanguageSelected
        {
            get => _isLanguageSelected;
            set
            {
                if (_isLanguageSelected != value)
                {
                    _isLanguageSelected = value;
                    OnPropertyChanged("IsLanguageSelected");
                }
            }
        }

        private bool _isLocationSelected;
        public bool IsLocationSelected
        {
            get => _isLocationSelected;
            set
            {
                if (_isLocationSelected != value)
                {
                    _isLocationSelected = value;
                    OnPropertyChanged("IsLocationSelected");
                }
            }
        }

        private ObservableCollection<NumOfTourRequestsPerYearViewModel> _numOfTourRequestsPerYears;

        public ObservableCollection<NumOfTourRequestsPerYearViewModel> NumOfTourRequestsPerYears
        {
            get => _numOfTourRequestsPerYears;
            set
            {
                if (_numOfTourRequestsPerYears != value)
                {
                    _numOfTourRequestsPerYears = value;
                    OnPropertyChanged("NumOfTourRequestsPerYears");
                }
            }
        }

        public TourRequestStatsViewModel()
        {
            _isLanguageSelected = true;
            _isLocationSelected = false;
            _numOfTourRequestsPerYears = new ObservableCollection<NumOfTourRequestsPerYearViewModel>
            {
                new NumOfTourRequestsPerYearViewModel("2023", 30),
                new NumOfTourRequestsPerYearViewModel("2022", 20),
                new NumOfTourRequestsPerYearViewModel("2021", 10)
            };
        }
    }
}
