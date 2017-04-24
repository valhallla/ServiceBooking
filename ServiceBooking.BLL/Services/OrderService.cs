using System;
using System.Collections.Generic;
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

        public IEnumerable<OrderViewModelBLL> GetAll()
        {
            var orders = _orderRepository.GetAll();
            Mapper.Initialize(cfg => cfg.CreateMap<Order, OrderViewModelBLL>());
            return Mapper.Map<IEnumerable<Order>, List<OrderViewModelBLL>>(orders);
        }

        public OperationDetails Create(OrderViewModelBLL order)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<OrderViewModelBLL, Order>());
            _orderRepository.Create(Mapper.Map<OrderViewModelBLL, Order>(order));
            return new OperationDetails(true, "Creation succeeded", string.Empty);
        }

        public OperationDetails ConfirmOrder(int id)
        {
            Order order = _orderRepository.Get(id);
            if (order != null)
            {
                order.AdminStatus = true;
                order.StatusId = 2;
                order.UploadDate = DateTime.Now;
                _orderRepository.Update(order);
                return new OperationDetails(true, "Order confirmed", string.Empty);
            }
            return new OperationDetails(false, "Order doesn't exist", "Id");
        }

        public OperationDetails DeleteOrder(int id)
        {
            _orderRepository.Delete(id);
            return new OperationDetails(false, "Order deleted", string.Empty);
        }

        public OrderViewModelBLL Find(int id)
        {
            Order order = _orderRepository.Get(id);

            if (ReferenceEquals(order, null))
                return null;

            Mapper.Initialize(cfg => cfg.CreateMap<Order, OrderViewModelBLL>());
            return Mapper.Map<Order, OrderViewModelBLL>(order);
        }

        public OperationDetails ChangeStatus(int id)
        {
            Order order = _orderRepository.Get(id);
            if (order != null)
            {
                order.StatusId++;
                _orderRepository.Update(order);
                return new OperationDetails(true, "Order status updated", string.Empty);
            }
            return new OperationDetails(false, "Order doesn't exist", "Id");
        }
    }
}
