using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOSTeam.TravelAgency.Domain.Models;

namespace TravelAgency.DTO
{
    public class AccommodationDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public Accommodation.AccommodationType Type { get; set; }
        public int MaxGuests { get; set; }
        public int MinDaysStay { get; set; }
        public int MinDaysForCancelation { get; set; }

        public AccommodationDTO()
        {
            Id = -1;
            Name = string.Empty;
            Location = string.Empty;
            Type = Accommodation.AccommodationType.NOTYPE;
            MaxGuests = 0;
            MinDaysStay = 0;
            MinDaysForCancelation = 0;
        }
        public AccommodationDTO(int id, string name, string location, Accommodation.AccommodationType type, int maxGuests, int minDaysStay, int minDaysForCancelation)
        {
            Id = id;
            Name = name;
            Location = location;
            Type = type;
            MaxGuests = maxGuests;
            MinDaysStay = minDaysStay;
            MinDaysForCancelation = minDaysForCancelation;
        }
    }
}
