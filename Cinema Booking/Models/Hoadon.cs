using System;
using System.Collections.Generic;

namespace Cinema_Booking.Models;

public partial class Hoadon
{
    public string MaHd { get; set; } = null!;

    public string? MaKh { get; set; }

    public string? MaNv { get; set; }

    public DateTime? NgayLap { get; set; }

    public decimal? TongTien { get; set; }

    public string? HinhThucTt { get; set; }

    public string? TrangThai { get; set; }

    public bool? DaXacNhanThanhToan { get; set; }

    public virtual ICollection<Cthd> Cthds { get; set; } = new List<Cthd>();

    public virtual Khachhang? MaKhNavigation { get; set; }

    public virtual Nhanvien? MaNvNavigation { get; set; }

    public virtual Thanhtoan? Thanhtoan { get; set; }

    public virtual ICollection<Ve> Ves { get; set; } = new List<Ve>();
}
