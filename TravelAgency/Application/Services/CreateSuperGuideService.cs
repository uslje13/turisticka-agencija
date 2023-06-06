using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text.pdf.hyphenation;
using SOSTeam.TravelAgency.Domain.Models;

namespace SOSTeam.TravelAgency.Application.Services
{
    public class CreateSuperGuideService
    {
        private readonly AppointmentService _appointmentService;
        private readonly TourService _tourService;
        private readonly TourReviewService _tourReviewService;
        private readonly SuperGuideService _superGuideService;

        public CreateSuperGuideService()
        {
            _appointmentService = new AppointmentService();
            _tourService = new TourService();
            _tourReviewService = new TourReviewService();
            _superGuideService = new SuperGuideService();
        }

        public void CreateSuperTourGuide()
        {
            //Sacuvati super vodica
        }

        private class AvgStruct
        {
            public double AvgGrade { get; set; }
            public int NumOfAppointments { get; set; }

            public AvgStruct()
            {
                this.AvgGrade = 0;
                this.NumOfAppointments = 0;
            }
        }

        private DateRange CreateLastYearDateRange()
        {
            var now = DateTime.Now;

            var lastYearStart = new DateTime(now.AddYears(-1).Year, 1, 1);
            var lastYearEnd = new DateTime(now.AddYears(-1).Year, 12, 31);

            return new DateRange(lastYearStart, lastYearEnd);
        }


        private List<string> FindAllLanguagesInLastYear()
        {
            var languages = new List<string>();

            var appointmentsInLastYear = GetAllAppointmentsInLastYear();

            foreach (var appointment in appointmentsInLastYear)
            {
                languages.Add(_tourService.GetById(appointment.TourId).Language);
            }

            var languagesDistinct = new List<string>(languages.Distinct());

            return languagesDistinct;
        }

        private List<Appointment> GetAllAppointmentsInLastYear()
        {
            var lastYear = CreateLastYearDateRange();

            var appointmentsInLastYear = _appointmentService.GetAllFinishedByUserId(App.LoggedUser.Id).FindAll(a =>
                a.Start >= lastYear.Start &&
                a.Start <= lastYear.End);

            return appointmentsInLastYear;
        }

        private Dictionary<string, int> GetNumOfAppointmentsPerLanguage()
        {
            var numOfAppointmentsPerLanguage = new Dictionary<string, int>();
            var languagesInLastYear = FindAllLanguagesInLastYear();

            foreach (var language in languagesInLastYear)
            {
                int numOfAppointments = 0;
                foreach (var appointment in GetAllAppointmentsInLastYear())
                {
                    if (language == _tourService.GetById(appointment.TourId).Language)
                    {
                        numOfAppointments++;
                    }
                }

                numOfAppointmentsPerLanguage.Add(language, numOfAppointments);
            }

            return numOfAppointmentsPerLanguage;
        }

        private Dictionary<string, int> FilterNumOfAppointmentsPerLanguage()
        {
            var numOfAppointmentsPerLanguage = GetNumOfAppointmentsPerLanguage();
            var filteredNumOfAppointmentsPerLanguage = numOfAppointmentsPerLanguage.Where(kv => kv.Value >= 20).ToDictionary(kv => kv.Key, kv => kv.Value);

            return filteredNumOfAppointmentsPerLanguage;
        }

        private List<string> GetSuperGuideLanguages()
        {
            
            var filteredAppointmentsPerLanguage = FilterNumOfAppointmentsPerLanguage();

            var potentialLanguage = filteredAppointmentsPerLanguage.Keys;

            var appointmentsInLastYear = GetAllAppointmentsInLastYear();

            var avgGradesPerLanguage = new Dictionary<string, AvgStruct>();

            //Lista jezika i njihova prosecna ocena
            foreach (var language in potentialLanguage)
            {
                avgGradesPerLanguage.Add(language, new AvgStruct());
            }

            foreach (var language in avgGradesPerLanguage.Keys)
            {
                foreach (var appointment in appointmentsInLastYear)
                {
                    if (language == _tourService.GetById(appointment.TourId).Language)
                    {
                        // Prosek ocene za jedan termin za termin
                        double avgGrade = GetAppointmentAvgGrade(appointment);
                        var tempAvgGradePerLanguage = avgGradesPerLanguage[language];
                        tempAvgGradePerLanguage.AvgGrade += avgGrade;
                        tempAvgGradePerLanguage.NumOfAppointments++;
                    }
                }
            }

            var avgGradePerLanguage = GetAvgGradePerLanguage(avgGradesPerLanguage);

            var filteredAvgGradePerLanguage = avgGradePerLanguage.Where(kv => kv.Value >= 9.0)
                .ToDictionary(kv => kv.Key, kv => kv.Value);

            var superGuideLanguages = filteredAvgGradePerLanguage.Keys.ToList();

            return superGuideLanguages;
        }

        private Dictionary<string, double> GetAvgGradePerLanguage(Dictionary<string, AvgStruct> gradesPerLanguage)
        {
            var avgGradesPerLanguage = new Dictionary<string, double>();

            foreach (var language in gradesPerLanguage.Keys)
            {
                double sumOfGrades = gradesPerLanguage[language].AvgGrade;
                int numOfAppointments = gradesPerLanguage[language].NumOfAppointments;
                avgGradesPerLanguage.Add(language, sumOfGrades/numOfAppointments);
            }

            return avgGradesPerLanguage;

        }

        private double GetAppointmentAvgGrade(Appointment appointment)
        {
            double appointmentGradeAvg = 0;
            foreach (var review in _tourReviewService.GetAllByAppointmentId(appointment.Id))
            {
                int sumOfGrades = 0;
                sumOfGrades += review.GuideLanguage;
                sumOfGrades += review.GuideKnowledge;
                sumOfGrades += review.InterestRating;

                appointmentGradeAvg = sumOfGrades / 3.0;

                
            }
            return appointmentGradeAvg;
        }

    }
}
