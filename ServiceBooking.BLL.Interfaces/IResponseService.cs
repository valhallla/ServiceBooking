using System.Collections.Generic;
using ServiceBooking.BLL.DTO;
using ServiceBooking.BLL.Infrastructure;

namespace ServiceBooking.BLL.Interfaces
{
    public interface IResponseService
    {
        IEnumerable<ResponseViewModelBLL> GetAllForOrder(int orderId);
        OperationDetails Create(ResponseViewModelBLL response);
    }
}
