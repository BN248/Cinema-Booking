using System;
using System.Collections.Generic;

namespace Cinema_Booking.Models;

public partial class Nhanvien
{
    public string MaNv { get; set; } = null!;

    public string? TenNv { get; set; }

    public bool? Phai { get; set; }

    public DateOnly? NgaySinh { get; set; }

    public string? Dt { get; set; }

    public string? Dc { get; set; }

    public string? Email { get; set; }

    public string? Cccd { get; set; }

    public string? ChucVu { get; set; }

    public string? LoaiNv { get; set; }

    public string? CaLam { get; set; }

    public DateOnly? NgayVaoLam { get; set; }

    public double? Luong { get; set; }

    public virtual ICollection<Hoadon> Hoadons { get; set; } = new List<Hoadon>();
}
