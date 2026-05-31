using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cinema_Booking.Models;

namespace Cinema_Booking.Controllers
{
    public class HomeController : Controller
    {
        private readonly QlrcpContext _context;

        public HomeController(QlrcpContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var phimHot = await _context.Phims
                .Include(x => x.MaTls)
                .Take(8)
                .ToListAsync();

            return View(phimHot);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}