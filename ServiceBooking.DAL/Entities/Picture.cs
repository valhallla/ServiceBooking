using System.ComponentModel.DataAnnotations;

namespace ServiceBooking.DAL.Entities
{
    public class Picture
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public byte[] Image { get; set; }
    }
}
