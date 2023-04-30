using DogalEmlak.Web.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation(); ;
builder.Services.AddDbContext<DatabaseContext>(opts =>
 {
     opts.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
     //opts.UseLazyLoadingProxies();
 });

//cookie yetkilendirme
builder.Services
	.AddAuthentication("Cookies")
	.AddCookie(opt =>
	{
		opt.Cookie.Name = ".DogalEmlak.Web.auth";
		opt.ExpireTimeSpan = TimeSpan.FromDays(1);
		opt.SlidingExpiration = false;
		opt.LoginPath = "/personel/giris";
		opt.LogoutPath = "/personel/cikis";
		opt.AccessDeniedPath = "/Home/AccessDenied";
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

//yetkilendirme aktif olmasý için...
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
