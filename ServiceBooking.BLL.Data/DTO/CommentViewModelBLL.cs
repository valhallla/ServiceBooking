using System;

namespace ServiceBooking.BLL.DTO
{
    public class CommentViewModelBLL
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public int Rating { get; set; }

        public int PerformerId { get; set; }

        public int CustomerId { get; set; }

        public DateTime Date { get; set; }
    }
}
