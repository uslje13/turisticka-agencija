﻿using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.ViewModels.Guest1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for SearchPage.xaml
    /// </summary>
    public partial class SearchPage : Page
    {
        public SearchPage(User user, Frame frame)
        {
            InitializeComponent();
            List<TextBox> textBoxes = new List<TextBox> { name, city, country, guestsNumber, daysNumber };
            List<ComboBoxItem> comboBoxItems = new List<ComboBoxItem> { CBItemApart, CBItemHouse, CBItemHut, CBItemNoType };
            SearchViewModel viewModel = new SearchViewModel(user, textBoxes, comboBoxItems, CBTypes, frame);
            DataContext = viewModel;
        }

        private void TestEnteredText(object sender, RoutedEventArgs e)
        {
            if(!name.Text.Equals(""))
            {
                if (!Regex.IsMatch(name.Text, @"^[A-Za-z]*$"))
                {
                    MessageBox.Show("Naziv smještaja može sadržati samo velika i mala slova.", " ", MessageBoxButton.OK, MessageBoxImage.Error);
                    name.Focus();
                    return;
                }
            }

            if (guestsNumber.Text.Equals(""))
            {
                guestsNumber.Text = "0";
            }
            else if (!Regex.IsMatch(guestsNumber.Text, @"^[0-9]+$"))
            {
                MessageBox.Show("Broj gostiju se mora sastojati od cifara!", " ", MessageBoxButton.OK, MessageBoxImage.Error);
                guestsNumber.Focus();
                return;
            }

            if (daysNumber.Text.Equals(""))
            {
                daysNumber.Text = "100";
            }
            else if (!Regex.IsMatch(daysNumber.Text, @"^[0-9]+$"))
            {
                MessageBox.Show("Broj dana se mora sastojati od cifara!", " ", MessageBoxButton.OK, MessageBoxImage.Error);
                daysNumber.Focus();
                return;
            }
        }
    }
}
