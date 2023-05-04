using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Xml.Linq;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.Views.Guest1;
using System.Windows.Data;
using Microsoft.Win32;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.ComponentModel;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest1
{
    public class MarkAccommodationViewModel : INotifyPropertyChanged
    {
        public List<RadioButton> CleanMarks { get; set; }
        public List<RadioButton> OwnerMarks { get; set; }
        public TextBox GuestComment { get; set; }
        public RelayCommand MarkAccCommand { get; set; }
        public RelayCommand AddImagesCommand { get; set; }
        public User LoggedInUser { get; set; }
        public CancelAndMarkResViewModel Accommodation { get; set; }
        public Window ThisWindow { get; set; }
        public TextBlock AccommodationNameTb { get; set; }
        public ListBox Images { get; set; }
        public RelayCommand DeleteSharedImageCommand { get; set; }
        public RelayCommand GoBackCommand { get; set; }

        private List<System.Windows.Controls.Image> selectedImages;
        //-----------------------------------------------------------------------------
        //-----------------------------------------------------------------------------
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChaged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));  
        }

        public List<System.Windows.Controls.Image> SelectedImages
        {
            get { return selectedImages; }
            set
            {
                selectedImages = value;
                OnPropertyChaged("SelectedImages");
            }
        }
        //-----------------------------------------------------------------------------
        //-----------------------------------------------------------------------------

        public MarkAccommodationViewModel(TextBlock tBlock, List<RadioButton> cleans, List<RadioButton> owners, TextBox comment, User user, CancelAndMarkResViewModel acc, Window window, ListBox images) 
        {
            AccommodationNameTb = tBlock;
            CleanMarks = cleans;
            OwnerMarks = owners;
            GuestComment = comment;
            LoggedInUser = user;
            Accommodation = acc;
            ThisWindow = window;
            Images = images;
            SelectedImages = new List<System.Windows.Controls.Image>();

            FillTextBox(acc);

            MarkAccCommand = new RelayCommand(ExecuteAccommodationMarking);
            AddImagesCommand = new RelayCommand(Execute_AddImages);
            GoBackCommand = new RelayCommand(Execute_GoBack);
            DeleteSharedImageCommand = new RelayCommand(Execute_DeleteSharedImage);
        }

        private void Execute_DeleteSharedImage(object sender)
        {
            System.Windows.Controls.Image? selected = sender as System.Windows.Controls.Image;
            //SelectedImages.Remove(selected);
            SelectedImages.RemoveAt(selected.PersistId);
        }

        private void Execute_GoBack(object sender)
        {
            ThisWindow.Close();
        }

        private void Execute_AddImages(object sender)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Priložite slike smještaja";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            op.Multiselect = true;

            if (op.ShowDialog() == true)
            {
                foreach(string imageUrl in op.FileNames)
                {
                    System.Windows.Controls.Image image = new System.Windows.Controls.Image();
                    image.Source = new BitmapImage(new Uri(imageUrl));
                    SelectedImages.Add(image);
                }
            } 
            Images.ItemsSource = SelectedImages;
        }

        private void FillTextBox(CancelAndMarkResViewModel acc)
        {
            Binding binding = new Binding();
            AccommodationService service = new AccommodationService();
            Accommodation accommodation = service.GetById(acc.AccommodationId);
            binding.Source = accommodation.Name;
            AccommodationNameTb.SetBinding(TextBlock.TextProperty, binding);
        }

        public int FindCleanMark(List<RadioButton> list)
        {
            int i = 1;
            foreach(var radio in list)
            {
                if(radio.IsChecked == true)
                {
                    return i;
                }
                i++;
            }
            return -1;
        }

        public int FindOwnerMark(List<RadioButton> list)
        {
            int i = 1;
            foreach (var radio in list)
            {
                if (radio.IsChecked == true)
                {
                    return i;
                }
                i++;
            }
            return -1;
        }

        private void ExecuteAccommodationMarking(object sender)
        {
            int cleanMark = FindCleanMark(CleanMarks);
            int ownerMark = FindOwnerMark(OwnerMarks);
            GuestAccMarkService service = new GuestAccMarkService();
            service.MarkAccommodation(cleanMark, ownerMark, GuestComment.Text, "", LoggedInUser, Accommodation);
            MessageBox.Show("Uspješno ocjenjen smještaj!");
            ThisWindow.Close();
        }
    }
}
