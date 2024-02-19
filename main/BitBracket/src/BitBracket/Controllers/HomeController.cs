using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BitBracket.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using BitBracket.DAL.Abstract;

namespace BitBracket.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IBitUserRepository _bitUserRepository;

    public HomeController(ILogger<HomeController> logger, UserManager<IdentityUser> userManager, IBitUserRepository bitUserRepository)
    {
        _logger = logger;
        _userManager = userManager;
        _bitUserRepository = bitUserRepository;
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
        BitUser bitUser = _bitUserRepository.GetBitUserByEntityId(id);
        string tag = bitUser.Tag;

        IdentityUser user = await _userManager.GetUserAsync(User);
        string email = user?.Email ?? "no email";
        string phone = user?.PhoneNumber ?? "no phone number";
        ViewBag.Tag = tag;
        ViewBag.Name = name;
        ViewBag.Email = email;
        ViewBag.Phone = phone;

        
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
