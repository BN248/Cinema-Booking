using System.ComponentModel.DataAnnotations;

namespace Cinema_Booking.Models
{
    public partial class Taikhoan
    {
        [Key]
        public int MaTK { get; set; }

        public string Username { get; set; } = null!;

        public string Pass { get; set; } = null!;

        public string? Email { get; set; }

        public string VaiTro { get; set; } = null!;
    }
}