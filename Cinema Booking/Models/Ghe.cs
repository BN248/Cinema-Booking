using System;
using System.Collections.Generic;

namespace Cinema_Booking.Models;

public partial class Ghe
{
    public int MaG { get; set; }

    public string? HangG { get; set; }

    public int? SoG { get; set; }

    public string? MaPh { get; set; }

    public string? LoaiG { get; set; }

    public virtual ICollection<GheSuatchieu> GheSuatchieus { get; set; } = new List<GheSuatchieu>();

    public virtual Phongchieu? MaPhNavigation { get; set; }

    public virtual ICollection<Ve> Ves { get; set; } = new List<Ve>();
}
