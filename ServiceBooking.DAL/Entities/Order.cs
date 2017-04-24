using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ServiceBooking.DAL.Entities
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public int? PictureId { get; set; }
        public Picture Picture { get; set; }

        public int StatusId { get; set; }
        public Status Status { get; set; }

        public bool AdminStatus { get; set; }

        [Required]
        [DataType(DataType.DateTime), DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime UploadDate { get; set; }

        [Required]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CompletionDate { get; set; }

        public int Price { get; set; }

        public virtual int UserId { get; set; }
        public ApplicationUser User { get; set; }

        public virtual ICollection<Response> Responses { get; set; }
    }
}