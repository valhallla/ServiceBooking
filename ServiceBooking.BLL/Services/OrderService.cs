//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using ServiceBooking.BLL.DTO;
//using ServiceBooking.BLL.Interfaces;
//using ServiceBooking.DAL.Entities;
//using ServiceBooking.DAL.Interfaces;

//namespace ServiceBooking.BLL.Services
//{
//    class OrderService : IOrderService
//    {
//        IEntityUnitOfWork Database { get; set; }

//        public OrderService(IEntityUnitOfWork uow)
//        {
//            Database = uow;
//        }

//        public void MakeOrder(OrderViewModel order)
//        {
//            var category = Database.Categories.Get(OrderViewModel.CategoryId);

//            // валидация
//            if (order == null)
//                throw new ValidationException("Телефон не найден", "");
//            // применяем скидку
//            decimal sum = new Discount(0.1m).GetDiscountedPrice(phone.Price);
//            //Order order = new Order
//            //{
//            //    Date = DateTime.Now,
//            //    Address = orderDto.Address,
//            //    PhoneId = phone.Id,
//            //    Sum = sum,
//            //    PhoneNumber = orderDto.PhoneNumber
//            //}

//        public CategoryViewModel Category(int? id)
//        {
//            throw new NotImplementedException();
//        }

//        public StatusViewModel Status(int? id)
//        {
//            throw new NotImplementedException();
//        }

//        public ClientViewModel Client(int? id)
//        {
//            throw new NotImplementedException();
//        }

//        public IEnumerable<ResponseViewModel> GetResponses()
//        {
//            throw new NotImplementedException();
//        }

//        public void Dispose()
//        {
//            throw new NotImplementedException();
//        }
//    }

//        public CategoryViewModel Category(int? id)
//        {
//            throw new NotImplementedException();
//        }

//        public StatusViewModel Status(int? id)
//        {
//            throw new NotImplementedException();
//        }

//        public ClientViewModel Client(int? id)
//        {
//            throw new NotImplementedException();
//        }

//        public IEnumerable<ResponseViewModel> GetResponses()
//        {
//            throw new NotImplementedException();
//        }

//        public void Dispose()
//        {
//            throw new NotImplementedException();
//        }
//    }
