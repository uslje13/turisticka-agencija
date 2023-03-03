using System;
using System.Collections.Generic;
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
    /// Interaction logic for DescriptionTour.xaml
    /// </summary>
    public partial class DescriptionTour : Window
    {

        private string _text;
        private readonly TourDescriptionRepository _repository;
        public string Text
        { 
            get => _text; 
            set
            {
                if (value != _text) 
                { 
                    _text = value;
                    OnPropertyChanged();
                }
            } 
        }

        public DescriptionTour()
        {
            InitializeComponent();
            DataContext = this;
            _repository = new TourDescriptionRepository();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)    //Da li kod string treba '?'
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ConfirmButtonClick(object sender, RoutedEventArgs e)
        {
            Comment description = new Comment(Text);
            _repository.Save(description);
            Close();
        }
    }
}
