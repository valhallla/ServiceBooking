using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ServiceBooking.DAL.Entities
{
    public class Status
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Value { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}