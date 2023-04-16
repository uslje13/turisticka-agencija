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

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest1
{
    public class MarkAccommodationViewModel
    {
        public List<RadioButton> CleanMarks { get; set; }
        public List<RadioButton> OwnerMarks { get; set; }
        public TextBox GuestComment { get; set; }
        public TextBox GuestImagesUrls { get; set; }
        public RelayCommand MarkAccCommand { get; set; }
        public User LoggedInUser { get; set; }
        public CancelAndMarkResViewModel Accommodation { get; set; }
        public Window ThisWindow { get; set; }
        public TextBlock AccommodationNameTb { get; set; }

        public MarkAccommodationViewModel(TextBlock tBlock, List<RadioButton> cleans, List<RadioButton> owners, TextBox comment, TextBox urls, User user, CancelAndMarkResViewModel acc, Window window) 
        {
            AccommodationNameTb = tBlock;
            CleanMarks = cleans;
            OwnerMarks = owners;
            GuestComment = comment;
            GuestImagesUrls = urls;
            LoggedInUser = user;
            Accommodation = acc;
            ThisWindow = window;

            FillTextBox(acc);
            MarkAccCommand = new RelayCommand(ExecuteAccommodationMarking);
        }

        private void FillTextBox(CancelAndMarkResViewModel acc)
        {
            Binding binding = new Binding();
            AccommodationService service = new AccommodationService();
            Accommodation accommodation = service.GetById(acc.AccommodationId);
            binding.Source = accommodation.Name;
            AccommodationNameTb.SetBinding(TextBlock.TextProperty, binding);
        }

        private int FindCleanMark(List<RadioButton> list)
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

        private int FindOwnerMark(List<RadioButton> list)
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
            service.MarkAccommodation(cleanMark, ownerMark, GuestComment.Text, GuestImagesUrls.Text, LoggedInUser, Accommodation);
            MessageBox.Show("Uspješno ocjenjen smještaj!");
            ThisWindow.Close();
        }
    }
}
