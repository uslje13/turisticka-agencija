﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TravelAgency.Model.Accommodation;
using System.Xml.Linq;
using TravelAgency.Serializer;
using System.Windows.Controls;

namespace TravelAgency.Model
{
    public class AccommodationReservation : ISerializable
    {
        public int Id { get; set; }
        public DateTime FirstDay { get; set; }
        public DateTime LastDay { get; set; }
        public int ReservationDuration { get; set; }
        public int AccommodationId { get; set; }

        public AccommodationReservation() 
        {
            Id = -1;
            FirstDay = new DateTime();
            LastDay = new DateTime();
            ReservationDuration = -1;
            AccommodationId = -1;
        }

        public AccommodationReservation(int id, DateTime firstDay, DateTime lastDay, int reservationDuration, int accommodationId)
        {
            Id = id;
            FirstDay = firstDay;
            LastDay = lastDay;
            ReservationDuration = reservationDuration;
            AccommodationId = accommodationId;
        }

        public AccommodationReservation(DateTime firstDay, DateTime lastDay, int reservationDuration, int accommodationId)
        {
            FirstDay = firstDay;
            LastDay = lastDay;
            ReservationDuration = reservationDuration;
            AccommodationId = accommodationId;
        }
        
        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), FirstDay.ToString(), LastDay.ToString(), ReservationDuration.ToString(), AccommodationId.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            int i = 0;
            Id = Convert.ToInt32(values[i++]);
            FirstDay = Convert.ToDateTime(values[i++]);
            LastDay = Convert.ToDateTime(values[i++]);
            ReservationDuration = Convert.ToInt32(values[i++]);
            AccommodationId = Convert.ToInt32(values[i++]);
        }
    }
}