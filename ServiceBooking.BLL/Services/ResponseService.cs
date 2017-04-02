using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Ninject;
using ServiceBooking.BLL.DTO;
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

        public IEnumerable<ResponseViewModel> GetAllForOrder(int orderId)
        {
            var responses = _responseRepository.Find(r => r.OrderId == orderId);
            Mapper.Initialize(cfg => cfg.CreateMap<Response, ResponseViewModel>());
            return Mapper.Map<IEnumerable<Response>, List<ResponseViewModel>>(responses);
        }
    }
}
