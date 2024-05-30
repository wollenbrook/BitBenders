using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BitBracket.Data;
using BitBracket.Models;
using BitBracket.DAL.Abstract;
using BitBracket.DAL.Concrete;
using MyApplication.Data;
using Microsoft.Extensions.DependencyInjection;
using SignalRChat.Hubs; 
using System.Text.Json.Serialization;
//using System.Net.Http.Headers;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSignalR();
var connectionString1 = builder.Configuration.GetConnectionString("AuthenticationConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<BitBracket.Data.ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString1));

var connectionString = builder.Configuration.GetConnectionString("BitBracketConnection");
builder.Services.AddDbContext<BitBracket.Models.BitBracketDbContext>(options => options
                    .UseLazyLoadingProxies()
                    .UseSqlServer(connectionString));
builder.Services.AddScoped<IAnnouncementRepository, AnnouncementRepository>();
builder.Services.AddScoped<IUserAnnouncementRepository, UserAnnouncementRepository>();
builder.Services.AddScoped<IBitUserRepository, BitUserRepository>();
builder.Services.AddScoped<ITournamentRepository, TournamentRepository>();
builder.Services.AddScoped<IBracketRepository, BracketRepository>();
builder.Services.AddScoped<IGuidBracketRepository, GuidBracketRepository>();
builder.Services.AddScoped<IFriendRequestRepository, FriendRequestRepository>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IWhisperService, WhisperService>();
builder.Services.AddScoped<IStandingRepository, StandingRepository>();
builder.Services.AddScoped<IBlockedUsersRepository, BlockedUsersRepository>();
// Register IWhisperService with HttpClientFactory
builder.Services.AddHttpClient();
builder.Services.AddScoped<IWhisperService, WhisperService>();



// Register the Whisper service
builder.Services.AddScoped<IWhisperService, WhisperService>();

// Register EmailService
var sendGridKey = builder.Configuration["SendGridKey"];
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

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});


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
app.MapHub<ChatHub>("/chatHub");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "SearchProfiles",
    pattern: "/SearchProfiles/{bitUserId}",
    defaults: new { controller = "Home", action = "SearchProfile" });
app.MapControllerRoute(
    name: "Tournaments",
    pattern: "/Tournaments/{tournamentId}",
    defaults: new { controller = "Home", action = "Tournaments" });
app.MapRazorPages();

app.Run();
