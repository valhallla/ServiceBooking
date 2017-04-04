using System.Collections.Generic;
using ServiceBooking.BLL.DTO;
using ServiceBooking.BLL.Infrastructure;

namespace ServiceBooking.BLL.Interfaces
{
    public interface IResponseService
    {
        IEnumerable<ResponseViewModel> GetAllForOrder(int orderId);
        OperationDetails Create(ResponseViewModel response);
    }
}
