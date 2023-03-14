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

namespace TravelAgency.View
{
    /// <summary>
    /// Interaction logic for AddImegesWindow.xaml
    /// </summary>
    public partial class AddImegesWindow : Window
    {
        private ObservableCollection<Model.Image> _images;
        private string _url;

        public ObservableCollection<Model.Image> Images
        { 
            get => _images;
            set
            {
                if (!value.Equals(_images)) 
                { 
                    _images = value;
                    OnPropertyChanged();
                }
            } 
        }

        public string Url 
        { 
            get => _url;
            set
            {
                if (!value.Equals(_url))
                {
                    _url = value;
                    OnPropertyChanged();
                }
            }
        }

        public AddImegesWindow()
        {
            InitializeComponent();
            DataContext = this;
            Images = new ObservableCollection<Model.Image>();

        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void AddUrlButtonClick(object sender, RoutedEventArgs e)
        {
            Model.Image image = new Model.Image();
            image.Url = Url;
            Images.Add(image);
        }
    }
}
