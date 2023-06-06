using SOSTeam.TravelAgency.Repositories.Serializer;
using SOSTeam.TravelAgency.WPF.ViewModels.TourGuide;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.Domain.Models
{
    public class ComplexTourRequest : ISerializable
    {
        public int Id { get; set; }
        public StatusType Status { get; set; }

        public ComplexTourRequest() {
            Status = StatusType.ON_HOLD;
        }

        public ComplexTourRequest(StatusType status)
        {
            Status= status;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            Status = values[1] switch
            {
                "ON_HOLD" => StatusType.ON_HOLD,
                "INVALID" => StatusType.INVALID,
                "ACCEPTED" => StatusType.ACCEPTED,
            };

        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                Status.ToString(),
            };
            return csvValues;
        }
    }
}
