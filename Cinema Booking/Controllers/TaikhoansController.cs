using Cinema_Booking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cinema_Booking.Controllers
{
    public class TaikhoansController : Controller
    {
        private readonly QlrcpContext _context;

        public TaikhoansController(QlrcpContext context)
        {
            _context = context;
        }

        // LIST
        public IActionResult Index()
        {
            var role = HttpContext.Session.GetString("Role");
            if (role != "Admin")
                return RedirectToAction("Login", "Account");

            return View(_context.Taikhoans.ToList());
        }

        // CREATE
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Taikhoan tk)
        {
            _context.Taikhoans.Add(tk);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // EDIT
        public IActionResult Edit(int id)
        {
            var tk = _context.Taikhoans.Find(id);
            return View(tk);
        }

        [HttpPost]
        public IActionResult Edit(Taikhoan tk)
        {
            _context.Taikhoans.Update(tk);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // DELETE
        public IActionResult Delete(int id)
        {
            var tk = _context.Taikhoans.Find(id);
            return View(tk);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            var tk = _context.Taikhoans.Find(id);
            if (tk != null)
            {
                _context.Taikhoans.Remove(tk);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}