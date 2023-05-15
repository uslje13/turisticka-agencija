﻿using SOSTeam.TravelAgency.WPF.ViewModels.Guest1;
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

namespace SOSTeam.TravelAgency.WPF.Views.Guest1
{
    /// <summary>
    /// Interaction logic for ReportResultsPage.xaml
    /// </summary>
    public partial class ReportResultsPage : Page
    {
        public ReportResultsPage(List<CancelAndMarkResViewModel> list, Frame frame, int type, DateTime fDate, DateTime lDate)
        {
            InitializeComponent();
            ReportResultsViewModel viewModel = new ReportResultsViewModel(list, frame, type, fDate, lDate);
            DataContext = viewModel;    
        }
    }
}