using ServiceBooking.BLL.Infrastructure;
using ServiceBooking.DAL.UnitOfWork.DTO;

namespace ServiceBooking.BLL.Interfaces
{
    public interface IExceptionDetailService
    {
        OperationDetails Create(ExceptionDetailViewModelBLL exceptionDetail);
    }
}
