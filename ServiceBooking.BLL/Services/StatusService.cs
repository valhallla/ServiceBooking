using System.Collections.Generic;
using System.Linq;
using Ninject;
using ServiceBooking.BLL.DTO;
using ServiceBooking.BLL.Interfaces;
using ServiceBooking.DAL.Entities;
using ServiceBooking.DAL.Interfaces;
using AutoMapper;

namespace ServiceBooking.BLL
{
    public class StatusService : IStatusService
    {
        private readonly IRepository<Status> _statusRepository;

        [Inject]
        public StatusService(IRepository<Status> statusRepository)
        {
            _statusRepository = statusRepository;
        }

        public StatusViewModel FindById(int id)
        {
            Status status = _statusRepository.Get(id);

            if (status != null)
                return new StatusViewModel { Id = status.Id, Value = status.Value };

            return null;
        }

        public IEnumerable<StatusViewModel> GetAll()
        {
            var statuses = _statusRepository.GetAll().ToList();
            Mapper.Initialize(cfg => cfg.CreateMap<Status, StatusViewModel>());
            return Mapper.Map<List<Status>, List<StatusViewModel>>(statuses);
        }
    }
}
