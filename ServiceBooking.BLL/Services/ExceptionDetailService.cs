using AutoMapper;
using Ninject;
using ServiceBooking.BLL.Infrastructure;
using ServiceBooking.BLL.Interfaces;
using ServiceBooking.DAL.Entities;
using ServiceBooking.DAL.Interfaces;
using ServiceBooking.DAL.UnitOfWork.DTO;

namespace ServiceBooking.BLL.Services
{
    public class ExceptionDetailService : IExceptionDetailService
    {
        private readonly IRepository<ExceptionDetail> _exceptionDetailRepository;

        [Inject]
        public ExceptionDetailService(IRepository<ExceptionDetail> exceptionDetailRepository)
        {
            _exceptionDetailRepository = exceptionDetailRepository;
        }

        public OperationDetails Create(ExceptionDetailViewModelBLL exceptionDetail)
        {
            if (exceptionDetail.UserId == 0)
                exceptionDetail.UserId = null;

            Mapper.Initialize(cfg => cfg.CreateMap<ExceptionDetailViewModelBLL, ExceptionDetail>());
            _exceptionDetailRepository.Create(Mapper.Map<ExceptionDetailViewModelBLL, ExceptionDetail>(exceptionDetail));
            return new OperationDetails(true, "Adding exception succeeded", string.Empty);
        }
    }
}
