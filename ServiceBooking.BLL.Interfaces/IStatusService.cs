using System.Collections.Generic;
using ServiceBooking.BLL.DTO;

namespace ServiceBooking.BLL.Interfaces
{
    public interface IStatusService
    {
        StatusViewModelBLL FindById(int id);
        IEnumerable<StatusViewModelBLL> GetAll();
    }
}
