using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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

        public ObservableCollection<String> _checkpointTypes;

        public ObservableCollection<String> CheckpointsTypes
        {
            get => _checkpointTypes;
            set
            {
                if (_checkpointTypes != value)
                {
                    _checkpointTypes = value;
                    OnPropertyChanged("CheckpointsTypes");
                }
            }
        }

        public RelayCommand AddCheckpointCommand { get; set; }
        public RelayCommand DeleteCheckpointCommand { get; set; }
        public  RelayCommand ClearCheckpointsCommand { get; set; }

        public AddCheckpointViewModel(ObservableCollection<Checkpoint> checkpoints)
        {
            CheckpointsTypes = InitializeCheckpointTypes();
            Checkpoints = checkpoints;
            _checkpointService = new CheckpointService();

            
            AddCheckpointCommand = new RelayCommand(AddCheckpoint, CanExecuteMethod);
            DeleteCheckpointCommand = new RelayCommand(DeleteCheckpoint, CanExecuteMethod);
            ClearCheckpointsCommand = new RelayCommand(DeleteAllCheckpoints, CanExecuteMethod);
        }

        private bool CanExecuteMethod(object parameter)
        {
            return true;
        }

        public ObservableCollection<String> InitializeCheckpointTypes()
        {
            ObservableCollection<String> checkpointsTypes = new ObservableCollection<String>();
            checkpointsTypes.Add("START");
            checkpointsTypes.Add("EXTRA");
            checkpointsTypes.Add("END");

            return checkpointsTypes;
        }

        public void AddCheckpoint(object sender)
        {
            Checkpoint checkpoint = new Checkpoint();
            checkpoint.Name = Name;
            checkpoint.Type = Type;

            if (checkpoint.Type == CheckpointType.START)
            {
                CheckpointsTypes.Remove("START");
                if (Checkpoints.Count > 0)
                {
                    Checkpoints.Insert(0, checkpoint);
                }
                else
                {
                    Checkpoints.Add(checkpoint);
                }
            }
            else if (checkpoint.Type == CheckpointType.END)
            {
                CheckpointsTypes.Remove("END");
                if (Checkpoints.Count > 0)
                {
                    Checkpoints.Insert(Checkpoints.Count,checkpoint);
                }
                else
                {
                    Checkpoints.Add(checkpoint);
                }
            }
            else if (checkpoint.Type == CheckpointType.EXTRA)
            {
                if (Checkpoints.Count == 0)
                {
                    Checkpoints.Add(checkpoint);
                }
                else if (Checkpoints.Count == 1)
                {
                    if (Checkpoints[0].Type == CheckpointType.START || Checkpoints[0].Type == CheckpointType.EXTRA)
                    {
                        Checkpoints.Add(checkpoint);
                    }
                    else if (Checkpoints[0].Type == CheckpointType.END)
                    {
                        Checkpoints.Insert(Checkpoints.Count-1,checkpoint);
                    }
                }
                else if(Checkpoints.Count > 1)
                {
                    if (Checkpoints[Checkpoints.Count - 1].Type == CheckpointType.END)
                    {
                        Checkpoints.Insert(Checkpoints.Count - 1, checkpoint);
                    }
                    else
                    {
                        Checkpoints.Insert(Checkpoints.Count, checkpoint);
                    }
                }
            }
        }

        public void DeleteAllCheckpoints(object sender)
        {
            Checkpoints.Clear();
        }

        public void DeleteCheckpoint(object sender)
        {
            var selectedCheckpoint = sender as Checkpoint;
            Checkpoints.Remove(selectedCheckpoint);
            if (selectedCheckpoint.Type == CheckpointType.START)
            {
                CheckpointsTypes.Insert(0,"START");
            }
            if (selectedCheckpoint.Type == CheckpointType.END)
            {
                CheckpointsTypes.Insert(CheckpointsTypes.Count,"END");
            }
        }

    }
}
