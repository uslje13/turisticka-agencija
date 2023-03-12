using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Model;
using TravelAgency.Serializer;

namespace TravelAgency.Repository
{
    public class GuestReviewRepository
    {
        private const string FilePath = "../../../Resources/Data/guestReview.csv";
        private readonly Serializer<GuestReview> _serializer;
        private List<GuestReview> _guestReviews;

        public GuestReviewRepository()
        {
            _serializer = new Serializer<GuestReview>();
            _guestReviews = new List<GuestReview>();
        }

        public List<GuestReview> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public GuestReview Save(GuestReview guestReview)
        {
            guestReview.Id = NextId();
            _guestReviews = _serializer.FromCSV(FilePath);
            _guestReviews.Add(guestReview);
            _serializer.ToCSV(FilePath, _guestReviews);
            return guestReview;
        }

        public void Delete(GuestReview guestReview)
        {
            _guestReviews = _serializer.FromCSV(FilePath);
            GuestReview found = _guestReviews.Find(t => t.Id == guestReview.Id) ?? throw new ArgumentException();
            _guestReviews.Remove(found);
            _serializer.ToCSV(FilePath, _guestReviews);
        }

        public GuestReview Update(GuestReview guestReview)
        {
            _guestReviews = _serializer.FromCSV(FilePath);
            GuestReview current = _guestReviews.Find(t => t.Id == guestReview.Id) ?? throw new ArgumentException();
            int index = _guestReviews.IndexOf(current);
            _guestReviews.Remove(current);
            _guestReviews.Insert(index, guestReview);
            _serializer.ToCSV(FilePath, _guestReviews);
            return guestReview;
        }
        public int NextId()
        {
            _guestReviews = _serializer.FromCSV(FilePath);
            if (_guestReviews.Count < 1)
            {
                return 1;
            }
            return _guestReviews.Max(l => l.Id) + 1;
        }

    }
}
