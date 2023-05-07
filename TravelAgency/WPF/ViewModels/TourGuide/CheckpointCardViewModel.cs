using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using SOSTeam.TravelAgency.Domain.Models;

namespace SOSTeam.TravelAgency.WPF.ViewModels.TourGuide
{
    public class CheckpointCardViewModel : ViewModel
    {
        private string _name;

        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        private CheckpointType _type;

        public CheckpointType Type
        {
            get => _type;
            set
            {
                if (value != _type)
                {
                    _type = value;
                    OnPropertyChanged("Type");
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

        public CheckpointCardViewModel()
        {
            _name = string.Empty;
            _type = CheckpointType.UNKNOWN;
            _canEdit = true;
            _canDelete = true;
            _background = new SolidColorBrush(Colors.AliceBlue);
        }
    }
}
