﻿using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.Views.Guest2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest2
{
    public class TourViewModel2
    {
        public User LoggedInUser { get; set; }

        public int TourId;
        public string Name { get; set; }
        public string Language { get; set; }
        public int Duration { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string LocationFullName { get; set; }

        private RelayCommand _tourDetailsCommand;

        public RelayCommand TourDetailsCommand
        {
            get { return _tourDetailsCommand; }
            set
            {
                _tourDetailsCommand = value;
            }
        }

        public TourViewModel2(int id,string name,string language,int duration,string city,string country,User loggedInUser) 
        {
            TourId = id;
            Name = name;
            Language = language;
            Duration = duration;
            Country = country;
            City = city;
            LocationFullName = city + " (" + country + ")";
            LoggedInUser = loggedInUser;
            TourDetailsCommand = new RelayCommand(Execute_OpenBookTourWindow, CanExecuteMethod);
        }

        private void Execute_OpenBookTourWindow(object obj)
        {
            BookTourWindow window = new BookTourWindow(TourId,LoggedInUser);
            window.ShowDialog();
        }

        private bool CanExecuteMethod(object parameter)
        {
            return true;
        }
    }
}
