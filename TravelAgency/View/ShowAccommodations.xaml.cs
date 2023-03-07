using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using TravelAgency.Repository;

namespace TravelAgency.View
{
    /// <summary>
    /// Interaction logic for ShowAccommodation.xaml
    /// </summary>
    public partial class ShowAccommodations : Window
    {
        public User LoggedInUser { get; set; }
        private AccommodationRepository _repository;
        public static ObservableCollection<Accommodation> Accommodations { get; set; }
        public Accommodation SelectedAccommodation { get; set; }
        
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public ShowAccommodations(User user)
        {
            DataContext = this;

            LoggedInUser = user;
            _repository = new AccommodationRepository();
            Accommodations = new ObservableCollection<Accommodation>();
            foreach(Accommodation item in _repository.GetAll()) 
            {
                Accommodations.Add(item);
            }

            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
