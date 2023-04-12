using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;

namespace SOSTeam.TravelAgency.WPF.ViewModels.TourGuide
{
    public class AddCheckpointViewModel : ViewModel
    {
        private readonly CheckpointService _checkpointService;
        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                if (value != _name)
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
                if (_type != value)
                {
                    _type = value;
                    OnPropertyChanged("Type");
                }
            }
        }

        private ObservableCollection<Checkpoint> _checkpoints;

        public ObservableCollection<Checkpoint> Checkpoints
        {
            get => _checkpoints;
            set
            {
                if (_checkpoints != value)
                {
                    _checkpoints = value;
                    OnPropertyChanged("Checkpoints");
                }
            }
        }

        private Checkpoint _selectedCheckpoint;

        public Checkpoint SelectedCheckpoint
        {
            get => _selectedCheckpoint;
            set
            {
                if (_selectedCheckpoint != value)
                {
                    _selectedCheckpoint = value;
                    OnPropertyChanged("SelectedCheckpoint");
                }
            }
        }

        public RelayCommand AddCheckpointCommand { get; set; }

        public AddCheckpointViewModel()
        {
            Checkpoints = new ObservableCollection<Checkpoint>();
            _checkpointService = new CheckpointService();

            AddCheckpointCommand = new RelayCommand(AddCheckpoint, CanExecuteMethod);
        }

        private bool CanExecuteMethod(object parameter)
        {
            return true;
        }

        public void AddCheckpoint(object sender)
        {
            Checkpoint checkpoint = new Checkpoint();
            checkpoint.Name = Name;
            checkpoint.Type = Type;
            Checkpoints.Add(checkpoint);
        }

    }
}
