using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Ninject;
using ServiceBooking.BLL.DTO;
using ServiceBooking.BLL.Infrastructure;
using ServiceBooking.BLL.Interfaces;
using ServiceBooking.DAL.Entities;
using ServiceBooking.DAL.Interfaces;

namespace ServiceBooking.BLL.Services
{
    public class ResponseService : IResponseService
    {
        private readonly IRepository<Response> _responseRepository;

        [Inject]
        public ResponseService(IRepository<Response> responseRepository)
        {
            _responseRepository = responseRepository;
        }

        public IEnumerable<ResponseViewModelBLL> GetAllForOrder(int orderId)
        {
            var responses = _responseRepository.Find(r => r.OrderId == orderId);
            Mapper.Initialize(cfg => cfg.CreateMap<Response, ResponseViewModelBLL>()
                .ForMember("PerformerId", opt => opt.MapFrom(c => c.UserId)));
            return Mapper.Map<IEnumerable<Response>, List<ResponseViewModelBLL>>(responses);
        }

        public OperationDetails Create(ResponseViewModelBLL response)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<ResponseViewModelBLL, Response>()
                .ForMember("UserId", opt => opt.MapFrom(c => c.PerformerId)));
            _responseRepository.Create(Mapper.Map<ResponseViewModelBLL, Response>(response));
            return new OperationDetails(true, @"Sending response succeeded", string.Empty);
        }
    }
}
