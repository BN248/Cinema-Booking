using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cinema_Booking.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;

namespace Cinema_Booking.Controllers
{
    public class PhimsController : Controller
    {
        private readonly QlrcpContext _context;

        public PhimsController(QlrcpContext context)
        {
            _context = context;
        }

        public override void OnActionExecuting(
    ActionExecutingContext context)
        {
            var action =
                context.RouteData.Values["action"]?.ToString();

            // User được xem danh sách phim
            if (action == "Index"
                || action == "Details"
                || action == "LichChieu")
            {
                base.OnActionExecuting(context);
                return;
            }

            var role =
                HttpContext.Session.GetString("Role");

            if (role != "Admin")
            {
                context.Result =
                    RedirectToAction(
                        "AccessDenied",
                        "Account");
            }

            base.OnActionExecuting(context);
        }
        public async Task<IActionResult> Index(string keyword)
        {
            var dsPhim = _context.Phims
                .Include(p => p.MaTls)
                .AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                dsPhim = dsPhim.Where(x => x.TenP.Contains(keyword));
            }

            return View(await dsPhim.ToListAsync());
        }

        public async Task<IActionResult> LichChieu(string id)
        {
            var suat = await _context.Suatchieus
                .Where(x => x.MaP == id)
                .Include(x => x.MaPNavigation)
                .Include(x => x.MaPhNavigation)
                .ToListAsync();

            return View(suat);
        }
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
                return NotFound();

            var phim = await _context.Phims
                .Include(p => p.CtPhim)
                .Include(p => p.MaTls)
                .FirstOrDefaultAsync(x => x.MaP == id);

            if (phim == null)
                return NotFound();

            return View(phim);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Phim phim)
        {
            if (ModelState.IsValid)
            {
                _context.Add(phim);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(phim);
        }
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();

            var phim = await _context.Phims.FindAsync(id);
            if (phim == null) return NotFound();

            return View(phim);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Phim phim)
        {
            if (id != phim.MaP) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(phim);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(phim);
        }
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null) return NotFound();

            var phim = await _context.Phims
                .FirstOrDefaultAsync(x => x.MaP == id);

            if (phim == null) return NotFound();

            return View(phim);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var phim = await _context.Phims.FindAsync(id);
            if (phim != null)
            {
                _context.Phims.Remove(phim);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}