using System.Collections.Generic;
using ServiceBooking.BLL.DTO;
using ServiceBooking.BLL.Infrastructure;

namespace ServiceBooking.BLL.Interfaces
{
    public interface ICommentService
    {
        IEnumerable<CommentViewModelBLL> GetAll();
        IEnumerable<CommentViewModelBLL> GetAllForPerformer(int performerId);
        OperationDetails Create(CommentViewModelBLL comment);
        OperationDetails Delete(int? id);
    }
}
