using System;
using System.Collections.Generic;

namespace Cinema_Booking.Models;

public partial class Thanhtoan
{
    public string MaTt { get; set; } = null!;

    public string? MaHd { get; set; }

    public string? PhuongThuc { get; set; }

    public decimal? SoTien { get; set; }

    public DateTime? ThoiGianTt { get; set; }

    public virtual Hoadon? MaHdNavigation { get; set; }
}
