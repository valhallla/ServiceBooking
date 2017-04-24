using System.Collections.Generic;
using ServiceBooking.BLL.DTO;

namespace ServiceBooking.BLL.Interfaces
{
    public interface ICategoryService
    {
        IEnumerable<CategoryViewModelBLL> GetAll();
        CategoryViewModelBLL FindById(int id);
        CategoryViewModelBLL FindByName(string name);
    }
}
