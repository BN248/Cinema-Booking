using Microsoft.AspNetCore.Mvc;
using Cinema_Booking.Models;

namespace Cinema_Booking.Controllers
{
    public class AccountController : Controller
    {
        private readonly QlrcpContext _context;

        public AccountController(QlrcpContext context)
        {
            _context = context;
        }

        // GET LOGIN
        public IActionResult Login()
        {
            return View();
        }

        // POST LOGIN
        // POST LOGIN
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var tk = _context.Taikhoans
                .FirstOrDefault(x =>
                    (x.Username == username || x.Email == username)
                    && x.Pass == password);

            if (tk == null)
            {
                ViewBag.Error = "Sai email hoặc mật khẩu";
                return View();
            }

            // Lưu session
            HttpContext.Session.SetString("Username", tk.Username);
            HttpContext.Session.SetString("Role", tk.VaiTro);
            HttpContext.Session.SetInt32("MaTK", tk.MaTK);

            return RedirectToAction("Index", "Home");
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(
    string username,
    string password,
    string email)
        {
            if (_context.Taikhoans.Any(x =>
                x.Username == username))
            {
                ViewBag.Error =
                    "Tên đăng nhập đã tồn tại";

                return View();
            }

            Taikhoan tk = new Taikhoan
            {
                Username = username,
                Pass = password,
                Email = email,
                VaiTro = "User"
            };

            _context.Taikhoans.Add(tk);

            _context.SaveChanges();

            TempData["Success"] =
                "Đăng ký thành công";

            return RedirectToAction("Login");
        }

        // LOGOUT
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Login");
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}