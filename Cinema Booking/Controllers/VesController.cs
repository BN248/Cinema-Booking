using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cinema_Booking.Models;
using QRCoder;
using Microsoft.AspNetCore.Mvc.Rendering;
using Cinema_Booking.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;

namespace Cinema_Booking.Controllers
{
    public class VesController : Controller
    {
        private readonly QlrcpContext _context;
        private readonly IHubContext<SeatHub> _hubContext;
        public VesController(
     QlrcpContext context,
     IHubContext<SeatHub> hubContext)
        {
            _context = context;

            _hubContext = hubContext;
        }
        public override void OnActionExecuting(
    ActionExecutingContext context)
        {
            var username =
                HttpContext.Session.GetString(
                    "Username");

            if (string.IsNullOrEmpty(username))
            {
                context.Result =
                    RedirectToAction(
                        "Login",
                        "Account");
            }

            base.OnActionExecuting(context);
        }
        public async Task<IActionResult> Index()
        {
            await HuyVeQuaHan();
            var role =
        HttpContext.Session.GetString("Role");

            var maTK =
                HttpContext.Session.GetInt32("MaTK");

            IQueryable<Ve> query =
                _context.Ves
                .Include(x => x.MaScNavigation)
                    .ThenInclude(sc => sc.MaPNavigation)
                .Include(x => x.MaGNavigation)
                .Include(x => x.MaHdNavigation)
                    .ThenInclude(hd => hd.Thanhtoan);

            if (role == "User")
            {
                query =
                    query.Where(x => x.MaTK == maTK);
            }

            var dsVe =
                await query.ToListAsync();

            return View(dsVe);
        }

        public IActionResult Create()
        {
            ViewBag.MaSc = new SelectList(
                _context.Suatchieus,
                "MaSc",
                "MaSc"
            );

            ViewBag.MaG = new SelectList(
                _context.Ghes,
                "MaG",
                "MaG"
            );

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Ve ve)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ve);

                await _context.SaveChangesAsync();

                TempData["Success"] = "Thêm vé thành công";

                return RedirectToAction(nameof(Index));
            }

            ViewBag.MaSc = new SelectList(
                _context.Suatchieus,
                "MaSc",
                "MaSc",
                ve.MaSc
            );

            ViewBag.MaG = new SelectList(
                _context.Ghes,
                "MaG",
                "MaG",
                ve.MaG
            );

            return View(ve);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();

            var ve = await _context.Ves.FindAsync(id);

            if (ve == null) return NotFound();

            ViewBag.MaSc = new SelectList(
                _context.Suatchieus,
                "MaSc",
                "MaSc",
                ve.MaSc
            );

            ViewBag.MaG = new SelectList(
                _context.Ghes,
                "MaG",
                "MaG",
                ve.MaG
            );

            return View(ve);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Ve ve)
        {
            if (id != ve.MaV)
                return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(ve);

                await _context.SaveChangesAsync();

                TempData["Success"] = "Cập nhật vé thành công";

                return RedirectToAction(nameof(Index));
            }

            return View(ve);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null) return NotFound();

            var ve = await _context.Ves
                .Include(x => x.MaScNavigation)
                .FirstOrDefaultAsync(x => x.MaV == id);

            if (ve == null) return NotFound();

            return View(ve);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var ve = await _context.Ves.FindAsync(id);

            if (ve != null)
            {
                _context.Ves.Remove(ve);

                await _context.SaveChangesAsync();

                TempData["Success"] = "Xóa vé thành công";
            }

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Details(string id)
        {
            if (id == null) return NotFound();

            var ve = await _context.Ves
                .Include(x => x.MaScNavigation)
                    .ThenInclude(sc => sc.MaPNavigation)
                .Include(x => x.MaScNavigation)
                    .ThenInclude(sc => sc.MaPhNavigation)
                .Include(x => x.MaGNavigation)
                .FirstOrDefaultAsync(x => x.MaV == id);

            if (ve == null) return NotFound();

            // Dữ liệu QR
            string qrText =
                $"Mã vé: {ve.MaV}\n" +
                $"Phim: {ve.MaScNavigation?.MaPNavigation?.TenP}\n" +
                $"Ghế: {ve.MaGNavigation?.HangG}{ve.MaGNavigation?.SoG}\n" +
                $"Ngày chiếu: {ve.MaScNavigation?.NgayChieu:dd/MM/yyyy}\n" +
                $"Giờ: {ve.MaScNavigation?.GioChieu:HH\\:mm}";

            // Tạo QR
            QRCodeGenerator qrGenerator = new QRCodeGenerator();

            QRCodeData qrCodeData =
                qrGenerator.CreateQrCode(qrText, QRCodeGenerator.ECCLevel.Q);

            PngByteQRCode qrCode = new PngByteQRCode(qrCodeData);

            byte[] qrCodeImage = qrCode.GetGraphic(20);

            ViewBag.QRCode =
                "data:image/png;base64," +
                Convert.ToBase64String(qrCodeImage);

            return View(ve);
        }
        public async Task<IActionResult> DatGhe(string id)
        {
            await HuyVeQuaHan();
            var suat = await _context.Suatchieus
                .Include(x => x.MaPNavigation)
                .Include(x => x.MaPhNavigation)
                .FirstOrDefaultAsync(x => x.MaSc == id);

            if (suat == null)
                return NotFound();

            var ghe = await _context.Ghes
                .Where(x => x.MaPh == suat.MaPh)
                .OrderBy(x => x.HangG)
                .ThenBy(x => x.SoG)
                .ToListAsync();

            var now = DateTime.Now;

            var gheDaDat = await _context.Ves
                .Where(x =>
                    x.MaSc == id &&
                    x.MaG != null &&
                    (
                        x.TrangThai == "Đã thanh toán"
                        ||
                        (
                            x.TrangThai == "Chưa thanh toán"
                            && x.HanThanhToan > now
                        )
                    )
                )
                .Select(x => x.MaG.Value)
                .ToListAsync();

            ViewBag.GheDaDat = gheDaDat;
            ViewBag.MaSc = id;
            ViewBag.Suat = suat;

            return View(ghe);
        }
        public async Task<IActionResult> LichSu()
        {
            await HuyVeQuaHan();

            int? maTk = HttpContext.Session.GetInt32("MaTK");

            var dsVe = await _context.Ves
                .Where(x => x.MaTK == maTk)
                .Include(x => x.MaScNavigation)
                .ThenInclude(x => x.MaPNavigation)
                .ToListAsync();

            return View(dsVe);
        }
        [HttpPost]
        public async Task<IActionResult> DatVe(string maSc, List<int> dsGhe)
        {
            if (dsGhe == null || dsGhe.Count == 0)
            {
                TempData["Loi"] = "Vui lòng chọn ghế";
                return RedirectToAction("DatGhe", new { id = maSc });
            }

            var suat = await _context.Suatchieus
                .FirstOrDefaultAsync(x => x.MaSc == maSc);

            if (suat == null)
                return NotFound();

            var hoaDon = new Hoadon
            {
                MaHd = Guid.NewGuid()
        .ToString("N")
        .Substring(0, 5)
        .ToUpper(),

                MaKh = "KH001",

                NgayLap = DateTime.Now,

                TongTien = suat.GiaVe * dsGhe.Count,

                DaXacNhanThanhToan = false
            };

            _context.Hoadons.Add(hoaDon);

            string maVeCuoi = "";

            foreach (var maGhe in dsGhe)
            {
                bool exists = await _context.Ves.AnyAsync(x =>
                    x.MaSc == maSc && x.MaG == maGhe
                );

                if (exists)
                {
                    TempData["Loi"] = "Ghế đã được đặt";
                    return RedirectToAction("DatGhe", new { id = maSc });
                }
                var maTk = HttpContext.Session.GetInt32("MaTK");

                if (maTk == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                var ve = new Ve
                {
                    MaV = Guid.NewGuid()
        .ToString("N")
        .Substring(0, 10)
        .ToUpper(),

                    MaHd = hoaDon.MaHd,
                    MaSc = maSc,
                    MaG = maGhe,
                    GiaVe = suat.GiaVe,

                    NgayDat = DateOnly.FromDateTime(DateTime.Now),
                    MaTK = maTk.Value,
                    TrangThai = "Chưa thanh toán",

                    HanThanhToan = DateTime.Now.AddMinutes(5)
                };

                _context.Ves.Add(ve);

                maVeCuoi = ve.MaV;
            }

            await _context.SaveChangesAsync();

            foreach (var maGhe in dsGhe)
            {
                await _hubContext.Clients.All.SendAsync(
                    "GheDaDat",
                    maSc,
                    maGhe
                );
            }

            TempData["Info"] = "Đã giữ ghế trong 5 phút. Vui lòng hoàn tất thanh toán.";

            return RedirectToAction(
    "ChonDoAn",
    "Ves",
    new { id = hoaDon.MaHd }
);
        }

        public async Task<IActionResult> ChonDoAn(string id)
        {
            ViewBag.MaHd = id;

            var dsSP = await _context.Sanphams.ToListAsync();

            return View(dsSP);
        }

        [HttpPost]
        public async Task<IActionResult> LuuDoAn(
    string maHD,
    IFormCollection form)
        {
            var dsSP = await _context.Sanphams.ToListAsync();

            foreach (var sp in dsSP)
            {
                string key = $"soluong_{sp.MaSp}";

                int sl = 0;

                int.TryParse(form[key], out sl);

                if (sl > 0)
                {
                    _context.Cthds.Add(
                        new Cthd
                        {
                            MaHd = maHD,
                            MaSp = sp.MaSp,
                            SoLuong = sl,
                            DonGia = sp.DonGia
                        });
                }
            }

            await _context.SaveChangesAsync();

            var hd = await _context.Hoadons
    .FirstOrDefaultAsync(x => x.MaHd == maHD);

            decimal tienDoAn = await _context.Cthds
                .Where(x => x.MaHd == maHD)
                .SumAsync(x =>
                    (x.SoLuong ?? 0) * (x.DonGia ?? 0));

            hd.TongTien = (hd.TongTien ?? 0) + tienDoAn;

            await _context.SaveChangesAsync();

            return RedirectToAction(
                "ThanhToan",
                new { id = maHD });
        }
        public async Task<IActionResult> ThanhToan(string id)
        {
            await HuyVeQuaHan();
            var hd = await _context.Hoadons
                .Include(x => x.Ves)
                .Include(x => x.Cthds)
                .FirstOrDefaultAsync(x => x.MaHd == id);

            if (hd == null)
                return NotFound();

            decimal tienVe = hd.Ves.Sum(x => x.GiaVe ?? 0);

            decimal tienDoAn = hd.Cthds.Sum(x =>
                (x.SoLuong ?? 0) * (x.DonGia ?? 0));

            ViewBag.TienVe = tienVe;
            ViewBag.TienDoAn = tienDoAn;
            ViewBag.TongThanhToan = tienVe + tienDoAn;

            return View(hd);
        }

        [HttpPost]
        public async Task<IActionResult> XacNhanThanhToan(
    string id,
    string phuongThuc)
        {
            var hd = await _context.Hoadons
                .Include(x => x.Ves)
                .Include(x => x.MaKhNavigation)
                .FirstOrDefaultAsync(x => x.MaHd == id);

            if (hd == null)
            {
                return NotFound();
            }

            return RedirectToAction(
                "XuLyThanhToan",
                new
                {
                    id = hd.MaHd,
                    phuongThuc = phuongThuc
                });
        }
        public async Task<IActionResult> XuLyThanhToan(
    string id,
    string phuongThuc)
        {
            var hd = await _context.Hoadons
                .FirstOrDefaultAsync(x => x.MaHd == id);

            if (hd == null)
                return NotFound();

            ViewBag.PhuongThuc = phuongThuc;

            ViewBag.QRCode =
    Url.Action(
        "TaoQR",
        "Ves",
        new { id = hd.MaHd }
    );
            return View(hd);
        }

        [HttpPost]
        public async Task<IActionResult> XacNhanDaNhanTien(string id)
        {
            var hd = await _context.Hoadons
                .FirstOrDefaultAsync(x => x.MaHd == id);

            if (hd == null)
                return NotFound();

            hd.DaXacNhanThanhToan = true;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> ThanhToanThanhCong(string id)
        {
            var hd = await _context.Hoadons
                .Include(x => x.Ves)
                .Include(x => x.MaKhNavigation)
                .FirstOrDefaultAsync(x => x.MaHd == id);

            if (hd == null)
                return NotFound();

            if (!hd.DaXacNhanThanhToan.GetValueOrDefault())
            {
                TempData["Error"] =
                    "Hệ thống chưa xác nhận thanh toán.";

                return RedirectToAction(
                    "XuLyThanhToan",
                    new { id = hd.MaHd });
            }

            if (hd.Ves.Any(x =>
    x.TrangThai == "Chưa thanh toán" &&
    x.HanThanhToan < DateTime.Now))
            {
                TempData["Error"] =
                    "Phiên thanh toán đã hết hạn.";

                return RedirectToAction("Index");
            }

            foreach (var ve in hd.Ves)
            {
                ve.TrangThai = "Đã thanh toán";
                ve.HanThanhToan = null;
            }
            var tt = new Thanhtoan
            {
                MaTt = Guid.NewGuid()
        .ToString("N")
        .Substring(0, 5)
        .ToUpper(),

                MaHd = hd.MaHd,

                PhuongThuc = "MoMo",

                SoTien = hd.TongTien ?? 0,

                ThoiGianTt = DateTime.Now
            };

            _context.Thanhtoans.Add(tt);
            await _context.SaveChangesAsync();

            foreach (var ve in hd.Ves)
            {
                await _hubContext.Clients.All.SendAsync(
                    "GheDaDat",
                    ve.MaSc,
                    ve.MaG
                );
            }

            TaoMailGiaLap(hd);

            TempData["Success"] =
                "Thanh toán thành công. Vé đã gửi về email.";

            return RedirectToAction("Index");
        }

        public async Task HuyVeQuaHan()
        {
            var now = DateTime.Now;

            var dsVe = await _context.Ves
                .Where(x =>
                    x.TrangThai == "Chưa thanh toán"
                    && x.HanThanhToan < now)
                .ToListAsync();

            if (dsVe.Any())
            {
                foreach (var ve in dsVe)
                {
                    var hd = await _context.Hoadons
                        .Include(x => x.Cthds)
                        .Include(x => x.Thanhtoan)
                        .FirstOrDefaultAsync(x => x.MaHd == ve.MaHd);

                    if (hd != null)
                    {
                        if (hd.Thanhtoan != null)
                        {
                            _context.Thanhtoans.Remove(hd.Thanhtoan);
                        }

                        _context.Cthds.RemoveRange(hd.Cthds);

                        _context.Hoadons.Remove(hd);
                    }
                }

                _context.Ves.RemoveRange(dsVe);

                await _context.SaveChangesAsync();
            }
        }

        [HttpPost]
        public async Task<IActionResult> XacNhanGiaLap(string id)
        {
            var hd = await _context.Hoadons
                .FirstOrDefaultAsync(x => x.MaHd == id);

            if (hd == null)
                return NotFound();

            hd.DaXacNhanThanhToan = true;

            await _context.SaveChangesAsync();

            return Ok();
        }
        public IActionResult TaoQR(string id)
        {
            QRCodeGenerator qrGenerator =
                new QRCodeGenerator();

            QRCodeData qrCodeData =
                qrGenerator.CreateQrCode(
                    $"THANHTOAN-{id}",
                    QRCodeGenerator.ECCLevel.Q);

            PngByteQRCode qrCode =
                new PngByteQRCode(qrCodeData);

            byte[] bytes =
                qrCode.GetGraphic(20);

            return File(bytes, "image/png");
        }
        private void TaoMailGiaLap(Hoadon hd)
        {
            string folder = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "mail");

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            string filePath = Path.Combine(
                folder,
                $"{hd.MaHd}.txt");

            string content = $@"
===== VÉ XEM PHIM =====

Mã hóa đơn: {hd.MaHd}

Tổng tiền: {hd.TongTien:N0} VNĐ

Thời gian gửi:
{DateTime.Now:dd/MM/yyyy HH:mm:ss}

Trạng thái:
Đã thanh toán thành công.
";

            System.IO.File.WriteAllText(
                filePath,
                content);
        }
    }
}