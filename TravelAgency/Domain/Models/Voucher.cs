﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOSTeam.TravelAgency.Repositories.Serializer;

namespace SOSTeam.TravelAgency.Domain.Models
{
    public class Voucher : ISerializable
    {
        public int Id { get; set; }
        public bool Used { get; set; }
        public DateOnly ExpiryDate { get; set; }
        public int UserId { get; set; }

        public Voucher()
        {
            Id = -1;
            Used = false;
            ExpiryDate = DateOnly.MinValue;
            UserId = -1;
        }

        public Voucher(bool used, DateOnly expiryDate, int userId)
        {
            Used = used;
            ExpiryDate = expiryDate;
            UserId = userId;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            Used = bool.Parse(values[1]);
            ExpiryDate = DateOnly.Parse(values[2]);
            UserId = int.Parse(values[3]);
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                Used.ToString(),
                ExpiryDate.ToString(),
                UserId.ToString()
            };
            return csvValues;
        }
    }
}
