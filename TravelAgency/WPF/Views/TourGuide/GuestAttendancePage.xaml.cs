﻿using SOSTeam.TravelAgency.WPF.ViewModels.TourGuide;
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

namespace SOSTeam.TravelAgency.WPF.Views.TourGuide
{
    /// <summary>
    /// Interaction logic for GuestAttendancePage.xaml
    /// </summary>
    public partial class GuestAttendancePage : Page
    {
        public GuestAttendancePage(CheckpointCardViewModel selectedCheckpointActivity, string tourName, DateTime? date)
        {
            InitializeComponent();
            DataContext = new GuestAttendancesViewModel(selectedCheckpointActivity, tourName, date);
        }
    }
}
