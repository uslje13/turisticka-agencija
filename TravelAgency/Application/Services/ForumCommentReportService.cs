using SOSTeam.TravelAgency.Domain;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.Application.Services
{
    internal class ForumCommentReportService
    {
        private readonly IForumCommentReportRepository _forumCommentReportRepository = Injector.CreateInstance<IForumCommentReportRepository>();

        public ForumCommentReportService()
        {

        }

        public List<ForumCommentReport> GetAll()
        {
            return _forumCommentReportRepository.GetAll();
        }

        public void Save(ForumCommentReport forumCommentReport)
        {
            _forumCommentReportRepository.Save(forumCommentReport);
        }

        public void Delete(int id)
        {
            _forumCommentReportRepository.Delete(id);
        }

        public void Update(ForumCommentReport forumCommentReport)
        {
            _forumCommentReportRepository.Update(forumCommentReport);
        }

        public ForumCommentReport GetById(int id)
        {
            return _forumCommentReportRepository.GetById(id);
        }
    }

}
