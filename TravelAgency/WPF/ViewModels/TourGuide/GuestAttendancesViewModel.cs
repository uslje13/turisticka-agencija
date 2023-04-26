using System;
using System.Collections.ObjectModel;


namespace SOSTeam.TravelAgency.WPF.ViewModels.TourGuide
{
    public class GuestAttendancesViewModel : ViewModel
    {
        public ObservableCollection<GuestAttendanceCardViewModel> GuestAttendanceCards { get; set; }

        public DateTime? Date { get; set; }

        public string TourName { get; set; }

        public string CheckpointName { get; set; }

        public GuestAttendancesViewModel(CheckpointCardViewModel selectedCheckpointCard, string tourName, DateTime? date)
        {
            var guestAttendanceCardCreator = new GuestAttendanceCardCreatorViewModel();
            GuestAttendanceCards = guestAttendanceCardCreator.CreateCards(selectedCheckpointCard);
            TourName = tourName;
            Date = date;
            CheckpointName = selectedCheckpointCard.Name;
        }
    }
}
