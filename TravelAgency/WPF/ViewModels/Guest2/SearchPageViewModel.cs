using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.WPF.Views.Guest2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest2
{
    public class SearchPageViewModel
    {
        private SearchPage _searchPage;

        private RelayCommand _backCommand;
        public RelayCommand BackCommand
        {
            get { return _backCommand; }
            set
            {
                _backCommand = value;
            }
        }

        public SearchPageViewModel(SearchPage page)
        {
            _searchPage = page;

        }
    }
}
