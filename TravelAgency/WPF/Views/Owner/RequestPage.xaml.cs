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
    /// Interaction logic for RequestPage.xaml
    /// </summary>
    public partial class RequestPage : Page
    {
        public RequestPage(User user, MainWindowViewModel mainWindowVM)
        {
            DataContext = new RequestPageViewModel(user,mainWindowVM);
            InitializeComponent();
        }
    }
}
