﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Serializer;

namespace TravelAgency.Model
{
    public class Accommodation : ISerializable
    {
        public enum AccommodationType
        {
            APARTMENT, HOUSE, HUT, NOTYPE
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public int LocationId { get; set; }
        public Location Location { get; set; }
        public AccommodationType Type { get; set; }
        public int MaxGuests { get; set; }
        public int MinDaysStay { get; set; }
        public int MinDaysForCancelation { get; set; }
        public string PhotoUrl { get; set; }
        public int OwnerId { get; set; }

        public Accommodation()
        {
            Id = -1;
            Name = string.Empty;
            LocationId = -1;
            Type = AccommodationType.NOTYPE;
            MaxGuests = 0;
            MinDaysStay = 0;
            MinDaysForCancelation = 0;
            PhotoUrl = string.Empty;
            OwnerId = -1;
        }
        public Accommodation(string name, AccommodationType type, int locationId, int maxGuests, int minDaysStay, string photoUrl, int ownerId, int minDaysForCancelation = 1)
        {
            Id = -1;
            Name = name;
            LocationId = locationId;
            Type = type;
            MaxGuests = maxGuests;
            MinDaysStay = minDaysStay;
            MinDaysForCancelation = minDaysForCancelation;
            PhotoUrl = photoUrl;
            OwnerId = ownerId; 
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Name, LocationId.ToString(), ((int)Type).ToString(), MaxGuests.ToString(), MinDaysStay.ToString(), MinDaysForCancelation.ToString(), PhotoUrl, OwnerId.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            int i = 0;
            Id = Convert.ToInt32(values[i++]);
            Name = values[i++];
            LocationId = Convert.ToInt32(values[i++]);
            Type = (AccommodationType)Convert.ToInt32(values[i++]);
            MaxGuests = Convert.ToInt32(values[i++]);
            MinDaysStay = Convert.ToInt32(values[i++]);
            MinDaysForCancelation = Convert.ToInt32(values[i++]);
            PhotoUrl = values[i++];
            OwnerId = Convert.ToInt32(values[i++]);
        }
    }
}
