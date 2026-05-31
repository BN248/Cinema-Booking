using System;
using System.Collections.Generic;

namespace Cinema_Booking.Models;

public partial class VwKhDanang
{
    public string MaKh { get; set; } = null!;

    public string? TenKh { get; set; }

    public string? Dc { get; set; }

    public string? Dt { get; set; }

    public string? Email { get; set; }
}
