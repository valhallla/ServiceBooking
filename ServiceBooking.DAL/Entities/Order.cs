using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ServiceBooking.DAL.Entities
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public int? CategoryId { get; set; }
        public Category Category { get; set; }

        public int StatusId { get; set; }
        public Status Status { get; set; }

        public bool AdminStatus { get; set; }

        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime UploadDate { get; set; }

        [Required]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CompletionDate { get; set; }

        [Required]
        public int Price { get; set; }

        public virtual int UserId { get; set; }
        public ApplicationUser User { get; set; }

        public virtual ICollection<Response> Responses { get; set; }
    }
}