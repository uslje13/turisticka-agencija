using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Xml.Linq;
using SOSTeam.TravelAgency.Domain.Models;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest1
{
    public class MarkAccommodationViewModel
    {
        public int CleanMark { get; set; }
        public int OwnerMark { get; set; }
        public string GuestComment { get; set; }
        public string GuestImagesUrls { get; set; }
        public RelayCommand MarkAccCommand { get; set; }
        public User LoggedInUser { get; set; }
        public CancelAndMarkResViewModel Accommodation { get; set; }

        public MarkAccommodationViewModel(int clean, int owner, string comment, string urls, User user, CancelAndMarkResViewModel acc) 
        {
            CleanMark = clean;
            OwnerMark = owner;
            GuestComment = comment;
            GuestImagesUrls = urls;
            LoggedInUser = user;
            Accommodation = acc;

            MarkAccCommand = new RelayCommand(ExecuteAccommodationMarking);
        }

        private void ExecuteAccommodationMarking(object sender)
        {
            GuestAccMarkService service = new GuestAccMarkService();
            service.MarkAccommodation(CleanMark, OwnerMark, GuestComment, GuestImagesUrls, LoggedInUser, Accommodation);
        }
    }
}
