﻿using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Domain.RepositoryInterfaces;
using SOSTeam.TravelAgency.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.Application.Services
{
    public class ComplexTourRequestService
    {
        private readonly IComplexTourRequestRepository _complexTourRequestRepository = Injector.CreateInstance<IComplexTourRequestRepository>();
        private readonly NewTourNotificationService _newTourNotificationService;
        private readonly TourRequestService _tourRequestService;

        public ComplexTourRequestService()
        {
            _newTourNotificationService = new NewTourNotificationService();
            _tourRequestService = new TourRequestService();
        }

        public void Delete(int id)
        {
            _complexTourRequestRepository.Delete(id);
        }

        public List<ComplexTourRequest> GetAll()
        {
            return _complexTourRequestRepository.GetAll();
        }

        public ComplexTourRequest GetById(int id)
        {
            return _complexTourRequestRepository.GetById(id);
        }

        public void Save(ComplexTourRequest tourRequest)
        {
            _complexTourRequestRepository.Save(tourRequest);
        }

        public void Update(ComplexTourRequest tourRequest)
        {
            _complexTourRequestRepository.Update(tourRequest);
        }

        public List<ComplexTourRequest> GetAllNotAcceptedOnePart()
        {
            var complexTourRequests = GetAll().FindAll(ctr => ctr.Status == StatusType.ON_HOLD);
            var requestsToRemove = new List<ComplexTourRequest>();

            foreach (var complexTourRequest in complexTourRequests)
            {
                if (IsRequestExistInNotification(complexTourRequest.Id))
                {
                    requestsToRemove.Add(complexTourRequest);
                }
            }

            foreach (var requestToRemove in requestsToRemove)
            {
                complexTourRequests.Remove(requestToRemove);
            }

            return complexTourRequests;
        }
        private bool IsRequestExistInNotification(int complexTourId)
        {
            //Prolazim kroz sva obavestenja koja je kreirao vodic
            foreach (var notification in _newTourNotificationService.GetAllByGuideId(App.LoggedUser.Id))
            {
                //Prodjem kroz sve zahteve za ture koji pripadaju kompleksnom zahtevu i koji su na prihvaceni
                var temp = _tourRequestService.GetAll()
                    .FindAll(tr => tr.ComplexTourRequestId == complexTourId && tr.Status == StatusType.ACCEPTED);
                foreach (var tourRequest in temp)
                {
                    //Ako se poklopi id tog dela zahteva sa obavestenjem koje je vodic napravio znaci da je vec prihvatio jedan deo ove ture
                    if (notification.RequestId == tourRequest.Id)
                    {
                        return true;
                    }
                }
            }
            //Ako se ne pronadje takvo obavestenje znaci da nikada nije prihvatio deo slozene ture
            return false;
        }
    }
}
