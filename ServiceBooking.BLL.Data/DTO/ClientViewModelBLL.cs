using System;
using System.Collections.Generic;
using ServiceBooking.DAL.Entities;

namespace ServiceBooking.BLL.DTO
{
    public class ClientViewModelBLL
    {
        public int Id { get; set; }

        public string Email { get; set; }
        
        public bool EmailConfirmed { get; set; }

        public string PhoneNumber { get; set; }

        public string Password { get; set; }

        public string UserName { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Company { get; set; }

        public DateTime RegistrationDate { get; set; }

        public ICollection<Category> CategoriesBll { get; set; }

        public ICollection<Order> OrdersBll { get; set; }

        public bool IsPerformer { get; set; }

        public string Info { get; set; }

        public int? Rating { get; set; }

        public bool AdminStatus { get; set; }

        public ICollection<Comment> CommentsBll { get; set; }

        public string Role { get; set; }

        public ClientViewModelBLL()
        {
            OrdersBll = new List<Order>();
            CommentsBll = new List<Comment>();
            CategoriesBll = new List<Category>();
        }
    }
}