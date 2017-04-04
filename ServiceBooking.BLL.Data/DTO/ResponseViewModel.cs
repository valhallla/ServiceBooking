using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceBooking.DAL.Entities;

namespace ServiceBooking.BLL.DTO
{
    public class ResponseViewModel
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public DateTime Date { get; set; }

        public int? OrderId { get; set; }

        public int PerformerId { get; set; }
    }
}
