﻿using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Ninject;
using ServiceBooking.BLL.DTO;
using ServiceBooking.BLL.Interfaces;
using ServiceBooking.DAL.Entities;
using ServiceBooking.DAL.Interfaces;

namespace ServiceBooking.BLL.Services
{
    public class StatusService : IStatusService
    {
        private readonly IRepository<Status> _statusRepository;

        [Inject]
        public StatusService(IRepository<Status> statusRepository)
        {
            _statusRepository = statusRepository;
        }

        public StatusViewModelBLL FindById(int id)
        {
            Status status = _statusRepository.Get(id);

            if (!ReferenceEquals(status, null))
                return new StatusViewModelBLL { Id = status.Id, Value = status.Value };

            return null;
        }

        public IEnumerable<StatusViewModelBLL> GetAll()
        {
            var statuses = _statusRepository.GetAll().ToList();
            Mapper.Initialize(cfg => cfg.CreateMap<Status, StatusViewModelBLL>());
            return Mapper.Map<List<Status>, List<StatusViewModelBLL>>(statuses);
        }
    }
}
