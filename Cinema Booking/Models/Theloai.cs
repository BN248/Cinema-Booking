using System;
using System.Collections.Generic;

namespace Cinema_Booking.Models;

public partial class Theloai
{
    public string MaTl { get; set; } = null!;

    public string? TenTl { get; set; }

    public virtual ICollection<Phim> MaPs { get; set; } = new List<Phim>();
}
