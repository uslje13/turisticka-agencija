using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
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
using SOSTeam.TravelAgency.Repositories;
using SOSTeam.TravelAgency.WPF.ViewModels.Guest2;

namespace SOSTeam.TravelAgency.WPF.Views
{
    /// <summary>
    /// Interaction logic for BookTourWindow.xaml
    /// </summary>
    public partial class BookTourWindow : Window
    {
        public BookTourWindow(TourViewModel selected, User loggedInUser)
        {
            InitializeComponent();
            BookTourViewModel2 viewModel = new BookTourViewModel2(selected, loggedInUser,this);
            DataContext = viewModel;
        }

    }
}
