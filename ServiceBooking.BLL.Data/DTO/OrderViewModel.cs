using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceBooking.DAL.Entities;

namespace ServiceBooking.BLL.DTO
{
    public class OrderViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int CategoryId { get; set; }

        public int StatusId { get; set; }

        public bool AdminStatus { get; set; }

        public DateTime UploadDate { get; set; }

        public DateTime CompletionDate { get; set; }

        public int Price { get; set; }

        public virtual int UserId { get; set; }
    }
}
