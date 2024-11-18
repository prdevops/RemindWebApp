using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RemindWebApp.DAL;
using RemindWebApp.Models;
using System;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.Lax;
});

builder.Services.AddIdentity<NewUser, IdentityRole>(identityOptions =>
{
    identityOptions.Password.RequireDigit = true;
    identityOptions.Password.RequiredLength = 8;
    identityOptions.Password.RequireLowercase = true;
    identityOptions.Password.RequireUppercase = true;
    identityOptions.Password.RequireNonAlphanumeric = true;

    identityOptions.User.RequireUniqueEmail = true;
    identityOptions.Lockout.MaxFailedAccessAttempts = 3;
    identityOptions.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(90);
    identityOptions.Lockout.AllowedForNewUsers = true;

    identityOptions.SignIn.RequireConfirmedEmail = true;
})
.AddEntityFrameworkStores<RemindDatabase>()
.AddDefaultTokenProviders();

builder.Services.AddDbContext<RemindDatabase>(options =>
{
    options.UseSqlServer(builder.Configuration["ConnectionString:DefaultConnection"]);
});

builder.Services.AddControllersWithViews(options =>
{
    options.EnableEndpointRouting = true;
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCookiePolicy();
app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
