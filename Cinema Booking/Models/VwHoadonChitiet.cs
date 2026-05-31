using System;
using System.Collections.Generic;

namespace Cinema_Booking.Models;

public partial class VwHoadonChitiet
{
    public string MaHd { get; set; } = null!;

    public string? TenKh { get; set; }

    public string? TenNv { get; set; }

    public DateTime? NgayLap { get; set; }

    public decimal? TongTien { get; set; }

    public string? HinhThucTt { get; set; }

    public string? TrangThai { get; set; }
}
