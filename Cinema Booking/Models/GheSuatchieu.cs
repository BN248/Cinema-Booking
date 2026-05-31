using System;
using System.Collections.Generic;

namespace Cinema_Booking.Models;

public partial class GheSuatchieu
{
    public string MaSc { get; set; } = null!;

    public int MaG { get; set; }

    public string? TrangThai { get; set; }

    public virtual Ghe MaGNavigation { get; set; } = null!;

    public virtual Suatchieu MaScNavigation { get; set; } = null!;
}
