using System;
using System.Collections.Generic;

namespace Cinema_Booking.Models;

public partial class Phim
{
    public string MaP { get; set; } = null!;

    public string? TenP { get; set; }

    public string? HinhAnh { get; set; }

    public int? ThoiLuong { get; set; }

    public int? DoTuoi { get; set; }

    public DateOnly? NgayKhoiChieu { get; set; }

    // CHI TIẾT PHIM
    public virtual CtPhim? CtPhim { get; set; }

    // SUẤT CHIẾU
    public virtual ICollection<Suatchieu> Suatchieus { get; set; }
        = new List<Suatchieu>();

    // THỂ LOẠI
    public virtual ICollection<Theloai> MaTls { get; set; }
        = new List<Theloai>();
}