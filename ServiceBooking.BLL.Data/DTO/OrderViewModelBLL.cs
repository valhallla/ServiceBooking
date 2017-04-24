using System;
using System.Collections.Generic;
using ServiceBooking.DAL.Entities;

namespace ServiceBooking.BLL.DTO
{
    public class OrderViewModelBLL
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int CategoryId { get; set; }

        public int? PictureId { get; set; }

        public int StatusId { get; set; }

        public bool AdminStatus { get; set; }

        public DateTime UploadDate { get; set; }

        public DateTime CompletionDate { get; set; }

        public int Price { get; set; }

        public virtual int UserId { get; set; }

        public ICollection<Response> Responses { get; set; }

        public OrderViewModelBLL()
        {
            Responses = new List<Response>();
        }
    }
}
