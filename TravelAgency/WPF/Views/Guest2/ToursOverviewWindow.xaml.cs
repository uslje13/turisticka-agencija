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
using System.Windows.Shapes;

namespace SOSTeam.TravelAgency.WPF.Views.Guest2
{
    /// <summary>
    /// Interaction logic for ToursOverviewWindow.xaml
    /// </summary>
    public partial class ToursOverviewWindow : Window
    {
        private readonly MainViewModel _viewModel;
        public ToursOverviewWindow(User loggedInUser)
        {
            InitializeComponent();
            MainViewModel viewModel = new MainViewModel(loggedInUser,this);
            _viewModel = viewModel; 
            this.DataContext = viewModel;
        }

        public void GetAttendanceMessages()
        {
            _viewModel.GetAttendanceMessage();
        }
    }
}
