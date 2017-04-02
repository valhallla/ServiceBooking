using System;
using System.Collections.Generic;
using System.Linq;
using Ninject;
using ServiceBooking.BLL.DTO;
using ServiceBooking.BLL.Interfaces;
using ServiceBooking.DAL.Entities;
using ServiceBooking.DAL.Interfaces;
using AutoMapper;
using ServiceBooking.BLL.Infrastructure;

namespace ServiceBooking.BLL.Services
{
    public class OrderService : IOrderService
    {
        private readonly IRepository<Order> _orderRepository;

        [Inject]
        public OrderService(IRepository<Order> orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public IEnumerable<OrderViewModel> GetAll()
        {
            var orders = _orderRepository.GetAll().ToList();

            Mapper.Initialize(cfg => cfg.CreateMap<Order, OrderViewModel>());
            var orderViewModels = Mapper.Map<List<Order>, List<OrderViewModel>>(orders);

            return orderViewModels;
        }

        public OperationDetails Create(OrderViewModel order)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<OrderViewModel, Order>());
            _orderRepository.Create(Mapper.Map<OrderViewModel, Order>(order));
            return new OperationDetails(true, @"Creation succeeded", string.Empty);
        }

        public OperationDetails ConfirmOrder(int id)
        {
            Order order = _orderRepository.Get(id);
            if (order != null)
            {
                order.AdminStatus = true;
                _orderRepository.Update(order);
                return new OperationDetails(true, @"Order confirmed", string.Empty);
            }
            return new OperationDetails(false, @"Order doesn't exist", "Id");
        }

        public OperationDetails DeleteOrder(int id)
        {
            _orderRepository.Delete(id);
            return new OperationDetails(false, @"Order deleted", string.Empty);
        }

        public OrderViewModel Find(int id)
        {
            Order order = _orderRepository.Get(id);
            Mapper.Initialize(cfg => cfg.CreateMap<Order, OrderViewModel>());
            return Mapper.Map<Order, OrderViewModel>(order);
        }
    }
}
