using System.Collections.Generic;
using ServiceBooking.DAL.Entities;

namespace ServiceBooking.BLL.DTO
{
    public class CategoryViewModelBLL
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

        public virtual ICollection<ApplicationUser> Performers { get; set; }

        public CategoryViewModelBLL()
        {
            Orders = new List<Order>();
            Performers = new List<ApplicationUser>();
        }
    }
}