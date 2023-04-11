using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest2
{
    public class TourViewModel2
    {
        public string Name { get; set; }
        public string Language { get; set; }
        public int Duration { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string CityAndCountry { get; set; }

        public TourViewModel2(string name,string language,int duration,string city,string country) 
        {
            Name = name;
            Language = language;
            Duration = duration;
            Country = country;
            City = city;
            CityAndCountry = city + " (" + country + ")";
        }
    }
}
