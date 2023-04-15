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
using SOSTeam.TravelAgency.WPF.ViewModels;
using SOSTeam.TravelAgency.WPF.ViewModels.Guest1;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Application.Services;

namespace SOSTeam.TravelAgency.WPF.Views.Guest1
{
    /// <summary>
    /// Interaction logic for UserProfille.xaml
    /// </summary>
    public partial class UserProfilleWindow : Window
    {
        public UserProfilleWindow(User user, bool color)
        {
            InitializeComponent();
            if (color)
                InboxButton.Background = new SolidColorBrush(Colors.OrangeRed);
            UserProfilleViewModel viewModel = new UserProfilleViewModel(user, UsersName);
            DataContext = viewModel;
        }
    }
}