using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceBooking.BLL.DTO;

namespace ServiceBooking.BLL.Interfaces
{
    public interface IStatusService
    {
        StatusViewModel FindById(int id);
    }
}
