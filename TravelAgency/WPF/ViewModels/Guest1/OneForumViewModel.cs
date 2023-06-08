using SOSTeam.TravelAgency.Domain.DTO;
using SOSTeam.TravelAgency.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest1
{
    public class OneForumViewModel
    {
        public string LocationName { get; set; }
        public string Description { get; set; }
        public string Usebility { get; set; }

        public OneForumViewModel(User user, NavigationService service, Forum forum)
        {
            LocationName = forum.LocationName;
            Usebility = forum.Useful;
            Description = forum.Description;
        }
    }
}
