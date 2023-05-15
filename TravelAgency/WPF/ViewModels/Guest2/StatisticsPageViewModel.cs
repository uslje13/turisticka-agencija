using LiveCharts;
using LiveCharts.Wpf;
using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest2
{
    public class StatisticsPageViewModel : ViewModel
    {
        public User LoggedInUser { get; set; }

        public SeriesCollection RequestsPercentagePerYear { get; set; }
        public SeriesCollection RequestsPercentage { get; set; }
        public float AcceptedRequestsPercentage { get; set; }
        public float InvalidRequestsPercentage { get; set; }
        public float AverageTouristNum { get; set; }

        private float _acceptedRequestsPercentageByYear;
        public float AcceptedRequestsPercentageByYear
        {
            get { return _acceptedRequestsPercentageByYear; }
            set
            {
                _acceptedRequestsPercentageByYear = value;
                OnPropertyChanged(nameof(AcceptedRequestsPercentageByYear));
            }
        }

        private float _invalidRequestsPercentageByYear;
        public float InvalidRequestsPercentageByYear
        {
            get { return _invalidRequestsPercentageByYear; }
            set
            {
                _invalidRequestsPercentageByYear = value;
                OnPropertyChanged(nameof(InvalidRequestsPercentageByYear));
            }
        }

        private float _averageTouristNumByYear;
        public float AverageTouristNumByYear
        {
            get { return _averageTouristNumByYear; }
            set
            {
                _averageTouristNumByYear = value;
                OnPropertyChanged(nameof(AverageTouristNumByYear));
            }
        }
        public ObservableCollection<int> AvailableYears { get; set; }

        private int _selectedYear;

        public int SelectedYear
        {
            get { return _selectedYear; }
            set
            {
                _selectedYear = value;
                GenerateStatisticsByYear(SelectedYear);
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

        private SeriesCollection _tourRequestsByLocation;
        public SeriesCollection TourRequestsByLocation
        {
            get { return _tourRequestsByLocation; }
            set
            {
                _tourRequestsByLocation = value;
                OnPropertyChanged(nameof(TourRequestsByLocation));
            }
        }

        private TourRequestService _tourRequestService;

        public StatisticsPageViewModel(User loggedInUser)
        {
            RequestsPercentagePerYear = new SeriesCollection();
            RequestsPercentage = new SeriesCollection();
            TourRequestsByLanguage = new SeriesCollection();
            TourRequestsByLocation = new SeriesCollection();
            LoggedInUser = loggedInUser;
            _tourRequestService = new TourRequestService();
            AvailableYears = new ObservableCollection<int>();
            SelectedYear = 2023;
            GetAvailableYears();
            GenerateGeneralStatistics();
            GenerateStatisticsByYear(SelectedYear);
            GenerateTourRequestsByLanguage();
            GenerateTourRequestsByLocation();
        }

        private void GetAvailableYears()
        {
            ObservableCollection<int> availableYears = new ObservableCollection<int>();

            foreach (var request in _tourRequestService.GetAll())
            {
                availableYears.Add(request.MaintenanceStartDate.Year);
                availableYears.Add(request.MaintenanceEndDate.Year);
            }

            var distinctYears = availableYears.Distinct().ToList();

            availableYears.Clear();

            foreach (var year in distinctYears)
            {
                AvailableYears.Add(year);
            }
        }

        private SeriesCollection GenerateChartData(Dictionary<string, int> dataCounts)
        {
            SeriesCollection series = new SeriesCollection();

            var labels = dataCounts.Keys.ToArray();
            var values = dataCounts.Values.ToArray();

            var columnSeries = new ColumnSeries
            {
                Values = new ChartValues<int>(values),
                DataLabels = true
            };

            Axis axis = new Axis
            {
                Labels = labels
            };

            series.Add(columnSeries);

            return series;
        }

        private void GenerateTourRequestsByLocation()
        {
            Dictionary<string, int> locationCounts = new Dictionary<string, int>();

            foreach (var request in _tourRequestService.GetAll())
            {
                if(request.UserId == LoggedInUser.Id)
                {
                    string location = request.City + ", " + request.Country;

                    if (locationCounts.ContainsKey(location))
                        locationCounts[location]++;
                    else
                        locationCounts[location] = 1;
                }
            }

            TourRequestsByLocation = GenerateChartData(locationCounts);
        }

        private void GenerateTourRequestsByLanguage()
        {
            Dictionary<string, int> languageCounts = new Dictionary<string, int>();

            foreach (var request in _tourRequestService.GetAll())
            {
                string language = request.Language;

                if (languageCounts.ContainsKey(language))
                    languageCounts[language]++;
                else
                    languageCounts[language] = 1;
            }

            TourRequestsByLanguage = GenerateChartData(languageCounts);
        }
        private void GenerateStatistics(int selectedYear = 0)
        {
            int acceptedRequests = 0;
            int invalidRequests = 0;
            int totalTouristNum = 0;
            int userRequestsNum = 0;

            foreach (var request in _tourRequestService.GetAll())
            {
                if (request.UserId == LoggedInUser.Id && (selectedYear == 0 || request.MaintenanceStartDate.Year == selectedYear || request.MaintenanceEndDate.Year == selectedYear))
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

            if(selectedYear== 0)
            {
                RequestsPercentage = new SeriesCollection
                {
                    new PieSeries
                    {
                        Title = "Prihvaceni zahtevi",
                        Values = new ChartValues<float> { AcceptedRequestsPercentage },
                        DataLabels = true
                    },
                    new PieSeries
                    {
                        Title = "Odbijeni zahtevi",
                        Values = new ChartValues<float> { InvalidRequestsPercentage },
                        DataLabels = true
                    }
                };
            }
            else
            {
                RequestsPercentagePerYear = new SeriesCollection
                {
                    new PieSeries
                    {
                        Title = "Prihvaceni zahtevi",
                        Values = new ChartValues<float> { AcceptedRequestsPercentageByYear },
                        DataLabels = true
                    },
                    new PieSeries
                    {
                        Title = "Odbijeni zahtevi",
                        Values = new ChartValues<float> { InvalidRequestsPercentageByYear },
                        DataLabels = true
                    }
                };
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
