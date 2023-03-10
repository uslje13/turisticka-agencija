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

namespace TravelAgency.View
{
    /// <summary>
    /// Interaction logic for CreateTourDates.xaml
    /// </summary>
    public partial class CreateTourDates : Window
    {
        private DateTime _start;
        private ObservableCollection<DateAndOccupancy> _datesAndOccupancies;

        public DateTime Start 
        { 
            get => _start; 
            set
            {
                if(value != _start)
                {
                    _start = value;
                    OnPropertyChanged();
                }
            } 
        }

        public ObservableCollection<DateAndOccupancy> DatesAndOccupancies
        { 
            get => _datesAndOccupancies; 
            set
            {
                if(value != _datesAndOccupancies)
                {
                    _datesAndOccupancies = value;
                    OnPropertyChanged();
                }
            } 
        }

        public CreateTourDates(ObservableCollection<DateAndOccupancy> datesAndOccupancies)
        {
            InitializeComponent();
            DataContext = this;
            timeTextBox.IsReadOnly = true;
            addTimeButton.IsEnabled = false;
            Start = DateTime.UtcNow;
            _datesAndOccupancies = datesAndOccupancies;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void AddDateButtonClick(object sender, RoutedEventArgs e)
        {
            DateAndOccupancy dateAndOccupancy = new DateAndOccupancy();
            dateAndOccupancy.Start = Start;
            dateAndOccupancy.Occupancy = 0;
            DatesAndOccupancies.Add(dateAndOccupancy);
        }
    }
}
