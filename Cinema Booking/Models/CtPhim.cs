using System;
using System.Collections.Generic;

namespace Cinema_Booking.Models;

public partial class CtPhim
{
    public string MaP { get; set; } = null!;

    public string? DaoDien { get; set; }

    public string? NgonNgu { get; set; }

    public string? QuocGia { get; set; }

    public string? MoTa { get; set; }

    public decimal? DanhGia { get; set; }

    public virtual Phim MaPNavigation { get; set; } = null!;
}
