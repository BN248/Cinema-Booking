using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Cinema_Booking.Models;

public partial class QlrcpContext : DbContext
{
    public QlrcpContext()
    {
    }

    public QlrcpContext(DbContextOptions<QlrcpContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CtPhim> CtPhims { get; set; }

    public virtual DbSet<Cthd> Cthds { get; set; }

    public virtual DbSet<Ghe> Ghes { get; set; }

    public virtual DbSet<GheSuatchieu> GheSuatchieus { get; set; }

    public virtual DbSet<Hoadon> Hoadons { get; set; }

    public virtual DbSet<Khachhang> Khachhangs { get; set; }

    public virtual DbSet<Nhanvien> Nhanviens { get; set; }

    public virtual DbSet<Phim> Phims { get; set; }

    public virtual DbSet<Phongchieu> Phongchieus { get; set; }

    public virtual DbSet<Sanpham> Sanphams { get; set; }

    public virtual DbSet<Suatchieu> Suatchieus { get; set; }

    public virtual DbSet<Thanhtoan> Thanhtoans { get; set; }

    public virtual DbSet<Theloai> Theloais { get; set; }

    public virtual DbSet<Ve> Ves { get; set; }

    public virtual DbSet<Taikhoan> Taikhoans { get; set; }

    public virtual DbSet<VwDoanhthuHoadon> VwDoanhthuHoadons { get; set; }

    public virtual DbSet<VwHoadonChitiet> VwHoadonChitiets { get; set; }

    public virtual DbSet<VwKhDanang> VwKhDanangs { get; set; }

    public virtual DbSet<VwPhimSuatchieu> VwPhimSuatchieus { get; set; }

    public virtual DbSet<VwVeTheoKhachhang> VwVeTheoKhachhangs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=THANHNGUYEN;Database=QLRCP;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CtPhim>(entity =>
        {
            entity.HasKey(e => e.MaP).HasName("PK__CT_PHIM__C7977BA809E7B87B");

            entity.ToTable("CT_PHIM");

            entity.Property(e => e.MaP).HasMaxLength(5);
            entity.Property(e => e.DanhGia).HasColumnType("decimal(3, 1)");
            entity.Property(e => e.DaoDien).HasMaxLength(30);
            entity.Property(e => e.MoTa).HasMaxLength(100);
            entity.Property(e => e.NgonNgu).HasMaxLength(10);
            entity.Property(e => e.QuocGia).HasMaxLength(30);

            entity.HasOne(d => d.MaPNavigation).WithOne(p => p.CtPhim)
                .HasForeignKey<CtPhim>(d => d.MaP)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CT_PHIM__MaP__44CA3770");
        });

        modelBuilder.Entity<Cthd>(entity =>
        {
            entity.HasKey(e => new { e.MaHd, e.MaSp });

            entity.ToTable("CTHD");

            entity.Property(e => e.MaHd)
                .HasMaxLength(5)
                .HasColumnName("MaHD");
            entity.Property(e => e.MaSp)
                .HasMaxLength(5)
                .HasColumnName("MaSP");
            entity.Property(e => e.DonGia).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ThanhTien)
                .HasComputedColumnSql("([SoLuong]*[DonGia])", false)
                .HasColumnType("decimal(21, 2)");

            entity.HasOne(d => d.MaHdNavigation).WithMany(p => p.Cthds)
                .HasForeignKey(d => d.MaHd)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CTHD__MaHD__0697FACD");

            entity.HasOne(d => d.MaSpNavigation).WithMany(p => p.Cthds)
                .HasForeignKey(d => d.MaSp)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CTHD__MaSP__078C1F06");
        });

        modelBuilder.Entity<Ghe>(entity =>
        {
            entity.HasKey(e => e.MaG).HasName("PK__GHE__C7977BB1268FBD9F");

            entity.ToTable("GHE");

            entity.HasIndex(e => new { e.MaPh, e.HangG, e.SoG }, "UQ_GHE_PHONG").IsUnique();

            entity.Property(e => e.HangG).HasMaxLength(5);
            entity.Property(e => e.LoaiG).HasMaxLength(20);
            entity.Property(e => e.MaPh)
                .HasMaxLength(5)
                .HasColumnName("MaPH");

            entity.HasOne(d => d.MaPhNavigation).WithMany(p => p.Ghes)
                .HasForeignKey(d => d.MaPh)
                .HasConstraintName("FK__GHE__MaPH__4B7734FF");
        });

        modelBuilder.Entity<GheSuatchieu>(entity =>
        {
            entity.HasKey(e => new { e.MaSc, e.MaG }).HasName("PK_GHE_PC");

            entity.ToTable("GHE_SUATCHIEU");

            entity.Property(e => e.MaSc)
                .HasMaxLength(5)
                .HasColumnName("MaSC");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(20)
                .HasDefaultValue("Trống");

            entity.HasOne(d => d.MaGNavigation).WithMany(p => p.GheSuatchieus)
                .HasForeignKey(d => d.MaG)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__GHE_SUATCHI__MaG__5E8A0973");

            entity.HasOne(d => d.MaScNavigation).WithMany(p => p.GheSuatchieus)
                .HasForeignKey(d => d.MaSc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__GHE_SUATCH__MaSC__5D95E53A");
        });

        modelBuilder.Entity<Hoadon>(entity =>
        {
            entity.HasKey(e => e.MaHd).HasName("PK__HOADON__2725A6E030EECF4D");

            entity.ToTable("HOADON");

            entity.Property(e => e.MaHd)
                .HasMaxLength(5)
                .HasColumnName("MaHD");
            entity.Property(e => e.HinhThucTt)
                .HasMaxLength(30)
                .HasColumnName("HinhThucTT");
            entity.Property(e => e.MaKh)
                .HasMaxLength(5)
                .HasColumnName("MaKH");
            entity.Property(e => e.MaNv)
                .HasMaxLength(5)
                .HasColumnName("MaNV");
            entity.Property(e => e.NgayLap)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TongTien).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.TrangThai).HasMaxLength(30);

            entity.HasOne(d => d.MaKhNavigation).WithMany(p => p.Hoadons)
                .HasForeignKey(d => d.MaKh)
                .HasConstraintName("FK__HOADON__MaKH__73852659");

            entity.HasOne(d => d.MaNvNavigation).WithMany(p => p.Hoadons)
                .HasForeignKey(d => d.MaNv)
                .HasConstraintName("FK__HOADON__MaNV__74794A92");
        });

        modelBuilder.Entity<Khachhang>(entity =>
        {
            entity.HasKey(e => e.MaKh).HasName("PK__KHACHHAN__2725CF1E3021F62F");

            entity.ToTable("KHACHHANG");

            entity.HasIndex(e => e.Dt, "UQ__KHACHHAN__32146216D840A3FD").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__KHACHHAN__A9D10534EF801C64").IsUnique();

            entity.Property(e => e.MaKh)
                .HasMaxLength(5)
                .HasColumnName("MaKH");
            entity.Property(e => e.Dc)
                .HasMaxLength(30)
                .HasColumnName("DC");
            entity.Property(e => e.Dt)
                .HasMaxLength(10)
                .HasColumnName("DT");
            entity.Property(e => e.Email).HasMaxLength(30);
            entity.Property(e => e.LoaiKh)
                .HasMaxLength(30)
                .HasColumnName("LoaiKH");
            entity.Property(e => e.NgayDk).HasColumnName("NgayDK");
            entity.Property(e => e.Phai).HasDefaultValue(false);
            entity.Property(e => e.TenKh)
                .HasMaxLength(30)
                .HasColumnName("TenKH");
        });

        modelBuilder.Entity<Nhanvien>(entity =>
        {
            entity.HasKey(e => e.MaNv).HasName("PK__NHANVIEN__2725D70A0B630581");

            entity.ToTable("NHANVIEN");

            entity.HasIndex(e => e.Dt, "UQ__NHANVIEN__3214621658E5C354").IsUnique();

            entity.HasIndex(e => e.Cccd, "UQ__NHANVIEN__A955A0AA91427F26").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__NHANVIEN__A9D105346852A44E").IsUnique();

            entity.Property(e => e.MaNv)
                .HasMaxLength(5)
                .HasColumnName("MaNV");
            entity.Property(e => e.CaLam).HasMaxLength(30);
            entity.Property(e => e.Cccd)
                .HasMaxLength(30)
                .HasColumnName("CCCD");
            entity.Property(e => e.ChucVu).HasMaxLength(30);
            entity.Property(e => e.Dc)
                .HasMaxLength(30)
                .HasColumnName("DC");
            entity.Property(e => e.Dt)
                .HasMaxLength(10)
                .HasColumnName("DT");
            entity.Property(e => e.Email).HasMaxLength(30);
            entity.Property(e => e.LoaiNv)
                .HasMaxLength(30)
                .HasColumnName("LoaiNV");
            entity.Property(e => e.Phai).HasDefaultValue(false);
            entity.Property(e => e.TenNv)
                .HasMaxLength(30)
                .HasColumnName("TenNV");
        });

        modelBuilder.Entity<Phim>(entity =>
        {
            entity.HasKey(e => e.MaP).HasName("PK__PHIM__C7977BA85A7CA8DE");

            entity.ToTable("PHIM");

            entity.Property(e => e.MaP).HasMaxLength(5);
            entity.Property(e => e.TenP).HasMaxLength(30);

            entity.HasMany(d => d.MaTls).WithMany(p => p.MaPs)
                .UsingEntity<Dictionary<string, object>>(
                    "PhimTl",
                    r => r.HasOne<Theloai>().WithMany()
                        .HasForeignKey("MaTl")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__PHIM_TL__MaTL__41EDCAC5"),
                    l => l.HasOne<Phim>().WithMany()
                        .HasForeignKey("MaP")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__PHIM_TL__MaP__40F9A68C"),
                    j =>
                    {
                        j.HasKey("MaP", "MaTl").HasName("PK_PTL");
                        j.ToTable("PHIM_TL");
                        j.IndexerProperty<string>("MaP").HasMaxLength(5);
                        j.IndexerProperty<string>("MaTl")
                            .HasMaxLength(5)
                            .HasColumnName("MaTL");
                    });
        });

        modelBuilder.Entity<Phongchieu>(entity =>
        {
            entity.HasKey(e => e.MaPh).HasName("PK__PHONGCHI__2725E7FAF27772B8");

            entity.ToTable("PHONGCHIEU");

            entity.Property(e => e.MaPh)
                .HasMaxLength(5)
                .HasColumnName("MaPH");
            entity.Property(e => e.LoaiPh)
                .HasMaxLength(30)
                .HasColumnName("LoaiPH");
            entity.Property(e => e.TenPh)
                .HasMaxLength(30)
                .HasColumnName("TenPH");
        });

        modelBuilder.Entity<Sanpham>(entity =>
        {
            entity.HasKey(e => e.MaSp).HasName("PK__SANPHAM__2725081CCB463968");

            entity.ToTable("SANPHAM");

            entity.Property(e => e.MaSp)
                .HasMaxLength(5)
                .HasColumnName("MaSP");
            entity.Property(e => e.DonGia).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.LoaiSp)
                .HasMaxLength(30)
                .HasColumnName("LoaiSP");
            entity.Property(e => e.TenSp)
                .HasMaxLength(50)
                .HasColumnName("TenSP");
        });

        modelBuilder.Entity<Suatchieu>(entity =>
        {
            entity.HasKey(e => e.MaSc).HasName("PK__SUATCHIE__272508093309405A");

            entity.ToTable("SUATCHIEU", tb => tb.HasTrigger("TRG_KIEMTRA_TRUNG_GIO"));

            entity.Property(e => e.MaSc)
                .HasMaxLength(5)
                .HasColumnName("MaSC");
            entity.Property(e => e.DinhDang).HasMaxLength(30);
            entity.Property(e => e.GiaVe).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.MaP).HasMaxLength(5);
            entity.Property(e => e.MaPh)
                .HasMaxLength(5)
                .HasColumnName("MaPH");

            entity.HasOne(d => d.MaPNavigation).WithMany(p => p.Suatchieus)
                .HasForeignKey(d => d.MaP)
                .HasConstraintName("FK__SUATCHIEU__MaP__51300E55");

            entity.HasOne(d => d.MaPhNavigation).WithMany(p => p.Suatchieus)
                .HasForeignKey(d => d.MaPh)
                .HasConstraintName("FK__SUATCHIEU__MaPH__5224328E");
        });

        modelBuilder.Entity<Thanhtoan>(entity =>
        {
            entity.HasKey(e => e.MaTt).HasName("PK__THANHTOA__27250079709E1B0C");

            entity.ToTable("THANHTOAN");

            entity.HasIndex(e => e.MaHd, "UQ_TT").IsUnique();

            entity.Property(e => e.MaTt)
                .HasMaxLength(5)
                .HasColumnName("MaTT");
            entity.Property(e => e.MaHd)
                .HasMaxLength(5)
                .HasColumnName("MaHD");
            entity.Property(e => e.PhuongThuc).HasMaxLength(30);
            entity.Property(e => e.SoTien).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ThoiGianTt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("ThoiGianTT");

            entity.HasOne(d => d.MaHdNavigation).WithOne(p => p.Thanhtoan)
                .HasForeignKey<Thanhtoan>(d => d.MaHd)
                .HasConstraintName("FK__THANHTOAN__MaHD__7FEAFD3E");
        });

        modelBuilder.Entity<Theloai>(entity =>
        {
            entity.HasKey(e => e.MaTl).HasName("PK__THELOAI__27250071BF77C5B2");

            entity.ToTable("THELOAI");

            entity.Property(e => e.MaTl)
                .HasMaxLength(5)
                .HasColumnName("MaTL");
            entity.Property(e => e.TenTl)
                .HasMaxLength(30)
                .HasColumnName("TenTL");
        });

        modelBuilder.Entity<Ve>(entity =>
        {
            entity.HasKey(e => e.MaV).HasName("PK__VE__C7977BA23819D4FE");

            entity.ToTable("VE", tb =>
                {
                    tb.HasTrigger("TRG_CAPNHAT_GHE_DABAN");
                    tb.HasTrigger("TRG_KHONG_DAT_VE_QUA_GIO");
                });

            entity.HasIndex(e => new { e.MaSc, e.MaG }, "UQ_GHE").IsUnique();

            entity.Property(e => e.MaV).HasMaxLength(5);
            entity.Property(e => e.GiaVe).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.MaHd)
                .HasMaxLength(5)
                .HasColumnName("MaHD");
            entity.Property(e => e.MaKh)
                .HasMaxLength(5)
                .HasColumnName("MaKH");
            entity.Property(e => e.MaSc)
                .HasMaxLength(5)
                .HasColumnName("MaSC");

            entity.HasOne(d => d.MaGNavigation).WithMany(p => p.Ves)
                .HasForeignKey(d => d.MaG)
                .HasConstraintName("FK__VE__MaG__7B264821");

            entity.HasOne(d => d.MaHdNavigation).WithMany(p => p.Ves)
                .HasForeignKey(d => d.MaHd)
                .HasConstraintName("FK__VE__MaHD__793DFFAF");

            entity.HasOne(d => d.MaKhNavigation).WithMany(p => p.Ves)
                .HasForeignKey(d => d.MaKh)
                .HasConstraintName("FK__VE__MaKH__7C1A6C5A");

            entity.HasOne(d => d.MaScNavigation).WithMany(p => p.Ves)
                .HasForeignKey(d => d.MaSc)
                .HasConstraintName("FK__VE__MaSC__7A3223E8");
        });

        modelBuilder.Entity<Taikhoan>(entity =>
        {
            entity.ToTable("TAIKHOAN");

            entity.HasKey(e => e.Username);

            entity.Property(e => e.Username)
                .HasMaxLength(50);

            entity.Property(e => e.Pass)
                .HasMaxLength(50);

            entity.Property(e => e.Email)
                .HasMaxLength(50);

            entity.Property(e => e.VaiTro)
                .HasMaxLength(20);
        });

        modelBuilder.Entity<VwDoanhthuHoadon>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VW_DOANHTHU_HOADON");

            entity.Property(e => e.MaHd)
                .HasMaxLength(5)
                .HasColumnName("MaHD");
            entity.Property(e => e.NgayLap).HasColumnType("datetime");
            entity.Property(e => e.TongTien).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<VwHoadonChitiet>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VW_HOADON_CHITIET");

            entity.Property(e => e.HinhThucTt)
                .HasMaxLength(30)
                .HasColumnName("HinhThucTT");
            entity.Property(e => e.MaHd)
                .HasMaxLength(5)
                .HasColumnName("MaHD");
            entity.Property(e => e.NgayLap).HasColumnType("datetime");
            entity.Property(e => e.TenKh)
                .HasMaxLength(30)
                .HasColumnName("TenKH");
            entity.Property(e => e.TenNv)
                .HasMaxLength(30)
                .HasColumnName("TenNV");
            entity.Property(e => e.TongTien).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.TrangThai).HasMaxLength(30);
        });

        modelBuilder.Entity<VwKhDanang>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VW_KH_DANANG");

            entity.Property(e => e.Dc)
                .HasMaxLength(30)
                .HasColumnName("DC");
            entity.Property(e => e.Dt)
                .HasMaxLength(10)
                .HasColumnName("DT");
            entity.Property(e => e.Email).HasMaxLength(30);
            entity.Property(e => e.MaKh)
                .HasMaxLength(5)
                .HasColumnName("MaKH");
            entity.Property(e => e.TenKh)
                .HasMaxLength(30)
                .HasColumnName("TenKH");
        });

        modelBuilder.Entity<VwPhimSuatchieu>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VW_PHIM_SUATCHIEU");

            entity.Property(e => e.GiaVe).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.MaP).HasMaxLength(5);
            entity.Property(e => e.MaSc)
                .HasMaxLength(5)
                .HasColumnName("MaSC");
            entity.Property(e => e.TenP).HasMaxLength(30);
        });

        modelBuilder.Entity<VwVeTheoKhachhang>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VW_VE_THEO_KHACHHANG");

            entity.Property(e => e.MaKh)
                .HasMaxLength(5)
                .HasColumnName("MaKH");
            entity.Property(e => e.TenKh)
                .HasMaxLength(30)
                .HasColumnName("TenKH");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
