using System.Collections.Generic;
using ServiceBooking.BLL.DTO;
using ServiceBooking.BLL.Infrastructure;

namespace ServiceBooking.BLL.Interfaces
{
    public interface IOrderService
    {
        IEnumerable<OrderViewModelBLL> GetAll();
        OperationDetails Create(OrderViewModelBLL orderDto);
        OperationDetails ConfirmOrder(int id);
        OperationDetails DeleteOrder(int id);
        OrderViewModelBLL Find(int id);
        OperationDetails ChangeStatus(int id);
    }
}
