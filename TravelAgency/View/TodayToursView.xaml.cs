﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using TravelAgency.Model;

namespace TravelAgency.View
{
    /// <summary>
    /// Interaction logic for TodayTourView.xaml
    /// </summary>
    public partial class TodayTourView : Window
    {
        public TodayTourView(ObservableCollection<Tour> tours)
        {
            InitializeComponent();
            DataContext = this;
        }
    }
}
