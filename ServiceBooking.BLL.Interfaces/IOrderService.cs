using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceBooking.BLL.DTO;

namespace ServiceBooking.BLL.Interfaces
{
    public interface IOrderService : IDisposable
    {
        //void MakeOrder(OrderViewModel order);
        //CategoryViewModel Category(int? id);
        //StatusViewModel Status(int? id);
        //ClientViewModel Client(int? id);
        //IEnumerable<ResponseViewModel> Responses();
        IEnumerable<OrderViewModel> GetAll();
    }
}
