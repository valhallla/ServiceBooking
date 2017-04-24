using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ServiceBooking.DAL.Entities
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

        public virtual ICollection<ApplicationUser> ApplicationUsers { get; set; }
    }
}