using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOSTeam.TravelAgency.Repositories;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest2
{
    public class TourViewModel : ViewModel
    {
        public int TourId { get; set; }
        public int AppointmentId { get; set; }
        public string Name { get; set; }
        public string Language { get; set; }
        public int MaxNumOfGuests { get; set; }
        public int Duration { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string CityAndCountry { get; set; }
        public int Ocupancy { get; set; }
        public TimeOnly Time { get; set; }
        public DateOnly Date { get; set; }

        public TourViewModel()
        {
        }
    
        public TourViewModel(string name, string language, int maxNumOfGuests, int duration, int ocupancy, string country, string city, int tourId, TimeOnly time, DateOnly date)
        {
            Name = name;
            Language = language;
            MaxNumOfGuests = maxNumOfGuests;
            Duration = duration;
            Country = country;
            City = city;
            CityAndCountry = city + " (" + country + ")";
            Ocupancy = ocupancy;
            TourId = tourId;
            Time = time;
            Date = date;
        }

        public TourViewModel(int tourId, string name, string language, string locationFullName, int maxNumOfGuests, int duration)
        {
            TourId = tourId;
            Name = name;
            Language = language;
            CityAndCountry = locationFullName;
            MaxNumOfGuests = maxNumOfGuests;
            Duration = duration;
        }
    
    }
}
