using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceBooking.DAL.Entities
{
    public class ClientUser : ApplicationUser
    {
        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

        public bool IsPerformer { get; set; }

        public int? CategoryId { get; set; }
        public Category Category { get; set; }

        public string Info { get; set; }

        public int Rating { get; set; }

        public string AdminStatus { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }
}
