﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Serializer;

namespace TravelAgency.Model
{
    public class Notification : ISerializable
    {
        public enum NotificationType
        {
            GUESTREVIEW, NOTYPE
        }
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Text { get; set; }
        public NotificationType Type { get; set; }
        public bool Read { get; set; }

        public Notification(int userId, string text, NotificationType type, bool read)
        {
            Id = -1;
            UserId = userId;
            Text = text;
            Type = type;
            Read = read;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), UserId.ToString(), Text, ((int)Type).ToString(), Read == true ? "1" : "0" };
            return csvValues;

        }

        public void FromCSV(string[] values)
        {
            int i = 0;
            Id = Convert.ToInt32(values[i++]);
            UserId = Convert.ToInt32(values[i++]);
            Text = values[i++];
            Type = (NotificationType)Convert.ToInt32(values[i++]);
            Read = values[i++].Equals("1");
        }

    }
}
