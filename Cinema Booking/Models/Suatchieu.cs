using System;
using System.Collections.Generic;

namespace Cinema_Booking.Models;

public partial class Suatchieu
{
    public string MaSc { get; set; } = null!;

    public string? MaP { get; set; }

    public string? MaPh { get; set; }

    public DateOnly? NgayChieu { get; set; }

    public TimeOnly? GioChieu { get; set; }

    public TimeOnly? GioKetThuc { get; set; }

    public decimal? GiaVe { get; set; }

    public string? DinhDang { get; set; }

    public virtual ICollection<GheSuatchieu> GheSuatchieus { get; set; } = new List<GheSuatchieu>();

    public virtual Phim? MaPNavigation { get; set; }

    public virtual Phongchieu? MaPhNavigation { get; set; }

    public virtual ICollection<Ve> Ves { get; set; } = new List<Ve>();
}
