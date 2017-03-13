using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ServiceBooking.DAL.Entities
{
    public class Status
    {
        public int Id { get; set; }

        [Required]
        public string Value { get; set; }

        public ICollection<Order> Orders { get; set; }

        public Status()
        {
            Orders = new List<Order>();
        }
    }
}