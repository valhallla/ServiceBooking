﻿using System.Collections.Generic;
using AutoMapper;
using Ninject;
using ServiceBooking.BLL.DTO;
using ServiceBooking.BLL.Infrastructure;
using ServiceBooking.BLL.Interfaces;
using ServiceBooking.DAL.Entities;
using ServiceBooking.DAL.Interfaces;

namespace ServiceBooking.BLL.Services
{
    public class CommentService : ICommentService
    {
        private readonly IRepository<Comment> _commentRepository;

        [Inject]
        public CommentService(IRepository<Comment> commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public IEnumerable<CommentViewModelBLL> GetAll()
        {
            var comments = _commentRepository.GetAll();
            Mapper.Initialize(cfg => cfg.CreateMap<Comment, CommentViewModelBLL>());
            return Mapper.Map<IEnumerable<Comment>, List<CommentViewModelBLL>>(comments);
        }

        public IEnumerable<CommentViewModelBLL> GetAllForPerformer(int performerId)
        {
            var comments = _commentRepository.Find(r => r.UserId == performerId);
            Mapper.Initialize(cfg => cfg.CreateMap<Comment, CommentViewModelBLL>()
                .ForMember("PerformerId", opt => opt.MapFrom(c => c.UserId)));
            return Mapper.Map<IEnumerable<Comment>, List<CommentViewModelBLL>>(comments);
        }

        public OperationDetails Create(CommentViewModelBLL comment)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<CommentViewModelBLL, Comment>()
                .ForMember("UserId", opt => opt.MapFrom(c => c.PerformerId)));
            _commentRepository.Create(Mapper.Map<CommentViewModelBLL, Comment>(comment));
            return new OperationDetails(true, "Sending comment succeeded", string.Empty);
        }

        public OperationDetails Delete(int? id)
        {
            if (id == null)
                return new OperationDetails(false, "Comment doesn't exist", "Id");

            _commentRepository.Delete(id.Value);
            return new OperationDetails(true, "Deleting comment succeeded", string.Empty);
        }
    }
}
