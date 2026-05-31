using System;
using System.Collections.Generic;

namespace Cinema_Booking.Models;

public partial class Khachhang
{
    public string MaKh { get; set; } = null!;

    public string? TenKh { get; set; }

    public bool? Phai { get; set; }

    public DateOnly? NgaySinh { get; set; }

    public string? Dt { get; set; }

    public string? Dc { get; set; }

    public string? Email { get; set; }

    public string? LoaiKh { get; set; }

    public int? Diem { get; set; }

    public DateOnly? NgayDk { get; set; }

    public virtual ICollection<Hoadon> Hoadons { get; set; } = new List<Hoadon>();

    public virtual ICollection<Ve> Ves { get; set; } = new List<Ve>();
}
