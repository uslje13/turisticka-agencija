using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace TravelAgency.View
{
    /// <summary>
    /// Interaction logic for AddImegesWindow.xaml
    /// </summary>
    public partial class AddImagesWindow : Window
    {
        private ObservableCollection<Model.Image> _images;
        private Model.Image.ImageType ImageType;
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

        private string _url;
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

        public Model.Image SelectedImage { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public AddImagesWindow(ObservableCollection<Model.Image> images, Model.Image.ImageType imageType )
        {
            InitializeComponent();
            DataContext = this;
            Images = images;
            ImageType = imageType;

            DisableCoverButton();
        }

        private void DisableCoverButton()
        {
            if (Images.Count < 1 || AlreadyOwnsCover())
            {
                imageCoverButton.IsEnabled = false;
            }
        }

        private void EnableCoverButton()
        {
            if (Images.Count > 0 && !AlreadyOwnsCover())
            {
                imageCoverButton.IsEnabled = true;
            }
        }

        private bool AlreadyOwnsCover()
        {
            foreach (var image in Images)
            {
                if (image.Cover == true)
                {
                    return true;
                }
            }
            return false;
        }

        private void AddUrlButtonClick(object sender, RoutedEventArgs e)
        {
            Model.Image image = new Model.Image();
            image.Url = Url;
            image.Type = ImageType;
            Images.Add(image);
            urlTextBox.Text = string.Empty;

            EnableCoverButton();
        }

        private void ImageCoverClickButton(object sender, RoutedEventArgs e)
        {
            if (SelectedImage == null)
            {
                MessageBox.Show("Morate odabrati sliku!");
            }
            else
            {
                SelectedImage.Cover = true;
                DisableCoverButton();
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
    }
}
