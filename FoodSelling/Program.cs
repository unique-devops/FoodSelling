using FoodSelling.Models;
using FoodSelling.Service;
using FoodSelling.SignalRHub;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<CartService>();
builder.Services.AddSignalR();
// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("con")));
builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();


builder.Services.AddAuthentication().AddCookie(options => { 
    options.LoginPath ="/Account/Login";
    options.LogoutPath = "/";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("CustomerOnly", policy => policy.RequireRole("Customer"));
});

builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
});
builder.Services.AddControllersWithViews();

builder.Services.AddDistributedMemoryCache();


builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else {
    app.UseDeveloperExceptionPage();
}
using (var scope = app.Services.CreateScope())
{
    var service = scope.ServiceProvider;
    var userManager = service.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager =  service.GetRequiredService<RoleManager<IdentityRole>>();

    await SeedData.Initialize(service, userManager, roleManager);
}


app.UseSession();
app.UseStaticFiles();

app.UseRouting();




app.UseAuthentication();
app.UseAuthorization();

app.MapHub<CartHub>("/cartHub");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
