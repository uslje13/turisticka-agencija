using SOSTeam.TravelAgency.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SOSTeam.TravelAgency.WPF.ViewModels.TourGuide
{
    public class AppointmentCardViewModel : ViewModel
    {
        private DateTime _start;

        public DateTime Start
        {
            get => _start;
            set
            {
                if (_start != value)
                {
                    _start = value;
                    OnPropertyChanged("Start");
                }
            }
        }

        private bool _canEdit;

        public bool CanEdit
        {
            get => _canEdit;
            set
            {
                if (value != _canEdit)
                {
                    _canEdit = value;
                    OnPropertyChanged("CanEdit");
                }
            }
        }

        private bool _canDelete;

        public bool CanDelete
        {
            get => _canDelete;
            set
            {
                if (value != _canDelete)
                {
                    _canDelete = value;
                    OnPropertyChanged("CanDelete");
                }
            }
        }

        private SolidColorBrush _background;

        public SolidColorBrush Background
        {
            get => _background;
            set
            {
                if (value != _background)
                {
                    _background = value;
                    OnPropertyChanged("Background");
                }
            }
        }

        public AppointmentCardViewModel()
        {
            _start = DateTime.MinValue;
            _canEdit = true;
            _canDelete = true;
            _background = new SolidColorBrush(Colors.AliceBlue);
        }

    }
}
