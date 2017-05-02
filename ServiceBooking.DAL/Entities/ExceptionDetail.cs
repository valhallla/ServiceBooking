using System;
using System.ComponentModel.DataAnnotations;

namespace ServiceBooking.DAL.Entities
{
    public class ExceptionDetail
    {
        [Key]
        public Guid Guid { get; set; }

        public string Message { get; set; }

        public string Url { get; set; }

        public string UrlReferrere { get; set; }

        public DateTime Date { get; set; }

        public string StackTrace { get; set; }

        public int? UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
