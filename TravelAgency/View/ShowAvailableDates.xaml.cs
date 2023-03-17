﻿using System;
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
        public List<AccReservationDTO> dtoReservation { get; set; }


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
            dtoReservation = new List<AccReservationDTO>();

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
            
            CheckRequestedDates();
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

        private void CheckRequestedDates()
        {
            int[] daysCounter = FindFreeDaysInRow();
            int[] appropiatedIndexes = FindAppropiatedIndexes(daysCounter);
            int notOkeyCounter = 0;
            int okeyCounter = 0;

            foreach (int index in appropiatedIndexes)
            {
                if (index == -1)
                {
                    notOkeyCounter++;
                } else okeyCounter++;
            }

            if(notOkeyCounter == appropiatedIndexes.Length)
            {
                //novi prozor sa ponudom termina van zadatog opsega
                //jer rezervacija u datom opsegu za trazeni broj dana nije moguca
            }
            else
            {
                CreateFreeAppointmentsCatalog(daysCounter, appropiatedIndexes, okeyCounter);
            }
        }

        private void CreateFreeAppointmentsCatalog(int[] array, int[] appIndexes, int okCount)
        {
            int checkCounter = 0;
            int daysSum = 0;
            int j = 0;
            for(int i = 0;  i < array.Length; i++)
            {
                if(checkCounter < okCount)
                {
                    if (i == appIndexes[j])
                    {
                        if (array[i] > DaysDuration)
                        {
                            int diff = array[i] - DaysDuration;
                            int daysSumCopy = daysSum;
                            for (int k = 0; k < diff; k++)
                            {
                                CreateCatalogItem(daysSumCopy);
                                daysSumCopy++;
                            }
                        }
                        else
                        {
                            CreateCatalogItem(daysSum);
                        }
                        j++;
                        checkCounter++;
                    }
                    daysSum += array[i];
                }
            }
        }

        private void CreateCatalogItem(int daysSum)
        {
            DateTime firstComponent = EnteredFirstDay.AddDays(daysSum);
            DateTime secondComponent = firstComponent.AddDays(DaysDuration);
            AccReservationDTO dto = new AccReservationDTO(accommodationDTO.AccommodationId, accommodationDTO.AccommodationName,
                                                          accommodationDTO.AccommodationMinDaysStay, firstComponent, secondComponent,
                                                          DaysDuration, accommodationDTO.AccommodationMaxGuests);
            dtoReservation.Add(dto);
        }

        private int[] FindFreeDaysInRow()
        {
            CalendarBlackoutDatesCollection blackoutDates = Calendar.BlackoutDates;
            int[] counterArray = new int[100];
            int i = 0;
            int z = 0;
            bool flag = false;
            int j = EnteredFirstDay.DayOfYear;
            int k = EnteredLastDay.DayOfYear;
            DateTime firstJan = new DateTime(EnteredFirstDay.Year, 1, 1);
            for (; j <= k; j++)
            {
                if (!blackoutDates.Contains(firstJan.AddDays(j-1)))
                {
                    if(flag)
                    {
                        i++;
                    }
                    counterArray[i]++;
                    datesArray[z++] = firstJan.AddDays(j - 1);
                    flag = false;
                }
                else
                {
                    flag = true;
                    counterArray[++i]++;
                    datesArray[z++] = firstJan.AddDays(j-1);
                }
            }

            return counterArray;
        }

        private int[] FindAppropiatedIndexes(int[] array)
        {
            int[] result = new int[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                result[i] = -1;
            }

            int j = 0;
            for(int i = 0; i < array.Length; i++)
            {
                if (array[i] != 1)
                {
                    if (array[i] >= DaysDuration)
                    {
                        result[j++] = i;
                    }
                }
            }

            return result;
        }

        private void PickCatalogItemClick(object sender, RoutedEventArgs e)
        {
            SelectReservationDates selectReservationDates = new SelectReservationDates(dtoReservation);
            selectReservationDates.ShowDialog();
        }
    }
}