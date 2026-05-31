using System;
using System.Collections.Generic;

namespace Cinema_Booking.Models;

public partial class Sanpham
{
    public string MaSp { get; set; } = null!;

    public string? TenSp { get; set; }

    public string? LoaiSp { get; set; }

    public decimal? DonGia { get; set; }

    public string? HinhAnh { get; set; }

    public virtual ICollection<Cthd> Cthds { get; set; } = new List<Cthd>();
}
