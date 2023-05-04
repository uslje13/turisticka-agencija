using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest2
{
    public class PreviousWindowOrPageName
    {
        private static string _previousWindowOrPageName;

        public static void SetPreviousWindowOrPageName(string windowOrPageName)
        {
            _previousWindowOrPageName = windowOrPageName;
        }

        public static string GetPreviousWindowOrPageName()
        {
            return _previousWindowOrPageName;
        }
    }
}
