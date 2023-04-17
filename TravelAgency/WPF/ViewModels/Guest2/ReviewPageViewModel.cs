using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.Views.Guest2;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest2
{
    public class ReviewPageViewModel : ViewModel
    {
        private ReviewPage _page;
        public int AppointmentId { get; set; }
        public int ReservationId { get; set; }
        public User LoggedInUser { get; set; }

        public List<Domain.Models.Image> Images { get; set; }

        private TourReviewService _tourReviewService;
        private ImageService _imageService;
        private ReservationService _reservationService;
        private string _imageUrl;
        public string ImageUrl
        {
            get { return _imageUrl; }
            set
            {
                _imageUrl = value;
                OnPropertyChanged(nameof(ImageUrl));
            }
        }

        private RelayCommand _backCommand;
        public RelayCommand BackCommand
        {
            get { return _backCommand; }
            set
            {
                _backCommand = value;
            }
        }

        private RelayCommand _addImageCommand;
        public RelayCommand AddImageCommand
        {
            get { return _addImageCommand; }
            set
            {
                _addImageCommand = value;
            }
        }

        private RelayCommand _createReviewCommand;

        public RelayCommand CreateReviewCommand
        {
            get { return _createReviewCommand; }
            set
            {
                _createReviewCommand = value;
            }
        }

        private bool _isKnowledgeOption1Selected;

        public bool IsKnowledgeOption1Selected
        {
            get { return _isKnowledgeOption1Selected; }
            set
            {
                _isKnowledgeOption1Selected = value;
                OnPropertyChanged(nameof(IsKnowledgeOption1Selected));
            }
        }

        private bool _isKnowledgeOption2Selected;

        public bool IsKnowledgeOption2Selected
        {
            get { return _isKnowledgeOption2Selected; }
            set
            {
                _isKnowledgeOption2Selected = value;
                OnPropertyChanged(nameof(IsKnowledgeOption2Selected));
            }
        }

        private bool _isKnowledgeOption3Selected;

        public bool IsKnowledgeOption3Selected
        {
            get { return _isKnowledgeOption3Selected; }
            set
            {
                _isKnowledgeOption3Selected = value;
                OnPropertyChanged(nameof(IsKnowledgeOption3Selected));
            }
        }

        private bool _isKnowledgeOption4Selected;

        public bool IsKnowledgeOption4Selected
        {
            get { return _isKnowledgeOption4Selected; }
            set
            {
                _isKnowledgeOption4Selected = value;
                OnPropertyChanged(nameof(IsKnowledgeOption4Selected));
            }
        }

        private bool _isKnowledgeOption5Selected;

        public bool IsKnowledgeOption5Selected
        {
            get { return _isKnowledgeOption5Selected; }
            set
            {
                _isKnowledgeOption5Selected = value;
                OnPropertyChanged(nameof(IsKnowledgeOption5Selected));
            }
        }

        private int _guideKnowledge;
        public int GuideKnowledge
        {
            get { return _guideKnowledge; }
            set
            {
                _guideKnowledge = value;
                OnPropertyChanged(nameof(GuideKnowledge));
            }
        }

        private bool _isLanguageOption1Selected;
        public bool IsLanguageOption1Selected
        {
            get { return _isLanguageOption1Selected; }
            set
            {
                _isLanguageOption1Selected = value;
                OnPropertyChanged(nameof(IsLanguageOption1Selected));
            }
        }

        private bool _isLanguageOption2Selected;

        public bool IsLanguageOption2Selected
        {
            get { return _isLanguageOption2Selected; }
            set
            {
                _isLanguageOption2Selected = value;
                OnPropertyChanged(nameof(IsLanguageOption2Selected));
            }
        }

        private bool _isLanguageOption3Selected;

        public bool IsLanguageOption3Selected
        {
            get { return _isLanguageOption3Selected; }
            set
            {
                _isLanguageOption3Selected = value;
                OnPropertyChanged(nameof(IsLanguageOption3Selected));
            }
        }

        private bool _isLanguageOption4Selected;

        public bool IsLanguageOption4Selected
        {
            get { return _isLanguageOption4Selected; }
            set
            {
                _isLanguageOption4Selected = value;
                OnPropertyChanged(nameof(IsLanguageOption4Selected));
            }
        }

        private bool _isLanguageOption5Selected;

        public bool IsLanguageOption5Selected
        {
            get { return _isLanguageOption5Selected; }
            set
            {
                _isLanguageOption5Selected = value;
                OnPropertyChanged(nameof(IsLanguageOption5Selected));
            }
        }

        private int _guideLangguage;
        public int GuideLanguage
        {
            get { return _guideLangguage; }
            set
            {
                _guideLangguage = value;
                OnPropertyChanged();
            }
        }

        private bool _isInterestOption1Selected;
        public bool IsInterestOption1Selected
        {
            get { return _isInterestOption1Selected; }
            set
            {
                _isInterestOption1Selected = value;
                OnPropertyChanged(nameof(IsInterestOption1Selected));
            }
        }

        private bool _isInterestOption2Selected;

        public bool IsInterestOption2Selected
        {
            get { return _isInterestOption2Selected; }
            set
            {
                _isInterestOption2Selected = value;
                OnPropertyChanged(nameof(IsInterestOption2Selected));
            }
        }

        private bool _isInterestOption3Selected;

        public bool IsInterestOption3Selected
        {
            get { return _isInterestOption3Selected; }
            set
            {
                _isInterestOption3Selected = value;
                OnPropertyChanged(nameof(IsInterestOption3Selected));
            }
        }

        private bool _isInterestOption4Selected;

        public bool IsInterestOption4Selected
        {
            get { return _isInterestOption4Selected; }
            set
            {
                _isInterestOption4Selected = value;
                OnPropertyChanged(nameof(IsInterestOption4Selected));
            }
        }

        private bool _isInterestOption5Selected;

        public bool IsInterestOption5Selected
        {
            get { return _isInterestOption5Selected; }
            set
            {
                _isInterestOption5Selected = value;
                OnPropertyChanged(nameof(IsInterestOption5Selected));
            }
        }

        private int _interestRating;
        public int InterestRating
        {
            get { return _interestRating; }
            set
            {
                _interestRating = value;
                OnPropertyChanged();
            }
        }

        private string _comment;
        public string Comment
        {
            get { return _comment; }
            set
            {
                _comment = value;
                OnPropertyChanged();
            }
        }


        public ReviewPageViewModel(ReviewPage page, User loggedInUser,int appointmnetId,int reservationId)
        {
            _page = page;
            AppointmentId= appointmnetId;
            LoggedInUser = loggedInUser;
            ReservationId= reservationId;
            Images = new List<Domain.Models.Image>();
            _tourReviewService = new TourReviewService();
            _imageService = new ImageService();
            _reservationService = new ReservationService();
            BackCommand = new RelayCommand(Execute_BackCommand,CanExecuteMethod);
            CreateReviewCommand = new RelayCommand(Execute_CreateReviewCommand, CanExecuteMethod);
            AddImageCommand = new RelayCommand(Execute_AddImageCommand, CanExecuteMethod);
        }

        private void Execute_AddImageCommand(object obj)
        {
            Domain.Models.Image image = new Domain.Models.Image(ImageUrl,false, AppointmentId, Domain.Models.Image.ImageType.GUEST2);
            Images.Add(image);
            ImageUrl = string.Empty;
        }

        private void Execute_CreateReviewCommand(object obj)
        {
            bool knowledgeFlag = KnowledgeRate();
            bool languageFlag = LanguageRate();
            bool interestFlag = InterestRate();
            bool commentFlag = true;

            if(Comment == null || Comment == string.Empty)
            {
                MessageBox.Show("Niste ostavili komentar");
                commentFlag = false;
            }

            if(knowledgeFlag && languageFlag && interestFlag && commentFlag)
            {
                MessageBoxResult result = ConfirmReview();
                if (result == MessageBoxResult.Yes)
                {
                    if(Images.Count > 0)
                    {
                        _imageService.SaveAll(Images);
                    }
                    _tourReviewService.CreateTourReview(LoggedInUser.Id, AppointmentId, GuideKnowledge, GuideLanguage, InterestRating, Comment, false);
                    _reservationService.Reviewed(ReservationId);
                    Window.GetWindow(_page).Close();
                }
            }
        }
        private MessageBoxResult ConfirmReview()
        {
            string sMessageBoxText = $"Da li ste sigurni da želite da ocenite turu i vodica";
            string sCaption = "Porvrda ocene";

            MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
            MessageBoxImage icnMessageBox = MessageBoxImage.Warning;

            MessageBoxResult result = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
            return result;
        }
        private void Execute_BackCommand(object obj)
        {
            NotificationsWindow window = new NotificationsWindow(LoggedInUser);
            window.Show();
            Window.GetWindow(_page).Close();
        }

        private bool CanExecuteMethod(object parameter)
        {
            return true;
        }

        private bool KnowledgeRate()
        {
            if (IsKnowledgeOption1Selected)
            {
                GuideKnowledge = 1;
                return true;
            }
            else if (IsKnowledgeOption2Selected)
            {
                GuideKnowledge = 2;
                return true;
            }
            else if (IsKnowledgeOption3Selected)
            {
                GuideKnowledge = 3;
                return true;
            }
            else if (IsKnowledgeOption4Selected)
            {
                GuideKnowledge = 4;
                return true;
            }
            else if (IsKnowledgeOption5Selected)
            {
                GuideKnowledge = 5;
                return true;
            }
            else
            {
                MessageBox.Show("Niste dodelili ocenu za znanje vodica");
                return false;
            }
        }

        private bool LanguageRate()
        {
            if (IsLanguageOption1Selected)
            {
                GuideLanguage = 1;
                return true;
            }
            else if (IsLanguageOption2Selected)
            {
                GuideLanguage = 2;
                return true;
            }
            else if (IsLanguageOption3Selected)
            {
                GuideLanguage = 3;
                return true;
            }
            else if (IsLanguageOption4Selected)
            {
                GuideLanguage = 4;
                return true;
            }
            else if (IsLanguageOption5Selected)
            {
                GuideLanguage = 5;
                return true;
            }
            else
            {
                MessageBox.Show("Niste dodelili ocenu za jezik vodica");
                return false;
            }
        }

        private bool InterestRate()
        {
            if (IsInterestOption1Selected)
            {
                InterestRating = 1;
                return true;
            }
            else if (IsInterestOption2Selected)
            {
                InterestRating = 2;
                return true;
            }
            else if (IsInterestOption3Selected)
            {
                InterestRating = 3;
                return true;
            }
            else if (IsInterestOption4Selected)
            {
                InterestRating = 4;
                return true;
            }
            else if (IsInterestOption5Selected)
            {
                InterestRating = 5;
                return true;
            }
            else
            {
                MessageBox.Show("Niste dodelili ocenu za zanimljivost ture");
                return false;
            }
        }


    }
}
