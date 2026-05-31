using Cinema_Booking.Models;
using Microsoft.EntityFrameworkCore;
using Cinema_Booking.Hubs;

var builder = WebApplication.CreateBuilder(args);

// =======================
// SERVICES
// =======================
builder.Services.AddControllersWithViews();

// DB Context
builder.Services.AddDbContext<QlrcpContext>(options =>
    options.UseSqlServer("Server=THANHNGUYEN;Database=QLRCP;Trusted_Connection=True;TrustServerCertificate=True"));

// Session
builder.Services.AddSession();
builder.Services.AddSignalR();

var app = builder.Build();

// =======================
// MIDDLEWARE
// =======================
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// ⚠️ Session phải nằm sau UseRouting
app.UseSession();

app.UseAuthorization();

// ROUTE
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapHub<SeatHub>("/seatHub");

app.Run();