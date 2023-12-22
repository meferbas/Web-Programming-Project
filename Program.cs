using System.Security.Cryptography.X509Certificates;
using AirlineSeatReservationSystem.Data.Concrete.Efcore;
using AirlineSeatReservationSystem.Data.Abstract;
using AirlineSeatReservationSystem.Data.Concrete;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<DataContext>(options =>
{


    options.UseNpgsql(builder.Configuration["ConnectionStrings:database"]);
}
);

builder.Services.AddScoped<IUserRepository, EfUserRepository>(); // yeni geldi
builder.Services.AddScoped<ISeatRepository, EfSeatRepository>();
builder.Services.AddScoped<IBookingRepository, EfBookingRepository>();


// builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options=>{
//     options.LoginPath="/Users/SignUp";
// });
builder.Services.AddScoped<IFlightRepository, EfFlightRepository>();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
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

app.UseRouting(); //yeni eklendi
app.UseAuthentication();
app.UseAuthorization();


app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Flight}/{action=Index}/{id?}");

app.Run();
