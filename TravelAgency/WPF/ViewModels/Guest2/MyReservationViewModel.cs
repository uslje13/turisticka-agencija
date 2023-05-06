using SOSTeam.TravelAgency.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest2
{
    public class MyReservationViewModel : ViewModel
    {
        public string Name { get; set; }
        public string LocationFullName { get; set; }
        public string Language { get; set; }
        public string Start { get; set; }
        public int TouristNum { get; set; }
        public string ImagePath { get; set; }

        private RelayCommand _reportCommand;

        public RelayCommand ReportCommand
        {
            get { return _reportCommand; }
            set
            {
                _reportCommand = value;
            }
        }
        public MyReservationViewModel(string name, string locationFullName,string language, DateTime start, int touristNum, string imagePath) 
        {
            Name = name;
            LocationFullName = locationFullName;
            Language = language;
            Start = start.ToString();
            TouristNum = touristNum;
            ReportCommand = new RelayCommand(Execute_ReportCommand,CanExecuteMethod);
            ImagePath= imagePath;
        }

        private void Execute_ReportCommand(object obj)
        {
            throw new NotImplementedException();
        }

        private bool CanExecuteMethod(object parameter)
        {
            return true;
        }
    }
}
