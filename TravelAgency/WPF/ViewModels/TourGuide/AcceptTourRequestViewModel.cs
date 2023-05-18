using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOSTeam.TravelAgency.Application.Services;

namespace SOSTeam.TravelAgency.WPF.ViewModels.TourGuide
{
    public class AcceptTourRequestViewModel : ViewModel
    {
        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged("Name");
                }
            }
        }
        private string _city;
        public string City
        {
            get => _city;
            set
            {
                if (_city != value)
                {
                    _city = value;
                    OnPropertyChanged("City");
                }
            }
        }
        private string _country;
        public string Country
        {
            get => _country;
            set
            {
                if (_country != value)
                {
                    _country = value;
                    OnPropertyChanged("Country");
                }
            }
        }
        private string _description;
        public string Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged("Description");
                }
            }
        }
        private string _language;
        public string Language
        {
            get => _language;
            set
            {
                if (_language != value)
                {
                    _language = value;
                    OnPropertyChanged("Language");
                }
            }
        }
        private int _maxNumOfGuests;
        public int MaxNumOfGuests
        {
            get => _maxNumOfGuests;
            set
            {
                if (_maxNumOfGuests != value)
                {
                    _maxNumOfGuests = value;
                    OnPropertyChanged("MaxNumOfGuests");
                }
            }
        }
        private int? _duration;
        public int? Duration
        {
            get => _duration;

            set
            {
                if (_duration != value)
                {
                    _duration = value;
                    OnPropertyChanged("Duration");
                }
            }
        }
        
        private bool CanSelectAppointment { get; set; }

        private readonly TourRequestService _tourRequestService;

        public ObservableCollection<string> Cities { get; set; }
        public ObservableCollection<string> Countries { get; set; }

        public ObservableCollection<string> Languages { get; set; }

        public AcceptTourRequestViewModel(int tourRequestId)
        {
            Cities = new ObservableCollection<string>();
            Countries = new ObservableCollection<string>();
            Languages = new ObservableCollection<string>();
            _tourRequestService = new TourRequestService();
            var tourRequest = _tourRequestService.GetById(tourRequestId);
            CanSelectAppointment = false;
            _duration = null;
            _name = string.Empty;
            _city = tourRequest.City;
            Cities.Add(tourRequest.City);
            _country = tourRequest.Country;
            Countries.Add(tourRequest.Country);
            _description = tourRequest.Description;
            _language = tourRequest.Language;
            Languages.Add(tourRequest.Language);
            _maxNumOfGuests = tourRequest.MaxNumOfGuests;
        }

        private void EnableSelectAppointment()
        {

        }

    }
}
