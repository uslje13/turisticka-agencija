using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Model;
using TravelAgency.Serializer;

namespace TravelAgency.Repository
{
    public class CheckpointActivityRepository
    {
        private const string FilePath = "../../../Resources/Data/checkpointActivities.csv";

        private readonly Serializer<CheckpointActivity> _serializer;

        private List<CheckpointActivity> _checkpointActivities;

        public CheckpointActivityRepository()
        {
            _serializer = new Serializer<CheckpointActivity>();
            _checkpointActivities = _serializer.FromCSV(FilePath);
        }

        public List<CheckpointActivity> GetAll()
        {
            return _checkpointActivities;
        }

        public void Save(CheckpointActivity checkpointActivity)
        {
            checkpointActivity.Id = NextId();
            _checkpointActivities = _serializer.FromCSV(FilePath);
            _checkpointActivities.Add(checkpointActivity);
            _serializer.ToCSV(FilePath, _checkpointActivities);
        }

        public void SaveAll(List<CheckpointActivity> checkpointActivities)
        {
            foreach (CheckpointActivity checkpointActivity in checkpointActivities)
            {
                Save(checkpointActivity);
            }
        }

        public void Update(CheckpointActivity checkpointActivity) 
        {
            _checkpointActivities = _serializer.FromCSV(FilePath);
            CheckpointActivity current = _checkpointActivities.Find(c => c.Id == checkpointActivity.Id) ?? throw new ArgumentException();
            int index = _checkpointActivities.IndexOf(current);
            _checkpointActivities.Remove(current);
            _checkpointActivities.Insert(index, checkpointActivity);
            _serializer.ToCSV(FilePath, _checkpointActivities);
        }


        public int NextId()
        {
            _checkpointActivities = _serializer.FromCSV(FilePath);
            if (_checkpointActivities.Count < 1)
            {
                return 1;
            }
            return _checkpointActivities.Max(d => d.Id) + 1;
        }
    }
}
