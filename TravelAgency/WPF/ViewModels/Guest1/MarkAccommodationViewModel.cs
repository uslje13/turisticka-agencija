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
using System.Collections.ObjectModel;
using System.IO;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest1
{
    public class MarkAccommodationViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public User LoggedInUser { get; set; }
        public Window ThisWindow { get; set; }
        public string AccNameTextBlock { get; set; }
        public string AllUrls { get; set; }
        public List<string> AllUrlsList { get; set; }
        private string enteredGuestComment { get; set; }
        public string EnteredGuestComment
        {
            get { return enteredGuestComment; }
            set
            {
                enteredGuestComment = value;
                OnPropertyChaged("EnteredGuestComment");
            }
        }
        public CancelAndMarkResViewModel Accommodation { get; set; }
         
        public List<RadioButton> CleanMarks { get; set; }
        public List<RadioButton> OwnerMarks { get; set; }

        public ObservableCollection<CancelAndMarkResViewModel> ReservationsForMark { get; set; }

        private ObservableCollection<System.Windows.Controls.Image> selectedImages;
        public ObservableCollection<System.Windows.Controls.Image> SelectedImages
        {
            get { return selectedImages; }
            set
            {
                selectedImages = value;
                OnPropertyChaged("SelectedImages");
            }
        }
        public RelayCommand MarkAccCommand { get; set; }
        public RelayCommand AddImagesCommand { get; set; }
        public RelayCommand DeleteSharedImageCommand { get; set; }
        public RelayCommand GoBackCommand { get; set; }

        public MarkAccommodationViewModel(List<RadioButton> cleans, List<RadioButton> owners, User user, CancelAndMarkResViewModel acc, Window window, ObservableCollection<CancelAndMarkResViewModel> reservationsForMark) 
        {
            AccNameTextBlock = acc.AccommodationName;
            CleanMarks = cleans;
            OwnerMarks = owners;
            LoggedInUser = user;
            Accommodation = acc;
            ThisWindow = window;
            AllUrls = String.Empty;
            ReservationsForMark = reservationsForMark;

            SelectedImages = new ObservableCollection<System.Windows.Controls.Image>();
            AllUrlsList = new List<string>();

            MarkAccCommand = new RelayCommand(ExecuteAccommodationMarking);
            AddImagesCommand = new RelayCommand(Execute_AddImages);
            GoBackCommand = new RelayCommand(Execute_GoBack);
            DeleteSharedImageCommand = new RelayCommand(Execute_DeleteSharedImage);
        }

        protected void OnPropertyChaged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Execute_DeleteSharedImage(object sender)
        {
            System.Windows.Controls.Image? selected = sender as System.Windows.Controls.Image;
            SelectedImages.Remove(selected);
            RemoveURL(selected);
        }

        private void RemoveURL(System.Windows.Controls.Image selected)
        {
            string realPath = selected.Source.ToString();
            string deletingPath = "";
            foreach(var url in AllUrlsList)
            {
                if(realPath.Contains(url))
                {
                    deletingPath = url;
                    break;
                }
            }

            AllUrlsList.Remove(deletingPath);
        }

        private void Execute_GoBack(object sender)
        {
            ThisWindow.Close();
        }

        private void Execute_AddImages(object sender)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Priložite slike smještaja";
            op.Filter = "Image Files|*.jpg;*.png;*.bmp|All Files|*.*";
            op.Multiselect = true;
            op.InitialDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources\\Images\\Guest1");

            if (op.ShowDialog() == true)
            {
                foreach(string imageUrl in op.FileNames)
                {
                    string csvPath = CreateCSVPath(op, imageUrl);
                    AllUrlsList.Add(csvPath);
                    
                    System.Windows.Controls.Image image = new System.Windows.Controls.Image();
                    image.Source = new BitmapImage(new Uri(imageUrl));
                    SelectedImages.Add(image);
                }
            } 
        }

        private string CreateCSVPath(OpenFileDialog op, string imageUrl)
        {
            string defaultPath = imageUrl.Replace(op.InitialDirectory, "").TrimStart('\\');
            string updatedPath = Path.Combine("/Resources/Images/Guest1", defaultPath).Replace('\\', '/');
            string[] dividedPath = updatedPath.Split('/');
            string csvPath = "/";
            int j = 0;
            foreach (string part in dividedPath)
            {
                j++;
                if (part.Equals("Resources"))
                {
                    csvPath += part + "/";
                }
                if (part.Equals("Images"))
                {
                    csvPath += part + "/";
                }
                if (part.Equals("Guest1"))
                {
                    csvPath += part + "/";
                }
                if (dividedPath.Count() == j)
                {
                    csvPath += part;
                }
            }

            return csvPath;
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
            foreach(var url in AllUrlsList)
            {
                AllUrls += url + ",";
            }
            GuestAccMarkService service = new GuestAccMarkService();
            service.MarkAccommodation(cleanMark, ownerMark, EnteredGuestComment, AllUrls, LoggedInUser, Accommodation);
            MessageBox.Show("Uspješno ocjenjen smještaj!");
            ReservationsForMark.Remove(Accommodation);
            ThisWindow.Close();
        }
    }
}
