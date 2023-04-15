﻿using SOSTeam.TravelAgency.WPF.ViewModels.Guest2;
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
    /// Interaction logic for VouchersWindow.xaml
    /// </summary>
    public partial class VouchersWindow : Window
    {
        public VouchersWindow()
        {
            InitializeComponent();
            VouchersWindowViewModel viewModel = new VouchersWindowViewModel(this);
            DataContext= viewModel;
        }
    }
}
