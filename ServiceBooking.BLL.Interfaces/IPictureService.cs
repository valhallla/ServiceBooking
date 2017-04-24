using ServiceBooking.BLL.Infrastructure;
using ServiceBooking.DAL.UnitOfWork.DTO;

namespace ServiceBooking.BLL.Interfaces
{
    public interface IPictureService
    {
        OperationDetails Create(byte[] image);
        int? FindByBytes(byte[] image);
        PictureViewModelBLL FindById(int id);
        OperationDetails Delete(int? id);
    }
}
