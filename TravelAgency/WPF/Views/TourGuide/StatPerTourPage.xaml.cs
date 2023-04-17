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
using System.Windows.Navigation;
using System.Windows.Shapes;
using LiveCharts;
using SOSTeam.TravelAgency.WPF.ViewModels.TourGuide;

namespace SOSTeam.TravelAgency.WPF.Views.TourGuide
{
    /// <summary>
    /// Interaction logic for StatPerTourPage.xaml
    /// </summary>
    public partial class StatPerTourPage : Page
    {
        public StatPerTourPage(TourCardViewModel selectedTourCard)
        {
            InitializeComponent();
            DataContext = new StatByTourViewModel(selectedTourCard);
        }
    }
}
