using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest1
{
    public class AnswerToGuestViewModel
    {
        public TextBlock UsersName { get; set; }
        public TextBlock firstOldDay { get; set; }
        public TextBlock lastOldDay { get; set; }
        public TextBlock firstNewDay { get; set; }
        public TextBlock lastNewDay { get; set; }
        public TextBlock accommodationName { get; set; }
        public string givenOwnerComment { get; set; }
        public WantedNewDate newReservation { get; set; }
        public ChangedReservationRequest oldReservation { get; set; }

        public AnswerToGuestViewModel(WantedNewDate dto, ChangedReservationRequest request) 
        {
            oldReservation = request;
            newReservation = dto;
            UsersName = new TextBlock();
            firstOldDay = new TextBlock();
            lastOldDay = new TextBlock();
            firstNewDay = new TextBlock();
            lastNewDay = new TextBlock();
            accommodationName = new TextBlock();
            givenOwnerComment = " ";

            FillTextBlocks();
        }

        private void FillTextBlocks()
        {
            FillUsername();
            FillFirstOldDay();
            FillLastOldDay();
            FillFirstNewDay();
            FillLastNewDay();
            FillAccommodationName();
        }

        private void FillUsername()
        {
            Binding binding = new Binding();
            UserService userService = new UserService();
            User user = userService.GetById(oldReservation.UserId);
            binding.Source = user.Username;
            UsersName.SetBinding(TextBlock.TextProperty, binding);
        }

        private void FillFirstOldDay()
        {
            Binding binding = new Binding();
            binding.Source = oldReservation.FirstDay.ToString();
            firstOldDay.SetBinding(TextBlock.TextProperty, binding);
        }

        private void FillLastOldDay()
        {
            Binding binding = new Binding();
            binding.Source = oldReservation.LastDay.ToString();
            lastOldDay.SetBinding(TextBlock.TextProperty, binding);
        }

        private void FillFirstNewDay()
        {
            Binding binding = new Binding();
            binding.Source = newReservation.wantedDate.ReservationFirstDay.ToString();
            firstNewDay.SetBinding(TextBlock.TextProperty, binding);
        }

        private void FillLastNewDay()
        {
            Binding binding = new Binding();
            binding.Source = newReservation.wantedDate.ReservationLastDay.ToString();
            lastNewDay.SetBinding(TextBlock.TextProperty, binding);
        }

        private void FillAccommodationName()
        {
            Binding binding = new Binding();
            binding.Source = newReservation.wantedDate.AccommodationName;
            accommodationName.SetBinding(TextBlock.TextProperty, binding);
        }
    }
}
