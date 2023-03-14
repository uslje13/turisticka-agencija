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
    /// Interaction logic for AddImegesWindow.xaml
    /// </summary>
    public partial class AddImagesWindow : Window
    {
        public Model.Image SelectedImage { get; set; }
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

        

        public AddImagesWindow(ObservableCollection<Model.Image> images)
        {
            InitializeComponent();
            DataContext = this;
            Images = images;
            if (IsImagesListEmpty()) 
            {
                checkCoverButton.IsEnabled = false;
            }
        }
        private bool IsImagesListEmpty()
        {
            if (_images.Count < 1)
            {
                return true;
            }
            return false;
        }

        private bool AlreadyOwnsCover()
        {
            foreach(var image in Images)
            {
                if(image.Cover == true)
                {
                    return true;
                }
            }
            return false;
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
            urlTextBox.Text = string.Empty;

            if (!IsImagesListEmpty() && !AlreadyOwnsCover())
            {
                checkCoverButton.IsEnabled = true;
            }
        }

        private void ConfirmButtonClick(object sender, RoutedEventArgs e)
        {
            if (Images.Count < 1)
            {
                MessageBox.Show("Morate dodati makar jednu sliku!.");
            }
            else
            {
                Close();
            }
        }
        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            Images.Clear();
            Close();
        }

        private void CheckCoverClickButton(object sender, RoutedEventArgs e)
        {
            if (SelectedImage == null)
            {
                MessageBox.Show("Morate odabrati sliku!");
            }
            else
            {
                SelectedImage.Cover = true;
                checkCoverButton.IsEnabled = false;
            }
        }
    }
}
