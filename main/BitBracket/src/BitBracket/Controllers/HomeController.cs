using System.Diagnostics;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using BitBracket.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using BitBracket.DAL.Abstract;
using BitBracket.ViewModels;

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
    public IActionResult AnnouncementCreate()
    {
        return View();
    }
    
    public async Task<IActionResult> Profile()
    { 
        // info from controller: non database info
        string name = User?.Identity?.Name ?? "No user found";
        // info from identity
        string id = _userManager.GetUserId(User);
        BitUser bitUser = _bitUserRepository.GetBitUserByEntityId(id);
        
        IdentityUser user = await _userManager.GetUserAsync(User);
        if (bitUser.ProfilePicture == null)
        {
            _bitUserRepository.UpdateBitUserProfilePictureIfNull(bitUser);
        }
        UserViewModel userViewModel = new UserViewModel
        {
            Tag = bitUser?.Tag ?? "No tag found",
            Username = name,
            Email = user?.Email ?? "no email",
            Bio = bitUser?.Bio ?? "No current bio.",
            ProfilePictureUrl = Convert.ToBase64String(bitUser.ProfilePicture)
        };

        return View(userViewModel);
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
