using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using ServiceBooking.BLL.DTO;
using ServiceBooking.BLL.Interfaces;
using ServiceBooking.DAL.Entities;
using ServiceBooking.DAL.Interfaces;
using ServiceBooking.DAL.Repositories;

namespace ServiceBooking.BLL.Services
{
    public class OrderService : IOrderService
    {
        public IRepository<Order> OrderRepository { get; }

        [Inject]
        public OrderService(IRepository<Order> orderRepository)
        {
            OrderRepository = orderRepository;
        }

        public IEnumerable<OrderViewModel> GetAll()
        {
            var orders = OrderRepository.GetAll();
            var orderViewModels = new List<OrderViewModel>();

            foreach (var order in orders)
            {
                orderViewModels.Add(new OrderViewModel
                {
                    Id = order.Id,
                    Name = order.Name,
                    Category = order.Category.Name,
                    Status = order.Status.Value,
                    AdminStatus = order.AdminStatus,
                    UploadDate = order.UploadDate,
                    CompletionDate = order.CompletionDate,
                    Price = order.Price,
                    ClientUserId = order.ClientUserId
                });
            }
            return orderViewModels;
        }

        //    public OrderService(IUnitOfWork uow)
        //    {
        //        Database = uow;
        //    }

        //    public void MakeOrder(OrderViewModel order)
        //    {
        //        var category = Database.Categories.Get(OrderViewModel.CategoryId);

        //        // валидация
        //        if (order == null)
        //            throw new ValidationException("Телефон не найден", "");
        //        // применяем скидку
        //        decimal sum = new Discount(0.1m).GetDiscountedPrice(phone.Price);
        //        //Order order = new Order
        //        //{
        //        //    Date = DateTime.Now,
        //        //    Address = orderDto.Address,
        //        //    PhoneId = phone.Id,
        //        //    Sum = sum,
        //        //    PhoneNumber = orderDto.PhoneNumber
        //        //}

        //    public CategoryViewModel Category(int? id)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    public StatusViewModel Status(int? id)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    public ClientViewModel Client(int? id)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    public IEnumerable<ResponseViewModel> GetResponses()
        //    {
        //        throw new NotImplementedException();
        //    }

        //    public void Dispose()
        //    {
        //        throw new NotImplementedException();
        //    }
        //}

        //public CategoryViewModel Category(int? id)
        //{
        //    throw new NotImplementedException();
        //}

        //public StatusViewModel Status(int? id)
        //{
        //    throw new NotImplementedException();
        //}

        //public ClientViewModel Client(int? id)
        //{
        //    throw new NotImplementedException();
        //}

        //public IEnumerable<ResponseViewModel> Responses()
        //{
        //    throw new NotImplementedException();
        //}

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
