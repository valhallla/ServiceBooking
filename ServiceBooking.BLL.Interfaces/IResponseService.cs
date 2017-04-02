using System.Collections.Generic;
using ServiceBooking.BLL.DTO;

namespace ServiceBooking.BLL.Interfaces
{
    public interface IResponseService
    {
        IEnumerable<ResponseViewModel> GetAllForOrder(int orderId);
    }
}
