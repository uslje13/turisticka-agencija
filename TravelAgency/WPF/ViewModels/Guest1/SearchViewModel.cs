using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.Views;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Linq;
using System.Windows;
using SOSTeam.TravelAgency.WPF.Views.Guest1;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest1
{
    public class SearchViewModel
    {
        public User LoggedInUser { get; set; }
        public List<TextBox> textBoxes { get; set; }
        public ComboBox CBTypes { get; set; }
        public List<ComboBoxItem> comboBoxItems { get; set; }
        public RelayCommand searchCommand { get; set; }
        public RelayCommand cancelCommand { get; set; }
        public Frame ThisFrame { get; set; }

        public SearchViewModel(User user, List<TextBox> list1, List<ComboBoxItem> list2, ComboBox comboBox, Frame frame)
        {
            LoggedInUser = user;
            CBTypes = comboBox;
            textBoxes = list1;
            comboBoxItems = list2;
            ThisFrame = frame;

            searchCommand = new RelayCommand(ExecuteAccommodationSearch);
            cancelCommand = new RelayCommand(ExecuteCancelAccommodationSearch);
        }

        private void ExecuteAccommodationSearch(object sender)
        {
            AccommodationService accommodationService = new AccommodationService();
            SuperOwnerService superOwnerService = new();
            List<LocAccommodationViewModel> searchResult = accommodationService.ExecuteAccommodationSearch(textBoxes[0].Text, textBoxes[1].Text, textBoxes[2].Text, 
                                                                                                            GetSelectedItem(CBTypes), int.Parse(textBoxes[3].Text), 
                                                                                                            int.Parse(textBoxes[4].Text));
            foreach (LocAccommodationViewModel item in searchResult) 
            {
                item.IsSuperOwned = superOwnerService.IsSuperOwnerAccommodation(item.AccommodationId);
            }

            ShowResults(searchResult.OrderByDescending(a => a.IsSuperOwned).ToList());
        }

        private void ExecuteCancelAccommodationSearch(object sender)
        {
            //ThisWindow.Close();
        }

        private LocAccommodationViewModel.AccommType GetSelectedItem(ComboBox cb)
        {
            if (cb.SelectedItem == comboBoxItems[0]) return LocAccommodationViewModel.AccommType.APARTMENT;
            else if (cb.SelectedItem == comboBoxItems[1]) return LocAccommodationViewModel.AccommType.HOUSE;
            else if (cb.SelectedItem == comboBoxItems[2]) return LocAccommodationViewModel.AccommType.HUT;
            else return LocAccommodationViewModel.AccommType.NOTYPE;
        }

        private void ShowResults(List<LocAccommodationViewModel> results)
        {
            if (results.Count > 0)
            {
                var navigationService = ThisFrame.NavigationService;
                navigationService.Navigate(new SearchResultsPage(results, LoggedInUser, ThisFrame));
                //SearchResultsPage newWindow = new SearchResultsPage(results, LoggedInUser);
            }
            else
            {
                MessageBox.Show("Nepostojeća kombinacija podataka. Pokušajte ponovo.");
            }
        }
    }
}
