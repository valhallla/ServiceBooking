using System.Collections.Generic;
using ServiceBooking.DAL.Entities;

namespace ServiceBooking.BLL.DTO
{
    public class ClientViewModel
    {
        public int Id { get; set; }

        public string Email { get; set; }
        
        public bool EmailConfirmed { get; set; }

        public string Password { get; set; }

        public string UserName { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public ICollection<Order> Orders { get; set; }

        public bool IsPerformer { get; set; }

        public int CategoryId { get; set; }

        public string Info { get; set; }

        public int Rating { get; set; }

        public string AdminStatus { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public string Role { get; set; }

        public ClientViewModel()
        {
            Orders = new List<Order>();
            Comments = new List<Comment>();
        }
    }
}