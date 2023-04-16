using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.DirectoryServices;
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
using TravelAgency.DTO;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Repositories;
using SOSTeam.TravelAgency.WPF.ViewModels.Guest1;
using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.WPF.ViewModels;

namespace SOSTeam.TravelAgency.WPF.Views
{
    /// <summary>
    /// Interaction logic for SearchWindow.xaml
    /// </summary>
    public partial class SearchWindow : Window
    {
        public SearchWindow(User user)
        {
            InitializeComponent();
            List<TextBox> textBoxes = new List<TextBox> { name, city, country, guestsNumber, daysNumber };
            List<ComboBoxItem> comboBoxItems = new List<ComboBoxItem> { CBItemApart, CBItemHouse, CBItemHut, CBItemNoType };
            SearchViewModel viewModel = new SearchViewModel(user, textBoxes, comboBoxItems, CBTypes, this);
            DataContext = viewModel;
        }
    }
}