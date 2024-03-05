//Program.cs

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BitBracket.Data;
using BitBracket.Models;
using BitBracket.DAL.Abstract;
using BitBracket.DAL.Concrete;
using MyApplication.Data;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString1 = builder.Configuration.GetConnectionString("AuthenticationConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<BitBracket.Data.ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString1));

var connectionString = builder.Configuration.GetConnectionString("BitBracketConnection");
builder.Services.AddDbContext<BitBracket.Models.BitBracketDbContext>(options => options
                    .UseLazyLoadingProxies()
                    .UseSqlServer(connectionString));
builder.Services.AddScoped<IAnnouncementRepository, AnnouncementRepository>();
builder.Services.AddScoped<IBitUserRepository, BitUserRepository>();
// Register EmailService
var sendGridKey = builder.Configuration["SendGridKey"]; // Ensure you have this key in your appsettings.json
builder.Services.AddScoped<IEmailService, EmailService>(_ => new EmailService(sendGridKey));

// Register SmsService

var accountsid = builder.Configuration["AccountSid"];
var authToken = builder.Configuration["AuthToken"];
var fromNumber = builder.Configuration["FromNumber"];
builder.Services.AddSingleton<ISmsService>(new SmsService(
        accountsid,
        authToken,
        fromNumber
));

builder.Services.AddControllers();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<BitBracket.Data.ApplicationDbContext>();
builder.Services.AddControllersWithViews();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
