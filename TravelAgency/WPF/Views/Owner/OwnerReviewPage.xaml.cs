﻿using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.ViewModels.Owner;
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

namespace SOSTeam.TravelAgency.WPF.Views.Owner
{
    /// <summary>
    /// Interaction logic for OwnerReviewPage.xaml
    /// </summary>
    public partial class OwnerReviewPage : Page
    {
        public OwnerReviewPage(User user, MainWindowViewModel mainWindowVM)
        {
            DataContext = new OwnerReviewPageViewModel(user, mainWindowVM);
            InitializeComponent();
        }
    }
}
