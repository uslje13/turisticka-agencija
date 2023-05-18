using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOSTeam.TravelAgency.Domain.Models;

namespace SOSTeam.TravelAgency.Application.Services
{
    public class AppointmentScheduleService
    {
        private readonly AppointmentService _appointmentService;
        private readonly TourRequestService _tourRequestService;
        private readonly TourService _tourService;

        public AppointmentScheduleService(int tourRequestId)
        {
            _appointmentService = new AppointmentService();
            _tourRequestService = new TourRequestService();
            _tourService = new TourService();
        }

        public List<DateRange> CreateTourGuideSchedule(int tourRequestId, int durationInHours, int userId)
        {
            var availableDates = new List<DateRange>();

            var requiredDateRange = CreateRequireDateRange(tourRequestId);

            var appointmentsInDateRange =
                _appointmentService.GetScheduledAppointments(requiredDateRange.Start, requiredDateRange.End, userId);

            var busyDateRanges = GetBusyDateRanges(appointmentsInDateRange);        //ovi ce svakako biti zabranjeni

            var availableDateRanges = GetAvailableDateRanges(busyDateRanges);

            foreach (var availableDateRange in availableDateRanges)
            {
                var freeTime = availableDateRange.End - availableDateRange.Start;
                if (TimeSpan.FromHours(durationInHours) > freeTime)
                {
                    availableDateRanges.Remove(availableDateRange);
                }
            }

            availableDates = availableDateRanges;

            return availableDates;
        }

        private DateRange CreateRequireDateRange(int tourRequestId)
        {
            var tourRequest = _tourRequestService.GetById(tourRequestId);

            var requiredStartDate = new DateTime(tourRequest.MaintenanceStartDate.Year,
                tourRequest.MaintenanceStartDate.Month, tourRequest.MaintenanceStartDate.Day);

            var requiredEndDate = new DateTime(tourRequest.MaintenanceEndDate.Year,
                tourRequest.MaintenanceEndDate.Month, tourRequest.MaintenanceEndDate.Day);

            var requiredDateRange = new DateRange(requiredStartDate, requiredEndDate);


            return requiredDateRange;
        }

        private List<DateRange> GetBusyDateRanges(List<Appointment> appointments)
        {
            var busyDateRanges = new List<DateRange>();

            foreach (var appointment in appointments)
            {
                int appointmentDuration = _tourService.GetById(appointment.TourId).Duration;
                var endDate = appointment.Start.AddDays(appointmentDuration);
                var dateRange = new DateRange(appointment.Start, endDate);
                busyDateRanges.Add(dateRange);
            }

            return busyDateRanges;
        }

        private List<DateRange> GetAvailableDateRanges(List<DateRange> busyDateRanges)
        {
            var availableDateRanges = new List<DateRange>();

            for (int i = 0; i < busyDateRanges.Count - 1; i++)
            {
                var currentEndDate = busyDateRanges[i].End;
                var nextStartDate = busyDateRanges[i + 1].Start;
                var freeDateRange = new DateRange(currentEndDate, nextStartDate);
                availableDateRanges.Add(freeDateRange);
            }

            return availableDateRanges;
        }

    }
}
