using System.Collections.Generic;
using ServiceBooking.BLL.DTO;

namespace ServiceBooking.BLL.Interfaces
{
    public interface IStatusService
    {
        StatusViewModel FindById(int id);
        IEnumerable<StatusViewModel> GetAll();
    }
}
