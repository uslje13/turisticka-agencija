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
using static System.Net.Mime.MediaTypeNames;

namespace TravelAgency.View
{
    /// <summary>
    /// Interaction logic for ShowAndSearchTours.xaml
    /// </summary>
    public partial class ShowAndSearchTours : Window
    {
        public User LoggedInUser { get; set; }
        public static ObservableCollection<Tour> Tours { get; set; }
        public Tour SelectedTour { get; set; }

        private readonly TourRepository _repository;
        private readonly LocationRepository _locationRepository;
        public ShowAndSearchTours(User user)
        {
            InitializeComponent();
            DataContext = this;
            LoggedInUser = user;
            _repository= new TourRepository();
            Tours = new ObservableCollection<Tour>(_repository.GetAll());

        }

        private void BookButtonClick(object sender, RoutedEventArgs e)
        {
            if(SelectedTour == null)
            {
                MessageBox.Show("Izaberi turu za rezervaciju");
            }
            else
            {
               if(SelectedTour.NumOfGuests < SelectedTour.MaxNumOfGuests)
               {
                    BookTour bookTourWindow = new BookTour(SelectedTour);
                    bookTourWindow.Show();
               }
               else
               {
                    MessageBox.Show("Nema slobodnih mesta za odabranu turu");
                    
                    /*foreach (Tour t in Tours.ToList())
                    {
                        if (t.LocationId == SelectedTour.LocationId)
                        {
                            Tours.Add(t);
                        }
                    }*/
                    AlternativeTours alternativeTours = new AlternativeTours(SelectedTour);
                    alternativeTours.Show();
               }
            }
        }

        private string _cityAndCountry;

        public string CityAndCountry
        {
            get { return _cityAndCountry; }
            set
            {
                _cityAndCountry = value;

                OnPropertyChanged();
            }
        }

        private string _searchText;

        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;

                OnPropertyChanged("SearchText");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ClickSearch(object sender, RoutedEventArgs e)
        {
            if(SearchText != null && SearchText != "")
            {
                string text = SearchText.ToLower();
                
                ObservableCollection<Tour> tours= new ObservableCollection<Tour>();

                foreach (Tour tour in Tours)
                {
                    if (tour.Duration.ToString().ToLower().Contains(text))
                    {
                        tours.Add(tour);
                    }
                    else if(tour.Language.ToLower().Contains(text))
                    {
                        tours.Add(tour);
                    }
                    else if(tour.MaxNumOfGuests > int.Parse(text)) 
                    {
                        tours.Add(tour);
                    }

                    ToursGrid.ItemsSource= tours;
                }
            }
            else
            {
                ToursGrid.ItemsSource = Tours;
            }
        }
    }
}
//tour.MaxNumOfGuests.ToString().ToLower().Contains(text)