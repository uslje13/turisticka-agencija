using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
    public partial class CreateCheckpoint : Window
    {

        public static ObservableCollection<Checkpoint> Checkpoints { get; set; }
        Checkpoint SelectedChecpoint { get; set; }
        private readonly CheckpointRepository _repository;
        public CreateCheckpoint()
        {
            InitializeComponent();
            DataContext = this;
            _repository = new CheckpointRepository();
            Checkpoints = new ObservableCollection<Checkpoint>(_repository.GetAll());
        }
    }
}
