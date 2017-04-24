using System.Linq;
using AutoMapper;
using Ninject;
using ServiceBooking.BLL.Infrastructure;
using ServiceBooking.BLL.Interfaces;
using ServiceBooking.DAL.Entities;
using ServiceBooking.DAL.Interfaces;
using ServiceBooking.DAL.UnitOfWork.DTO;

namespace ServiceBooking.BLL.Services
{
    public class PictureService : IPictureService
    {
        private readonly IRepository<Picture> _pictureRepository;

        [Inject]
        public PictureService(IRepository<Picture> pictureRepository)
        {
            _pictureRepository = pictureRepository;
        }

        public OperationDetails Create(byte[] image)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<byte[], Picture>()
                .ForMember("Image", opt => opt.MapFrom(c => image)));
            _pictureRepository.Create(Mapper.Map<byte[], Picture>(image));
            return new OperationDetails(true, "Uploading avatar succeeded", string.Empty);
        }

        public int? FindByBytes(byte[] pictureDto)
        {
            var pictures = _pictureRepository.GetAll().ToList();
            foreach (var picture in pictures)
            {
                if (picture.Image.Length != pictureDto.Length)
                    continue;
                for (int i = 0; i < pictureDto.Length; i++)
                {
                    if (pictureDto[i] != picture.Image[i])
                        break;
                    if (i == pictureDto.Length - 1)
                        return picture.Id;
                }
            }
            return null;
        }

        public PictureViewModelBLL FindById(int id) 
        {
            Picture picture = _pictureRepository.Get(id);

            if (!ReferenceEquals(picture, null))
                return new PictureViewModelBLL() { Id = picture.Id, Image = picture.Image};

            return null;
        }

        public OperationDetails Delete(int? id)
        {
            if(id == null)
                return new OperationDetails(false, "Picture doesn't exist", "Id");

            _pictureRepository.Delete(id.Value);
            return new OperationDetails(true, "Deleting picture succeeded", string.Empty);
        }
    }
}
