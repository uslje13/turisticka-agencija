using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TravelAgency.DTO
{
    public class TourDTO
    {
        private string _name;
        private string _city;
        private string _country;
        private int _maxNumOfGuests;
        private DateTime _startDateTime;
        private int _duration;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public TourDTO()
        {
            _name = string.Empty;
            _city = string.Empty;
            _country = string.Empty;
            _maxNumOfGuests = 0;
            _startDateTime = DateTime.MinValue;
            _duration = 0;
        }

        public TourDTO(string name, string city, string country, int maxNumOfGuests, DateTime startDateTime, int duration)
        {
            _name = name;
            _city = city;
            _country = country;
            _maxNumOfGuests = maxNumOfGuests;
            _startDateTime = startDateTime;
            _duration = duration;
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }

        public string City
        {
            get { return _city; }
            set
            {
                if (_city != value)
                {
                    _city = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Country
        {
            get { return _country; }
            set
            {
                if (_country != value)
                {
                    _country = value;
                    OnPropertyChanged();
                }
            }
        }

        public int MaxNumOfGuests
        {
            get { return _maxNumOfGuests; }
            set
            {
                if (_maxNumOfGuests != value)
                {
                    _maxNumOfGuests = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime StartDateTime
        {
            get { return _startDateTime; }
            set
            {
                if (_startDateTime != value)
                {
                    _startDateTime = value;
                    OnPropertyChanged();
                }
            }
        }

        public int Duration
        {
            get { return _duration; }
            set
            {
                if (_duration != value)
                {
                    _duration = value;
                    OnPropertyChanged();
                }
            }
        }

    }
}
