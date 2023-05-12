using LiveCharts;
using LiveCharts.Wpf;
using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest2
{
    public class StatisticsPageViewModel : ViewModel
    {
        public User LoggedInUser { get; set; }

        public float AcceptedRequestsPercentage { get; set; }
        public float InvalidRequestsPercentage { get; set; }
        public float AverageTouristNum { get; set; }
        public float AcceptedRequestsPercentageByYear { get; set; }
        public float InvalidRequestsPercentageByYear { get; set; }
        public float AverageTouristNumByYear { get; set; }

        private int _selectedYear;

        public int SelectedYear
        {
            get { return _selectedYear; }
            set
            {
                _selectedYear = value;
                OnPropertyChanged(nameof(SelectedYear));
            }
        }

        private SeriesCollection _tourRequestsByLanguage;
        public SeriesCollection TourRequestsByLanguage
        {
            get { return _tourRequestsByLanguage; }
            set
            {
                _tourRequestsByLanguage = value;
                OnPropertyChanged(nameof(TourRequestsByLanguage));
            }
        }

        private TourRequestService _tourRequestService;

        public StatisticsPageViewModel(User loggedInUser)
        {
            LoggedInUser = loggedInUser;
            _tourRequestService = new TourRequestService();
            SelectedYear = 2024;
            GenerateGeneralStatistics();
            GenerateStatisticsByYear(SelectedYear);
            GenerateTourRequestsByLanguage();
        }

        private void GenerateTourRequestsByLanguage()
        {
            Dictionary<string, int> languageCounts = new Dictionary<string, int>();

            // Iterate over your tour requests and count the requests by language
            foreach (var request in _tourRequestService.GetAll())
            {
                string language = request.Language;

                if (languageCounts.ContainsKey(language))
                    languageCounts[language]++;
                else
                    languageCounts[language] = 1;
            }

            // Create the chart series data
            SeriesCollection series = new SeriesCollection();
            foreach (var languageCount in languageCounts)
            {
                series.Add(new PieSeries
                {
                    Title = languageCount.Key,
                    Values = new ChartValues<int> { languageCount.Value },
                    DataLabels = true
                });
            }

            TourRequestsByLanguage = series;
        }
        private void GenerateStatistics(int selectedYear = 0)
        {
            int acceptedRequests = 0;
            int invalidRequests = 0;
            int totalTouristNum = 0;
            int userRequestsNum = 0;

            foreach (var request in _tourRequestService.GetAll())
            {
                if (request.UserId == LoggedInUser.Id && (selectedYear == 0 || request.MaintenanceStartDate.Year == selectedYear))
                {
                    if (request.Status == StatusType.ACCEPTED)
                    {
                        acceptedRequests++;
                        totalTouristNum += request.MaxNumOfGuests;
                    }
                    else if (request.Status == StatusType.INVALID)
                    {
                        invalidRequests++;
                    }

                    userRequestsNum++;
                }
            }

            if (selectedYear == 0)
            {
                AverageTouristNum = acceptedRequests == 0 ? 0 : totalTouristNum / acceptedRequests;
                AcceptedRequestsPercentage = userRequestsNum == 0 ? 0 : (float)acceptedRequests / userRequestsNum * 100;
                InvalidRequestsPercentage = userRequestsNum == 0 ? 0 : (float)invalidRequests / userRequestsNum * 100;
            }
            else
            {
                AverageTouristNumByYear = acceptedRequests == 0 ? 0 : totalTouristNum / acceptedRequests;
                AcceptedRequestsPercentageByYear = userRequestsNum == 0 ? 0 : (float)acceptedRequests / userRequestsNum * 100;
                InvalidRequestsPercentageByYear = userRequestsNum == 0 ? 0 : (float)invalidRequests / userRequestsNum * 100;
            }
        }

        private void GenerateStatisticsByYear(int selectedYear)
        {
            GenerateStatistics(selectedYear);
        }

        private void GenerateGeneralStatistics()
        {
            GenerateStatistics();
        }
    }
}
