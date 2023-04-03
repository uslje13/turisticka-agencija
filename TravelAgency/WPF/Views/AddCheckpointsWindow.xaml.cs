using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using SOSTeam.TravelAgency.Domain.Models;

namespace SOSTeam.TravelAgency.WPF.Views
{
    /// <summary>
    /// Interaction logic for CreateCheckpointsWindow.xaml
    /// </summary>
    public partial class AddCheckpointsWindow : Window
    {
        private ObservableCollection<Checkpoint> _checkponits;
        public ObservableCollection<Checkpoint> Checkpoints
        {
            get => _checkponits;
            set
            {
                if (!value.Equals(_checkponits))
                {
                    _checkponits = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _checkpointName;  //wpf framework already have name
        public string CheckpointName
        {
            get => _checkpointName;
            set
            {
                if (!value.Equals(_checkpointName))
                {
                    _checkpointName = value;
                    OnPropertyChanged();
                }
            }
        }

        public Checkpoint SelectedCheckpoint { get; set; }

        private int _extraCheckpointIndex;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public AddCheckpointsWindow(ObservableCollection<Checkpoint> checkpoints)
        {
            InitializeComponent();
            DataContext = this;
            Checkpoints = checkpoints;

            _extraCheckpointIndex = 1;
            DisableExtraCheckpoint();
        }

        private void AddStartCPButtonClick(object sender, RoutedEventArgs e)
        {
            Checkpoint checkpoint = new Checkpoint();
            checkpoint.Name = CheckpointName;
            checkpoint.Type = CheckpointType.START;
            Checkpoints.Insert(0, checkpoint);
            CheckpointName = string.Empty;

            DisableStartCheckpoint();
        }


        private void AddEndCPButtonClick(object sender, RoutedEventArgs e)
        {
            Checkpoint checkpoint = new Checkpoint();
            checkpoint.Name = CheckpointName;
            checkpoint.Type = CheckpointType.END;
            Checkpoints.Add(checkpoint);
            CheckpointName = string.Empty;

            DisableEndCheckpoint();
        }

        private void AddExtraCPButtonClick(object sender, RoutedEventArgs e)
        {
            Checkpoint checkpoint = new Checkpoint();
            checkpoint.Name = CheckpointName;
            checkpoint.Type = CheckpointType.EXTRA;
            Checkpoints.Insert(_extraCheckpointIndex, checkpoint);
            CheckpointName = string.Empty;

            addExtraCPTextBox.Text = string.Empty;
        }
        private void DisableStartCheckpoint()
        {
            addStartCPButton.IsEnabled = false;
            addStartCPTextBox.IsReadOnly = true;
            EnableExtraCheckpoints();
        }

        private void DisableEndCheckpoint()
        {
            addEndCPButton.IsEnabled = false;
            addEndCPTextBox.IsReadOnly = true;
            EnableExtraCheckpoints();
        }

        private void DisableExtraCheckpoint()
        {
            addExtraCPTextBox.IsReadOnly = true;
            addExtraCPButton.IsEnabled = false;
        }

        private void EnableExtraCheckpoints()
        {
            if (Checkpoints.Count == 2)
            {
                addExtraCPButton.IsEnabled = true;
                addExtraCPTextBox.IsReadOnly = false;
            }
        }

        private void ConfirmButtonClick(object sender, RoutedEventArgs e)
        {
            if (Checkpoints.Count < 2)
            {
                MessageBox.Show("Ne možete kreirati ključne tačke!\nMorate uneti obavezne ključne tačke (START, END).");
            }
            else
            {
                Close();
            }

        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            Checkpoints.Clear();
            Close();
        }
    }
}
