using System;
using System.Collections.Generic;

namespace Cinema_Booking.Models;

public partial class VwDoanhthuHoadon
{
    public string MaHd { get; set; } = null!;

    public DateTime? NgayLap { get; set; }

    public decimal? TongTien { get; set; }
}
