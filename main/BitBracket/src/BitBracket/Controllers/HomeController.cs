using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BitBracket.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace BitBracket.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly UserManager<IdentityUser> _userManager;

    public HomeController(ILogger<HomeController> logger, UserManager<IdentityUser> userManager)
    {
        _logger = logger;
        _userManager = userManager;
    }

    public IActionResult Index()
    {

        return View();
    }
    public IActionResult Announcement()
    {
        return View();
    }
    
    public async Task<IActionResult> Profile()
    { 
        // info from controller: non database info
        bool isAuthenticated = User.Identity.IsAuthenticated;
        string name = User.Identity.Name;
        string AuthType = User.Identity.AuthenticationType;

        // info from identity
        string id = _userManager.GetUserId(User);

        IdentityUser user = await _userManager.GetUserAsync(User);
        string email = user?.Email ?? "no email";
        string phone = user?.PhoneNumber ?? "no phone number";
        ViewBag.Message = $"user {name} is authenticated: {isAuthenticated} using {AuthType}, ID is {id} email is: {email}";
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
