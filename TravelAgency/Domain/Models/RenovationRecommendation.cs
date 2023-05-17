﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using SOSTeam.TravelAgency.Repositories.Serializer;
using System.Windows.Controls;

namespace SOSTeam.TravelAgency.Domain.Models
{

    public class RenovationRecommendation : ISerializable
    {
        public int Id { get; set; }
        public int AccommodationId { get; set; }
        public int GuestId { get; set; }
        public int RenovationRank { get; set; }
        public string Comment { get; set; }
        public RenovationRecommendation() { }

        public RenovationRecommendation(int accommodationId, int guestId, int renovationRank, string comment)
        {
            Id = -1;
            AccommodationId = accommodationId;
            GuestId = guestId;
            RenovationRank = renovationRank;
            Comment = comment;
        }

        public void FromCSV(string[] values)
        {
            var i = 0;
            Id = int.Parse(values[i++]);
            AccommodationId = int.Parse(values[i++]);
            GuestId = int.Parse(values[i++]);
            RenovationRank = int.Parse(values[i++]);
            Comment = values[i++];


        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                AccommodationId.ToString(),
                GuestId.ToString(),
                RenovationRank.ToString(),
                Comment
            };

            return csvValues;
        }

        
    }
}
