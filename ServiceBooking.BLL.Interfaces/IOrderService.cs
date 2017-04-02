using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceBooking.BLL.DTO;
using ServiceBooking.BLL.Infrastructure;

namespace ServiceBooking.BLL.Interfaces
{
    public interface IOrderService
    {
        //void MakeOrder(OrderViewModel order);
        //CategoryViewModel Category(int? id);
        //StatusViewModel Status(int? id);
        //ClientViewModel Client(int? id);
        //IEnumerable<ResponseViewModel> Responses();
        IEnumerable<OrderViewModel> GetAll();
        OperationDetails Create(OrderViewModel orderDto);
        OperationDetails ConfirmOrder(int id);
        OperationDetails DeleteOrder(int id);
        OrderViewModel Find(int id);
    }
}
