using System.Collections.Generic;
using Ninject;
using ServiceBooking.BLL.DTO;
using ServiceBooking.BLL.Interfaces;
using ServiceBooking.DAL.Entities;
using ServiceBooking.DAL.Interfaces;
using AutoMapper;

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
            var orders = _orderRepository.GetAll();
            var orderViewModels = new List<OrderViewModel>();

            foreach (var order in orders)
            {
                orderViewModels.Add(new OrderViewModel
                {
                    Id = order.Id,
                    Name = order.Name,
                    CategoryId = order.Category.Id,
                    StatusId = order.Status.Id,
                    AdminStatus = order.AdminStatus,
                    UploadDate = order.UploadDate,
                    CompletionDate = order.CompletionDate,
                    Price = order.Price,
                    ClientUserId = order.ClientUserId
                });
            }
            return orderViewModels;
        }

        public void Create(OrderViewModel order)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<OrderViewModel, Order>());
            _orderRepository.Create(Mapper.Map<OrderViewModel, Order>(order));
        }

        public void ConfirmOrder(int id)
        {
            Order order = _orderRepository.Get(id);
            order.AdminStatus = true;
            _orderRepository.Update(order);
        }

        public void RejectOrder(int id)
        {
            _orderRepository.Delete(id);
        }
    }
}
