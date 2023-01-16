using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Ciomag_Andreea_Museum.Data;
using Ciomag_Andreea_Museum.Hubs;
using Microsoft.AspNetCore.Identity;
using Ciomag_Andreea_Museum.Areas.Identity.Data;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<MuseumContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("MuseumConnection")));
builder.Services.AddDbContext<IdentityContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("MuseumConnection")));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<IdentityContext>();

builder.Services.AddSignalR();
builder.Services.AddRazorPages();
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
    options.Lockout.MaxFailedAccessAttempts = 3;
    options.Lockout.AllowedForNewUsers = true;
    
    options.Password.RequiredLength = 8;
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequiredUniqueChars = 5;

    options.SignIn.RequireConfirmedEmail = false;

    options.User.RequireUniqueEmail = true;
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvxyzABCDEFGHIJKLMNOPQRSTUVXYZ1234567890._@";
});
builder.Services.AddAuthorization(opts => { opts.AddPolicy("MarketingManager", policy => { policy.RequireRole("Manager");
    policy.RequireClaim("Department", "Marketing"); }); });
builder.Services.AddAuthorization(opts => { opts.AddPolicy("PurchasingEmployee", policy => { policy.RequireRole("Employee");
        policy.RequireClaim("Department", "Purchasing"); }); });
builder.Services.ConfigureApplicationCookie(opts =>
{
    opts.AccessDeniedPath = "/Identity/Account/AccessDenied";
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
app.UseAuthentication();;

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHub<ChatHub>("/Chat");
app.MapRazorPages();

app.Run();

