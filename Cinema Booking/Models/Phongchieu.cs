using System;
using System.Collections.Generic;

namespace Cinema_Booking.Models;

public partial class Phongchieu
{
    public string MaPh { get; set; } = null!;

    public string? TenPh { get; set; }

    public string? LoaiPh { get; set; }

    public int? SoGhe { get; set; }

    public virtual ICollection<Ghe> Ghes { get; set; } = new List<Ghe>();

    public virtual ICollection<Suatchieu> Suatchieus { get; set; } = new List<Suatchieu>();
}
