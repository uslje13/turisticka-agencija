﻿using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.ViewModels.Guest2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SOSTeam.TravelAgency.WPF.Views.Guest2
{
    /// <summary>
    /// Interaction logic for MyReservationsPage.xaml
    /// </summary>
    public partial class MyReservationsPage : Page
    {
        public MyReservationsPage(User loggedInUser)
        {
            InitializeComponent();
            MyReservationsPageViewModel viewModel= new MyReservationsPageViewModel(loggedInUser);
            DataContext= viewModel;
        }
    }
}
