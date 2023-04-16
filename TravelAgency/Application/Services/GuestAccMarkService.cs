using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Domain.RepositoryInterfaces;
using SOSTeam.TravelAgency.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using SOSTeam.TravelAgency.WPF.ViewModels.Guest1;
using SOSTeam.TravelAgency.Domain.Models;

namespace SOSTeam.TravelAgency.Application.Services
{
    public class GuestAccMarkService
    {
        private readonly IGuestAccommodationMarkRepository _guestAccommodationMarkRepository = Injector.CreateInstance<IGuestAccommodationMarkRepository>();
        private readonly IAccReservationRepository _accReservationRepository = Injector.CreateInstance<IAccReservationRepository>();
        private readonly IImageRepository _imageRepository = Injector.CreateInstance<IImageRepository>();

        public GuestAccMarkService() { }

        public void MarkAccommodation(int cleanMark, int ownerMark, string comment, string urls, User user, CancelAndMarkResViewModel acc)
        {
            MakeAndSaveMark(cleanMark, ownerMark, comment, urls, user, acc);
            SaveChangesToCSVs(acc);
            SaveImages(urls, acc.ReservationId);
        }

        private void SaveChangesToCSVs(CancelAndMarkResViewModel acc)
        {
            List<AccommodationReservation> finishedReservations = _accReservationRepository.LoadFinishedReservations();
            foreach (var item in finishedReservations)
            {
                if (item.Id == acc.ReservationId)
                {
                    item.ReadMarkNotification = true;
                    _accReservationRepository.UpdateFinishedReservationsCSV(item);
                    _accReservationRepository.Update(item);
                }
            }
        }

        private void MakeAndSaveMark(int cleanMark, int ownerMark, string comment, string urls, User user, CancelAndMarkResViewModel acc)
        {
            GuestAccommodationMark accommodationMarks = new GuestAccommodationMark(cleanMark, ownerMark, comment, urls, user.Id, acc.AccommodationId);
            if (accommodationMarks.UrlGuestImage.Equals(""))
                accommodationMarks.UrlGuestImage = "Nema priloženih slika.";
            _guestAccommodationMarkRepository.Save(accommodationMarks);
        }

        private void SaveImages(string urls, int resId)
        {
            if(!urls.Equals(""))
            {
                string[] urlArray = urls.Split(',');
                foreach(string url in urlArray) 
                {
                    Domain.Models.Image image = new Domain.Models.Image(url, false, resId, Domain.Models.Image.ImageType.RESERVATION);
                    _imageRepository.Save(image);
                }
            }
        }

        public void Delete(int id)
        {
            _guestAccommodationMarkRepository.Delete(id);
        }

        public List<GuestAccommodationMark> GetAll()
        {
            return _guestAccommodationMarkRepository.GetAll();
        }


        public GuestAccommodationMark GetById(int id)
        {
            return _guestAccommodationMarkRepository.GetById(id);
        }

        public void Save(GuestAccommodationMark guestAccommodationMark)
        {
            _guestAccommodationMarkRepository.Save(guestAccommodationMark);
        }

        public void Update(GuestAccommodationMark guestAccommodationMark)
        {
            _guestAccommodationMarkRepository.Update(guestAccommodationMark);
        }
    }
}
