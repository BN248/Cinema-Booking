using Cinema_Booking.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;

namespace Cinema_Booking.Controllers
{
    public class SanphamsController : Controller
    {
        private readonly QlrcpContext _context;
        private readonly IWebHostEnvironment _environment;

        public SanphamsController(
            QlrcpContext context,
            IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public override void OnActionExecuting(
    ActionExecutingContext context)
        {
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

        // DANH SÁCH SẢN PHẨM
        public async Task<IActionResult> Index()
        {
            return View(await _context.Sanphams.ToListAsync());
        }

        // ======================
        // THÊM SẢN PHẨM
        // ======================

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            Sanpham sp,
            IFormFile? imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null)
                {
                    string fileName =
                        Guid.NewGuid().ToString()
                        + Path.GetExtension(imageFile.FileName);

                    string folder =
                        Path.Combine(
                            _environment.WebRootPath,
                            "images");

                    if (!Directory.Exists(folder))
                    {
                        Directory.CreateDirectory(folder);
                    }

                    string filePath =
                        Path.Combine(folder, fileName);

                    using (var stream =
                           new FileStream(
                               filePath,
                               FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    sp.HinhAnh =
                        "/images/" + fileName;
                }

                _context.Sanphams.Add(sp);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(sp);
        }

        // ======================
        // SỬA SẢN PHẨM
        // ======================

        public async Task<IActionResult> Edit(string id)
        {
            var sp = await _context.Sanphams.FindAsync(id);

            if (sp == null)
                return NotFound();

            return View(sp);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            string id,
            Sanpham sp,
            IFormFile? imageFile)
        {
            if (id != sp.MaSp)
                return NotFound();

            var sanPhamDB =
                await _context.Sanphams.FindAsync(id);

            if (sanPhamDB == null)
                return NotFound();

            sanPhamDB.TenSp = sp.TenSp;
            sanPhamDB.LoaiSp = sp.LoaiSp;
            sanPhamDB.DonGia = sp.DonGia;

            if (imageFile != null)
            {
                string fileName =
                    Guid.NewGuid().ToString()
                    + Path.GetExtension(imageFile.FileName);

                string folder =
                    Path.Combine(
                        _environment.WebRootPath,
                        "images");

                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                string filePath =
                    Path.Combine(folder, fileName);

                using (var stream =
                       new FileStream(
                           filePath,
                           FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                sanPhamDB.HinhAnh =
                    "/images/" + fileName;
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // ======================
        // XÓA SẢN PHẨM
        // ======================

        public async Task<IActionResult> Delete(string id)
        {
            var sp = await _context.Sanphams.FindAsync(id);

            if (sp == null)
                return NotFound();

            return View(sp);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string maSp)
        {
            var sp =
                await _context.Sanphams.FindAsync(maSp);

            if (sp == null)
                return NotFound();

            _context.Sanphams.Remove(sp);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}