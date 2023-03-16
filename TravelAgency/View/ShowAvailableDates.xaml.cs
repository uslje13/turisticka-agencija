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
        public DateTime[] datesArray { get; set; }


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
            datesArray = new DateTime[100];

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
            AccReservationDTO accReservationDTO = new AccReservationDTO();
            List<AccReservationDTO> reservationsDTO = CreateAllDTOreservations();
            Calendar.BlackoutDates.AddDatesInPast();
            foreach (var item in reservationsDTO)
            {
                if(item.AccommodationId == accommodationDTO.AccommodationId)
                {
                    accReservationDTO = item;
                    MarkCalendar(item);
                }
            }
            
            CheckRequestedDates(EnteredFirstDay, EnteredLastDay, DaysDuration, accReservationDTO);
        }

        private void MarkCalendar(AccReservationDTO reservationDTO)
        {
            int[] ints = GetDateData(reservationDTO);
            DateTime item1 = new DateTime(ints[0], ints[1], ints[2]);
            DateTime item2 = new DateTime(ints[3], ints[4], ints[5]);
            Calendar.BlackoutDates.Add(new CalendarDateRange(item1, item2));
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

        private void CheckRequestedDates(DateTime firstDay, DateTime lastDay, int daysStay, AccReservationDTO reservationDTO)
        {
            int[] counterArray = FindFreeDaysInRow(firstDay, lastDay);
            int[] results = FindMaxCounter(counterArray);

            if(results[0] < daysStay)
            {
                //novi prozor sa ponudom termina jer rezervacija u datom opsegu za trazeni broj dana nije moguca
            }
            else if(results[0] == daysStay)
            {
                //pristupa se tom nizu i on se rezervise
                int index = results[1];
                //datesArray[index]
            } 
            else if(results[0] > daysStay)
            {
                //pristupa se tom nizu i on se rezervise za AddDays(daysStay)
            }
        }

        private void AddReservation(DateTime start, DateTime end, int days, int accId)
        {
            AccommodationReservation reservation = new AccommodationReservation(start, end, days, accId);
            AccommodationReservationRepository reservationRepository = new AccommodationReservationRepository();
            reservationRepository.Save(reservation);
            MessageBox.Show("Uspešno rezervisano.");
        }

        private int[] FindFreeDaysInRow(DateTime firstDay, DateTime lastDay)
        {
            CalendarBlackoutDatesCollection blackoutDates = Calendar.BlackoutDates;
            int[] counterArray = new int[100];
            int i = 0;
            int j = firstDay.DayOfYear;
            int k = lastDay.DayOfYear;
            DateTime firstJan = new DateTime(firstDay.Year, 1, 1);
            for (; j <= k; j++)
            {
                if (!blackoutDates.Contains(firstJan.AddDays(j-1)))
                {
                    counterArray[i]++;
                    //datesArray[i] = item;
                }
                else
                {
                    counterArray[++i]++;
                    //datesArray[++i] = item;
                }
            }

            return counterArray;
        }

        private int[] FindMaxCounter(int[] array)
        {
            int max = array[0];
            int maxIndex = 0;
            for(int i = 0; i < array.Length; i++)
            {
                if (array[i] > max)
                {
                    max = array[i];
                    maxIndex = i;
                }
            }

            int[] result = new int[2];
            result[0] = max;
            result[1] = maxIndex;
            
            return result;
        }
    }
}
