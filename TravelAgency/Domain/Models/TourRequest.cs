using SOSTeam.TravelAgency.Repositories.Serializer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace SOSTeam.TravelAgency.Domain.Models
{
    public enum StatusType { ON_HOLD = 0, INVALID = 1, ACCEPTED = 2 }
    public class TourRequest : ISerializable
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public bool IsNotificationViewed { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Description { get; set; }
        public string Language { get; set; }
        public int MaxNumOfGuests { get; set; }
        public DateOnly MaintenanceStartDate { get; set; }
        public DateOnly MaintenanceEndDate { get; set; }
        public StatusType Status { get; set; }
        public TourRequest()
        {
            Id = -1;
            City = string.Empty;
            Country = string.Empty;
            Description = string.Empty;
            Language = string.Empty;
            MaxNumOfGuests = 0;
            MaintenanceStartDate = DateOnly.MinValue;
            MaintenanceEndDate = DateOnly.MaxValue;
            Status = StatusType.ON_HOLD;
        }

        public TourRequest(string city, string country, string description, string language, int maxNumOfGuests, DateOnly maintenanceStartDate, DateOnly maintenanceEndDate, StatusType statusType, int userId, bool isNotificationViewed = false)
        {
            City = city;
            Country = country;
            Description = description;
            Language = language;
            MaxNumOfGuests = maxNumOfGuests;
            MaintenanceStartDate = maintenanceStartDate;
            MaintenanceEndDate = maintenanceEndDate;
            Status = statusType;
            UserId = userId;
            IsNotificationViewed = isNotificationViewed;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            City = values[1];
            Country = values[2];
            Description = values[3];
            Language = values[4];
            MaxNumOfGuests = int.Parse(values[5]);
            MaintenanceStartDate = DateOnly.ParseExact(values[6], "dd.MM.yyyy.", CultureInfo.InvariantCulture);
            MaintenanceEndDate = DateOnly.ParseExact(values[7], "dd.MM.yyyy.", CultureInfo.InvariantCulture);
            Status = values[8] switch
            {
                "ON_HOLD" => StatusType.ON_HOLD,
                "INVALID" => StatusType.INVALID,
                "ACCEPTED" => StatusType.ACCEPTED,
            };
            UserId = int.Parse(values[9]);
            IsNotificationViewed = bool.Parse(values[10]);
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                City,
                Country,
                Description,
                Language,
                MaxNumOfGuests.ToString(),
                MaintenanceStartDate.ToString("dd.MM.yyyy."),
                MaintenanceEndDate.ToString("dd.MM.yyyy."),
                Status.ToString(),
                UserId.ToString(),
                IsNotificationViewed.ToString(),
            };
            return csvValues;
        }
    }
}
