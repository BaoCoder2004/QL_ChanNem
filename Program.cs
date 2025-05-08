using AspNetCoreHero.ToastNotification;
using BTL_TKWeb.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Text.Encodings.Web;
using System.Text.Unicode;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<QuanLyChanGaGoiDemContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("QuanLyChanGaGoiDemContext")));
builder.Services.AddSingleton<HtmlEncoder>(HtmlEncoder.Create(allowedRanges: new[] { UnicodeRanges.All }));
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
        options =>
        {
            options.AccessDeniedPath = new PathString("/");
        });
//cookie session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".VuaDem.Session";
    options.IdleTimeout = TimeSpan.FromDays(1);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddNotyf(config =>
{
    config.DurationInSeconds
    = 10;
    config.IsDismissable =
    true;
    config.Position =
    NotyfPosition.BottomRight;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
//cookie session
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "dangnhapadmin",
    pattern: "DangNhapAdmin",
    defaults: new {controller="AdminAccount",action= "DangNhapAdmin" }
    );
app.MapControllerRoute(
    name: "dangnhap",
    pattern: "DangNhap",
    defaults: new { controller = "Account", action = "DangNhap" }
    );
app.MapControllerRoute(
    name: "taotaikhoan",
    pattern: "TaoTaiKhoan",
    defaults: new { controller = "Account", action = "TaoTaiKhoan" }
    );
app.Run();
