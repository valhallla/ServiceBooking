using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceBooking.BLL.DTO;

namespace ServiceBooking.BLL.Interfaces
{
    public interface ICategoryService
    {
        IEnumerable<CategoryViewModel> GetAll();
        CategoryViewModel FindById(int id);
        CategoryViewModel FindByName(string name);
    }
}
