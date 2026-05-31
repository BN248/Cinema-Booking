using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cinema_Booking.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;

namespace Cinema_Booking.Controllers
{
    public class SuatchieusController : Controller
    {
        private readonly QlrcpContext _context;

        public SuatchieusController(QlrcpContext context)
        {
            _context = context;
        }

        public override void OnActionExecuting(
     ActionExecutingContext context)
        {
            var action =
                context.RouteData.Values["action"]?.ToString();

            if (action == "SuatTheoPhim"
                || action == "Details")
            {
                base.OnActionExecuting(context);
                return;
            }

            var role =
                HttpContext.Session.GetString("Role");

            if (role != "Admin"
                && role != "Staff")
            {
                context.Result =
                    RedirectToAction(
                        "AccessDenied",
                        "Account");
            }

            base.OnActionExecuting(context);
        }

        public async Task<IActionResult> Index()
        {
            var dsSuatChieu = await _context.Suatchieus
    .Include(x => x.MaPNavigation)
    .Include(x => x.MaPhNavigation)
    .ToListAsync();
            return View(dsSuatChieu);
        }

        public async Task<IActionResult> SuatTheoPhim(string maP)
        {
            var dsSuat = await _context.Suatchieus

                .Include(x => x.MaPNavigation)
                .Include(x => x.MaPhNavigation)

                .Where(x => x.MaP == maP)

                .ToListAsync();

            return View(dsSuat);
        }

        public async Task<IActionResult> Details(string id)
        {
            if (id == null) return NotFound();

            var suat = await _context.Suatchieus
    .Include(x => x.MaPNavigation)
    .Include(x => x.MaPhNavigation)
    .FirstOrDefaultAsync(x => x.MaSc == id);

            if (suat == null) return NotFound();

            return View(suat);
        }

        public IActionResult Create()
        {
            ViewBag.MaP = new SelectList(_context.Phims, "MaP", "TenP");
            ViewBag.MaPh = new SelectList(_context.Phongchieus, "MaPh", "TenPh");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Suatchieu suatchieu)
        {
            // kiểm tra giờ kết thúc

            if (suatchieu.GioKetThuc <= suatchieu.GioChieu)
            {
                ModelState.AddModelError("GioKetThuc",
                    "Giờ kết thúc phải lớn hơn giờ chiếu");
            }

            // kiểm tra trùng suất chiếu cùng phòng

            bool trungLich = await _context.Suatchieus.AnyAsync(x =>

                x.MaPh == suatchieu.MaPh
                &&
                x.NgayChieu == suatchieu.NgayChieu
                &&
                (
                    suatchieu.GioChieu < x.GioKetThuc
                    &&
                    suatchieu.GioKetThuc > x.GioChieu
                )
            );

            if (trungLich)
            {
                ModelState.AddModelError("GioChieu",
                    "Phòng này đã có suất chiếu trong khoảng thời gian này");
            }

            if (ModelState.IsValid)
            {
                _context.Add(suatchieu);

                await _context.SaveChangesAsync();

                TempData["Success"] =
                    "Thêm suất chiếu thành công";

                return RedirectToAction(nameof(Index));
            }

            ViewBag.MaP = new SelectList(_context.Phims,
                                         "MaP",
                                         "TenP",
                                         suatchieu.MaP);

            ViewBag.MaPh = new SelectList(_context.Phongchieus,
                                          "MaPh",
                                          "TenPh",
                                          suatchieu.MaPh);

            return View(suatchieu);
        }
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();

            var suat = await _context.Suatchieus.FindAsync(id);

            if (suat == null) return NotFound();

            ViewBag.MaP = new SelectList(
                _context.Phims,
                "MaP",
                "TenP",
                suat.MaP
            );

            ViewBag.MaPh = new SelectList(
                _context.Phongchieus,
                "MaPh",
                "TenPh",
                suat.MaPh
            );

            return View(suat);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Suatchieu suatchieu)
        {
            if (id != suatchieu.MaSc)
                return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(suatchieu);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.MaP = new SelectList(_context.Phims,
                             "MaP",
                             "TenP",
                             suatchieu.MaP);

            ViewBag.MaPh = new SelectList(_context.Phongchieus,
                                          "MaPh",
                                          "TenPh",
                                          suatchieu.MaPh);

            ViewBag.MaP = new SelectList(
    _context.Phims,
    "MaP",
    "TenP",
    suatchieu.MaP
);

            ViewBag.MaPh = new SelectList(
                _context.Phongchieus,
                "MaPh",
                "TenPh",
                suatchieu.MaPh
            );

            return View(suatchieu);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null) return NotFound();

            var suat = await _context.Suatchieus
                .FirstOrDefaultAsync(x => x.MaSc == id);

            if (suat == null) return NotFound();

            return View(suat);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var suat = await _context.Suatchieus.FindAsync(id);

            if (suat == null)
                return NotFound();

            // kiểm tra đã có vé chưa

            bool daCoVe = await _context.Ves
                .AnyAsync(x => x.MaSc == id);

            if (daCoVe)
            {
                TempData["Error"] =
                    "Không thể xóa vì suất chiếu đã có người đặt vé.";

                return RedirectToAction(nameof(Index));
            }

            _context.Suatchieus.Remove(suat);

            await _context.SaveChangesAsync();

            TempData["Success"] =
                "Xóa suất chiếu thành công.";

            return RedirectToAction(nameof(Index));
        }
    }
}