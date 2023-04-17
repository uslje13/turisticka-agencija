using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOSTeam.TravelAgency.Domain;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Domain.RepositoryInterfaces;

namespace SOSTeam.TravelAgency.Application.Services
{
    public class CheckpointActivityService
    {
        private readonly ICheckpointActivityRepository _checkpointActivityRepository = Injector.CreateInstance<ICheckpointActivityRepository>();
        public CheckpointActivityService() { }

        public void Delete(int id)
        {
            _checkpointActivityRepository.Delete(id);
        }

        public List<CheckpointActivity> GetAll()
        {
            return _checkpointActivityRepository.GetAll();
        }

        public List<CheckpointActivity> GetAllByAppointmentId(int id)
        {
            return _checkpointActivityRepository.GetAllByAppointmentId(id);
        }

        public CheckpointActivity GetById(int id)
        {
            return _checkpointActivityRepository.GetById(id);
        }

        public void Save(CheckpointActivity activity)
        {
            _checkpointActivityRepository.Save(activity);
        }

        public void SaveAll(List<CheckpointActivity> activities)
        {
            _checkpointActivityRepository.SaveAll(activities);
        }

        public void Update(CheckpointActivity activity)
        {
            _checkpointActivityRepository.Update(activity);
        }

        public CheckpointActivity FindActiveCheckpoint(int appointmentId)
        {
            foreach (CheckpointActivity checkpointActivity in _checkpointActivityRepository.GetAll())
            {
                if (checkpointActivity.AppointmentId == appointmentId && checkpointActivity.Status == CheckpointStatus.ACTIVE)
                {
                    return checkpointActivity;
                }
            }
            return null;
        }
    }
}
