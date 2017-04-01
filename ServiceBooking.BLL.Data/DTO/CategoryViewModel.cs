using System.Collections.Generic;
using ServiceBooking.DAL.Entities;

namespace ServiceBooking.BLL.DTO
{
    public class CategoryViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

        public virtual ICollection<ClientUser> Performers { get; set; }
    }
}