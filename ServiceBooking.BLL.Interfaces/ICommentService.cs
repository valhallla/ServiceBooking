using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceBooking.BLL.DTO;
using ServiceBooking.BLL.Infrastructure;

namespace ServiceBooking.BLL.Interfaces
{
    public interface ICommentService
    {
        IEnumerable<CommentViewModelBLL> GetAllForPerformer(int performerId);
        OperationDetails Create(CommentViewModelBLL comment);
    }
}
