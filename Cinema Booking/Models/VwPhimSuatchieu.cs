using System;
using System.Collections.Generic;

namespace Cinema_Booking.Models;

public partial class VwPhimSuatchieu
{
    public string MaP { get; set; } = null!;

    public string? TenP { get; set; }

    public string MaSc { get; set; } = null!;

    public DateOnly? NgayChieu { get; set; }

    public TimeOnly? GioChieu { get; set; }

    public decimal? GiaVe { get; set; }
}
