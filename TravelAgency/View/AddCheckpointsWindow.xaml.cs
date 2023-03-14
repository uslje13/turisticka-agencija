using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TravelAgency.Model;
using TravelAgency.Repository;

namespace TravelAgency.View
{
    /// <summary>
    /// Interaction logic for CreateCheckpoint.xaml
    /// </summary>
    public partial class AddCheckpointsWindow : Window
    {
        public Checkpoint SelectedCheckpoint { get; set; }

        private ObservableCollection<Checkpoint> _chechponits;
        
        private string _checkpointName;  //wpf framework already have name

        private int _extraCheckpointIndex;

        public ObservableCollection<Checkpoint> Checkpoints
        {
            get => _chechponits;
            set
            {
                if (value != Checkpoints)
                {
                    _chechponits = value;
                    OnPropertyChanged();
                }
            }
        }

        public string CheckpointName 
        { 
            get => _checkpointName;
            set
            {
                if (value != _checkpointName)
                {
                    _checkpointName = value;
                    OnPropertyChanged();
                }
            } 
        }

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
            addExtraCPButton.IsEnabled = false;
            _extraCheckpointIndex = 1;
        }

        private void AddStartCPButtonClick(object sender, RoutedEventArgs e)
        {
            Checkpoint checkpoint = new Checkpoint();
            checkpoint.Name = CheckpointName;
            checkpoint.Type = CheckpointType.START;
            addStartCPButton.IsEnabled = false;
            addStartCPTextBox.IsReadOnly = true;
            Checkpoints.Add(checkpoint);
        }


        private void AddEndCPButtonClick(object sender, RoutedEventArgs e)
        {
            Checkpoint checkpoint = new Checkpoint();
            checkpoint.Name = CheckpointName;
            checkpoint.Type = CheckpointType.END;
            addEndCPButton.IsEnabled = false;
            addEndCPTextBox.IsReadOnly = true;
            Checkpoints.Add(checkpoint);
            EnableExtraCheckpoints();
        }

        private void AddExtraCPButtonClick(object sender, RoutedEventArgs e)
        {
            Checkpoint checkpoint = new Checkpoint();
            checkpoint.Name = CheckpointName;
            checkpoint.Type = CheckpointType.EXTRA;
            addExtraCPTextBox.Text = string.Empty;
            Checkpoints.Insert(_extraCheckpointIndex, checkpoint);
            _extraCheckpointIndex++;
            EnableExtraCheckpoints();
        }

        private void EnableExtraCheckpoints()
        {
            if(Checkpoints.Count == 2) 
            {
                addExtraCPButton.IsEnabled = true;
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
