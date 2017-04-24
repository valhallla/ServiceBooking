using System;
using System.ComponentModel.DataAnnotations;

namespace ServiceBooking.DAL.Entities
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Text { get; set; }

        public int Rating { get; set; }

        [Required]
        [DataType(DataType.DateTime), DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        public int UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int CustomerId { get; set; }
    }
}