using System.Collections.Generic;
using ServiceBooking.DAL.Entities;

namespace ServiceBooking.BLL.DTO
{
    public class StatusViewModelBLL
    {
        public int Id { get; set; }

        public string Value { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
