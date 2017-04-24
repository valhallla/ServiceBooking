using System;
using System.ComponentModel.DataAnnotations;

namespace ServiceBooking.DAL.Entities
{
    public class Response
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Text { get; set; }

        [Required]
        [DataType(DataType.DateTime), DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        public int? OrderId { get; set; }
        public Order Order { get; set; }

        public int UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}