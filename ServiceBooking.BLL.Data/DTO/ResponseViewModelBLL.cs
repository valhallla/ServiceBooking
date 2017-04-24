using System;

namespace ServiceBooking.BLL.DTO
{
    public class ResponseViewModelBLL
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public DateTime Date { get; set; }

        public int? OrderId { get; set; }

        public int PerformerId { get; set; }
    }
}
