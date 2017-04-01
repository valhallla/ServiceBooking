using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using ServiceBooking.BLL.DTO;
using ServiceBooking.BLL.Interfaces;
using ServiceBooking.DAL.Entities;
using ServiceBooking.DAL.Interfaces;
using ServiceBooking.DAL.Repositories;

namespace ServiceBooking.BLL
{
    public class StatusService : IStatusService
    {
        public IRepository<Status> StatusRepository { get; }

        [Inject]
        public StatusService(IRepository<Status> statusRepository)
        {
            StatusRepository = statusRepository;
        }

        public StatusViewModel FindById(int id)
        {
            Status status = StatusRepository.Get(id);

            if (status != null)
                return new StatusViewModel { Id = status.Id, Value = status.Value };

            return null;
        }
    }
}
