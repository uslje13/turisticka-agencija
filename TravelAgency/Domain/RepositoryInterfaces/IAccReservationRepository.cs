using SOSTeam.TravelAgency.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SOSTeam.TravelAgency.Domain.RepositoryInterfaces
{
    public interface IAccReservationRepository : IRepository<AccommodationReservation>
    {
        void SaveToOtherCSV(AccommodationReservation reservation);
        void DeleteFromOtherCSV(AccommodationReservation reservation);
        void SaveCanceledReservation(AccommodationReservation reservation);
        List<AccommodationReservation> LoadCanceledReservations();
        List<AccommodationReservation> LoadFromOtherCSV();
        void SaveChangeAcceptedReservation(AccommodationReservation reservation);
    }
}
