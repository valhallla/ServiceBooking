using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceBooking.DAL.Entities;

namespace ServiceBooking.BLL.DTO
{
    public class UserDto
    {
        public string Id { get; set; }

        public string Email { get; set; }
        
        public bool EmailConfirmed { get; set; }

        public string Password { get; set; }

        public string UserName { get; set; }

        public string Name { get; set; }

        public ICollection<Order> Orders { get; set; }

        public bool IsPerformer { get; set; }

        public int? CategoryId { get; set; }
        public Category Category { get; set; }

        public string Info { get; set; }

        public int Rating { get; set; }

        public string AdminStatus { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public string Role { get; set; }

        public UserDto()
        {
            Orders = new List<Order>();
            Comments = new List<Comment>();
        }
    }
}