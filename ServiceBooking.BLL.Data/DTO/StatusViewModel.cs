using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceBooking.DAL.Entities;

namespace ServiceBooking.BLL.DTO
{
    public class StatusViewModel
    {
        public int Id { get; set; }

        public string Value { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
