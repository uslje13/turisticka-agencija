using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.WPF.Views.Guest2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest2
{
    public class VouchersWindowViewModel : ViewModel
    {
        private VouchersWindow _window;
        private RelayCommand _backCommand;

        public RelayCommand BackCommand
        {
            get { return _backCommand; }
            set
            {
                _backCommand = value;
            }
        }

        public VouchersWindowViewModel(VouchersWindow window)
        {
            _window = window;
            BackCommand = new RelayCommand(Execute_CancelCommand, CanExecuteMethod);
        }

        private bool CanExecuteMethod(object parameter)
        {
            return true;
        }

        private void Execute_CancelCommand(object sender)
        {
            _window.Close();
        }
    }
}
