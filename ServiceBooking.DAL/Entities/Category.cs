using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ServiceBooking.DAL.Entities
{
    public class Category
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public ICollection<Order> Orders { get; set; }

        public Category()
        {
            Orders = new List<Order>();
        }
    }
}