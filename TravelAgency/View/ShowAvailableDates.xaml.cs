using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TravelAgency.Model;
using TravelAgency.Repository;

namespace TravelAgency.View
{
    /// <summary>
    /// Interaction logic for ShowAvailableDates.xaml
    /// </summary>
    public partial class ShowAvailableDates : Window
    {
        public ObservableCollection<AccReservationDTO> reservationDTOList {  get; set; }
        public AccommodationRepository accommodationRepository { get; set; }
        public AccommodationReservationRepository reservationRepository { get; set; }
        public List<Accommodation> accommodations { get; set; }
        public List<AccommodationReservation> reservations { get; set; }
        public AccommodationDTO accommodationDTO { get; set; }
        public DateTime EnteredFirstDay { get; set; }
        public DateTime EnteredLastDay { get; set; }

        public int DaysDuration { get; set; }

        public ShowAvailableDates()
        {
            InitializeComponent();
        }

        public ShowAvailableDates(AccommodationDTO dto, DateTime firstDay, DateTime lastDay, int days)
        {
            InitializeComponent();
            DataContext = this;
            reservationDTOList = new ObservableCollection<AccReservationDTO>();
            accommodationRepository = new AccommodationRepository();
            reservationRepository = new AccommodationReservationRepository();
            
            accommodationDTO = dto;
            EnteredFirstDay = firstDay;
            EnteredLastDay = lastDay;
            DaysDuration = days;

            accommodations = accommodationRepository.GetAll();
            reservations = reservationRepository.GetAll();

            MarkCalendars();
        }

        private List<AccReservationDTO> CreateAllDTOreservations()
        {
            foreach (var accommodation in accommodations)
            {
                foreach (var reservation in reservations)
                {
                    if (accommodation.Id == reservation.AccommodationId)
                    {
                        AccReservationDTO Dto = CreateOneDTOreservation(accommodation, reservation);
                        reservationDTOList.Add(Dto);
                    }
                }
            }
            return reservationDTOList.ToList();
        }

        private AccReservationDTO CreateOneDTOreservation(Accommodation acc, AccommodationReservation res)
        {
            AccReservationDTO dto = new AccReservationDTO(acc.Id, acc.Name, acc.MinDaysStay, res.FirstDay, res.LastDay, res.ReservationDuration, acc.MaxGuests);
            return dto;
        }

        private void MarkCalendars()
        {
            List<AccReservationDTO> reservationsDTO = CreateAllDTOreservations();
            CalendarFirst.BlackoutDates.AddDatesInPast();
            CalendarSecond.BlackoutDates.AddDatesInPast();
            foreach (var item in reservationsDTO)
            {
                if(item.AccommodationId == accommodationDTO.AccommodationId)
                {
                    MarkFirstCalendar(item);
                    MarkSecondCalendar(item);
                    //sad ovdje treba uzeti u obzir zadati opseg datuma i duzinu dana i shodno tome napraviti rezervaciju
                }
            }
        }

        private void MarkFirstCalendar(AccReservationDTO reservationDTO)
        {
            int[] ints = GetDateData(reservationDTO);
            DateTime item1 = new DateTime(ints[0], ints[1], ints[2]);
            DateTime item2 = new DateTime(ints[3], ints[4], ints[5]);
            CalendarFirst.BlackoutDates.Add(new CalendarDateRange(item1, item2));
        }

        private void MarkSecondCalendar(AccReservationDTO reservationDTO)
        {
            int[] ints = GetDateData(reservationDTO);
            DateTime item1 = new DateTime(ints[0], ints[1], ints[2]);
            DateTime item2 = new DateTime(ints[3], ints[4], ints[5]);
            CalendarSecond.BlackoutDates.Add(new CalendarDateRange(item1, item2));
        }

        private int[] GetDateData(AccReservationDTO res)
        {
            int[] data = new int[6];
            data[0] = res.ReservationFirstDay.Year;
            data[1] = res.ReservationFirstDay.Month;
            data[2] = res.ReservationFirstDay.Day;

            data[3] = res.ReservationLastDay.Year;
            data[4] = res.ReservationLastDay.Month;
            data[5] = res.ReservationLastDay.Day;

            return data;
        }
    }
}
