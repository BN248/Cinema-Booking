using System;
using System.Collections.Generic;

namespace Cinema_Booking.Models;

public partial class Ve
{
    public string MaV { get; set; } = null!;

    public string? MaHd { get; set; }

    public string? MaSc { get; set; }

    public int? MaG { get; set; }

    public string? MaKh { get; set; }

    public int MaTK { get; set; }

    public decimal? GiaVe { get; set; }

    public DateOnly? NgayDat { get; set; }

    public string? TrangThai { get; set; }

    public DateTime? HanThanhToan { get; set; }

    public virtual Ghe? MaGNavigation { get; set; }

    public virtual Hoadon? MaHdNavigation { get; set; }

    public virtual Khachhang? MaKhNavigation { get; set; }

    public virtual Suatchieu? MaScNavigation { get; set; }
}
