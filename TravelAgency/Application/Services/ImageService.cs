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
    public class ImageService
    {
        private readonly IImageRepository _imageRepository = Injector.CreateInstance<IImageRepository>();

        public ImageService() { }

        public void Delete(int id)
        {
            _imageRepository.Delete(id);
        }

        public List<Image> GetAll()
        {
            return _imageRepository.GetAll();
        }

        public List<Image> GetAllForTours()
        {
            return _imageRepository.GetAllForTours();
        }

        public Image GetById(int id)
        {
            return _imageRepository.GetById(id);
        }

        public void Save(Image image)
        {
            _imageRepository.Save(image);
        }

        public void SaveAll(List<Image> images)
        {
            _imageRepository.SaveAll(images);
        }

        public void Update(Image image)
        {
            _imageRepository.Update(image);
        }

    }
}
