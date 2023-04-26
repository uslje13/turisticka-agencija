using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Domain.Models;
using System.Collections.ObjectModel;

namespace SOSTeam.TravelAgency.WPF.ViewModels.TourGuide
{
    public class CheckpointCardCreatorViewModel
    {
        public Appointment? ActiveAppointment { get; private set; }

        private readonly AppointmentService _appointmentService;
        private readonly CheckpointActivityService _checkpointActivityService;
        private readonly CheckpointService _checkpointService;
        public CheckpointCardCreatorViewModel(User loggedUser)
        {
            _appointmentService = new AppointmentService();
            _checkpointActivityService = new CheckpointActivityService();
            _checkpointService = new CheckpointService();

            ActiveAppointment = _appointmentService.GetActiveByUserId(loggedUser.Id);
        }

        public ObservableCollection<CheckpointCardViewModel> CreateCards()
        {
            var checkpointCards = new ObservableCollection<CheckpointCardViewModel>();
            if (ActiveAppointment != null)
            {
                foreach (var checkpointActivity in _checkpointActivityService.GetAllByAppointmentId(ActiveAppointment.Id))
                {
                    var checkpoint = _checkpointService.GetById(checkpointActivity.CheckpointId);
                    var viewModel = CreateCheckpointCard(checkpointActivity, checkpoint);
                    checkpointCards.Add(viewModel);
                }
            }
            return checkpointCards;
        }

        private CheckpointCardViewModel CreateCheckpointCard(CheckpointActivity checkpointActivity, Checkpoint checkpoint)
        {
            var checkpointCard = new CheckpointCardViewModel
            {
                CheckpointId = checkpointActivity.CheckpointId,
                ActivityId = checkpointActivity.Id,
                Name = checkpoint.Name,
                Type = checkpoint.Type,
                StatusEnum = checkpointActivity.Status,
                CanShowAttendance = true,
            };

            checkpointCard.SetStatusAndBackground();
            checkpointCard.SetCanShowAttendance();
            return checkpointCard;
        }
    }
}
