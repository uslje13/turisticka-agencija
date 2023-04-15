using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.Views.Owner;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Owner
{
    internal class AccommodationPageViewModel : ViewModel
    {
        public User LoggedInUser { get; private set; }
        private AccommodationsPage _accommodationsPage;
        private AccommodationService _accommodationService;
        private MainWindowViewModel _mainwindowVM;
        public ObservableCollection<PictureViewModel> Accommodations { get; set; }
        public PictureViewModel SelectedAccommodation { get; set; }
        public RelayCommand OpenAccommodationDetails { get; private set; }
        public RelayCommand AddAccommodation { get; private set; }
        public RelayCommand EditAccommodation { get; private set; }
        public RelayCommand DeleteAccommodation { get; private set; }

        public AccommodationPageViewModel(User user,MainWindowViewModel mainWindowVM,AccommodationsPage accommodationPage)
        {
            LoggedInUser = user;
            _accommodationsPage = accommodationPage;
            _mainwindowVM = mainWindowVM;
            _accommodationService = new();
            Accommodations = new();

            AddAccommodation = new RelayCommand(Execute_AddAccommodation, CanExecuteAddAccommodation);
            EditAccommodation = new RelayCommand(Execute_EditAccommodation, CanExecuteEditAccommodation);
            DeleteAccommodation = new RelayCommand(Execute_DeleteAccommodation, CanExecuteDeleteAccommodation);
            OpenAccommodationDetails = new RelayCommand(Execute_OpenAccommodationDetails, CanExecuteOpenAccommodationDetails);



        }

        private bool CanExecuteOpenAccommodationDetails(object obj)
        {
            return SelectedAccommodation != null;
        }


        private bool CanExecuteDeleteAccommodation(object obj)
        {
            return SelectedAccommodation != null;
        }

        private bool CanExecuteEditAccommodation(object obj)
        {
            return true;
        }

        private bool CanExecuteAddAccommodation(object obj)
        {
            return true;
        }

        private void Execute_OpenAccommodationDetails(object obj)
        {
            _mainwindowVM.SetPage(new AccommodationInfoPage(LoggedInUser,_accommodationService.GetById(SelectedAccommodation.AccommodationId)));
        }


        private void Execute_DeleteAccommodation(object obj)
        {
            _accommodationService.Delete(SelectedAccommodation.AccommodationId);
            Accommodations.Remove(SelectedAccommodation);
        }

        private void Execute_EditAccommodation(object obj)
        {
            return;
        }

        private void Execute_AddAccommodation(object obj)
        {
            _mainwindowVM.Execute_NavigationButtonCommand("AccommodationAdd");
            return;
        }

        public void FillAccommodationsPanel()
        {

            foreach (Accommodation accommodation in _accommodationService.GetAllByUserId(LoggedInUser.Id))
            {
                Accommodations.Add(new PictureViewModel(accommodation.Name, "/Resources/Images/UnknownPhoto.png",accommodation.Id));
            }

        }



        
    }

    public class PictureViewModel
    {
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public int AccommodationId { get; private set; }

        public PictureViewModel(string name,string imagePath, int accommodationId) 
        {
            Name = name;
            ImagePath = imagePath;
            AccommodationId = accommodationId;
        }
        
    }

}
