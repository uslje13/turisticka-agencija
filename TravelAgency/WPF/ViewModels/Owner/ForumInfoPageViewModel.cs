using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Owner
{
    public class ForumInfoPageViewModel : ViewModel
    {
        public string OpenerUsername { get; set; }
        public string Location { get; set; }
        private string _newComment;
        public string NewComment
        {
            get { return _newComment; }
            set
            {
                if (_newComment != value)
                {
                    _newComment = value;
                    OnPropertyChanged("NewComment");
                }
            }
        }
        public List<ForumComment> TestComments { get; set; }
        public Forum ForumInfo { get; set; }
        public RelayCommand AddComment { get; private set; }

        private ObservableCollection<ForumCommentViewModel> _comments;
        public ObservableCollection<ForumCommentViewModel> Comments
        {
            get { return _comments; }
            set
            {
                _comments = value;
                OnPropertyChanged("Comments");
            }
        }

        private UserService _userService;
        private LocationService _locationService;
        private ForumCommentReportService _forumCommentReportService;
        public ForumInfoPageViewModel(Forum forum)
        {
            _userService = new();
            _locationService = new();
            _forumCommentReportService = new();

            ForumInfo = forum;
            OpenerUsername = _userService.GetById(forum.UserId).Username;
            var location = _locationService.GetById(forum.LocationId);
            Location = _locationService.GetFullName(location);
            NewComment = "";

            TestComments = new List<ForumComment>();
            AddTestComments();
            FillObservableCollection();
            AddComment = new RelayCommand(Execute_AddComment, CanExecuteAddComment);
        }

        private void Execute_AddComment(object obj)
        {
            //TODO: DOdaj cuvanje
            var forumComment = new ForumComment(
                0,
                App.LoggedUser.Id,
                ForumInfo.Id,
                NewComment,
                false,
                Roles.OWNER
                );
            TestComments.Add(forumComment);
            Comments.Add(new ForumCommentViewModel(
                    _userService.GetById(forumComment.UserId).Username,
                    forumComment,
                    0,
                    false
                    ));
            NewComment = "";
        }

        private bool CanExecuteAddComment(object obj)
        {
            return NewComment.Length > 1; 
        }

        private void FillObservableCollection()
        {
            Comments = new();
            foreach(var comment in TestComments) 
            {
                Comments.Add(new ForumCommentViewModel(
                    _userService.GetById(comment.Id).Username,
                    comment,
                    0,
                    false
                    ));
            }
        }

        private void AddTestComments()
        {
            ForumComment comment1 = new ForumComment(1, 1, 1, "Brat moj dobri posteni najposteniji.", true, Roles.OWNER);
            ForumComment comment2 = new ForumComment(2, 2, 1, "Nisam bio tu al je Loooooseeeeeeeeeee xD.", false, Roles.GUEST1);
            ForumComment comment3 = new ForumComment(3, 3, 1, "IDE GAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAS IDE GASSSSSSSSSSS IDE GAAAAAAAAAAAAAAAAAAAAAAS IDE GAAAAAAAAAAAAAAAAAS.", true, Roles.GUEST2);
            ForumComment comment4 = new ForumComment(4, 4, 2, "And here's one more comment.", false, Roles.TOURISTGUIDE);

            TestComments.Add(comment1);
            TestComments.Add(comment2);
            TestComments.Add(comment3);
            TestComments.Add(comment4);
        }
    }

    public class ForumCommentViewModel 
    {
        public string Username { get; set; }
        public ForumComment Comment { get; set; }
        public int ReportNum { get; set; }
        public bool IsReported { get; set; }

        public ForumCommentViewModel(string username, ForumComment comment, int reportNum, bool isReported)
        {
            Username = username;
            Comment = comment;
            ReportNum = reportNum;
            IsReported = isReported;
        }
    }
}
