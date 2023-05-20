﻿using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Owner
{
    public class RenovationPageViewModel : ViewModel
    {
        public User LoggedInUser { get; private set; }

        private ObservableCollection<RenovationViewModel> _renovations;
        public ObservableCollection<RenovationViewModel> Renovations
        {
            get { return _renovations; }
            set
            {
                _renovations = value;
                OnPropertyChanged("Renovations");
            }
        }

        public RenovationViewModel SelectedRenovation { get; set; }

        public RelayCommand DeleteRenovation { get; private set; }
        public RelayCommand AddRenovation { get; private set; }


        private MainWindowViewModel _mainwindowVM;


        private ImageService _imageService;
        private AccommodationService _accommodationService;
        private AccommodationRenovationService _accommodationRenovationService;






        public RenovationPageViewModel(User user, MainWindowViewModel mainWindowVM)
        {
            LoggedInUser = user;
            _accommodationService = new();
            _imageService = new();
            _mainwindowVM = mainWindowVM;
            _accommodationRenovationService = new();

            var accommodations = _accommodationService.GetAllByUserId(user.Id);
            var renovations = _accommodationRenovationService.GetAll().Where(r => accommodations.Any(a => a.Id == r.AccommodationId));
            SetRenovationsCollection(accommodations, renovations);
            DeleteRenovation = new RelayCommand(Execute_DeleteRenovation, CanExecuteDeleteRenovation);
            AddRenovation = new RelayCommand(Execute_AddRenovation, CanExecuteAddRenovation);

        }

        private void Execute_AddRenovation(object obj)
        {
            _mainwindowVM.Execute_NavigationButtonCommand("RenovationAdd");
        }

        private bool CanExecuteAddRenovation(object obj)
        {
            return true;
        }

        private bool CanExecuteDeleteRenovation(object obj)
        {
            return SelectedRenovation != null;
        }

        private void Execute_DeleteRenovation(object obj)
        {
            _accommodationRenovationService.Delete(SelectedRenovation.Id);
            Renovations.Remove(SelectedRenovation);
        }

        private void SetRenovationsCollection(List<Accommodation> accommodations, IEnumerable<AccommodationRenovation> renovations)
        {
            Renovations = new ObservableCollection<RenovationViewModel>();
            foreach (var renovation in renovations)
            {
                Renovations.Add(new RenovationViewModel(
                    renovation.Id,
                    accommodations.Find(a => a.Id == renovation.AccommodationId).Name,
                    _imageService.GetAccommodationCover(renovation.AccommodationId).Path,
                    renovation.FirstDay.ToString("dd/MM/yyyy") + " - " + renovation.LastDay.ToString("dd/MM/yyyy")
                    ));
            }
        }
    }

    public class RenovationViewModel 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PictureUrl { get; set; }
        public string RenovationPeriod { get; set; }

        public RenovationViewModel(int id, string name, string pictureUrl, string renovationPeriod)
        {
            Id = id;
            Name = name;
            PictureUrl = pictureUrl;
            RenovationPeriod = renovationPeriod;
        }
    }
}
