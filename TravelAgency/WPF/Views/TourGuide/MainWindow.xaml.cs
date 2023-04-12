﻿using System;
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
using SOSTeam.TravelAgency.Domain.Models;

namespace SOSTeam.TravelAgency.WPF.Views.TourGuide
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(User loggedUser)
        {
            InitializeComponent();
            //ToursOverview.Content = new HomePage(loggedUser);
            ToursOverview.Content = new AddCheckpointsPage();
        }

        private void CreateButton_OnClick(object sender, RoutedEventArgs e)
        {
            //TourGuide.CreateTourPage.Content = new CreateTourPage();
            
        }
    }
}
