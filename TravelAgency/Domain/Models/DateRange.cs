using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SOSTeam.TravelAgency.Domain.Models
{
    public class DateRange
    {
        public DateTime? Start;
        public DateTime? End;

        public DateRange(DateTime start, DateTime end)
        {
            if (start <= end)
            {
                Start = start;
                End = end;
            }
            else
            {
                Start = null;
                End = null;
            }
        }

        public bool IsOutOfRange(DateRange other)
        {
            if (Start == null || End == null)
            {
                return true;
            }
            if ((other.Start < Start && other.End < Start) || (other.Start > End && other.End > End))
            {
                return true;
            }

            return false;
        }
    }
}
