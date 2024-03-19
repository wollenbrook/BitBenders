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
    private readonly IAnnouncementRepository _announcementRepository;

    public HomeController(ILogger<HomeController> logger, UserManager<IdentityUser> userManager, IBitUserRepository bitUserRepository, IAnnouncementRepository announcementRepository)
    {
        _logger = logger;
        _userManager = userManager;
        _bitUserRepository = bitUserRepository;
        _announcementRepository = announcementRepository;
    }

    public async Task<IActionResult> Index()
    {
        var latestAnnouncement = await _announcementRepository.GetLatestAnnouncementAsync();
        return View(latestAnnouncement);
    }
    public IActionResult Announcement()
    {
        return View();
    }
    public IActionResult AnnouncementCreate()
    {
        return View();
    }
    public IActionResult TestInput()
    {
        var viewModel = new SpeechToTextViewModel();
        // Populate viewModel.SupportedLanguages as necessary
        return View(viewModel);
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
            ProfilePictureUrl = bitUser?.ProfilePicture != null ? "data:image/png;base64," + Convert.ToBase64String(bitUser.ProfilePicture) : "https://bitbracketimagestorage.blob.core.windows.net/images/Blank_Profile.png"

        };

        return View(userViewModel);
    }
    public IActionResult Search()
    {
        return View();
    } 
    public async Task<IActionResult> SearchProfiles(string username)
    {
        //BitUser bitUser = _bitUserRepository.GetBitUserByName(name);
        BitUser bitUser = _bitUserRepository.GetBitUserByRegularId(id);
        BitUser bitUser1 = _bitUserRepository.GetBitUserByName(bitUser.Username);
        var user = await _userManager.FindByIdAsync(bitUser1.AspnetIdentityId);
        var userEmail = _userManager.GetEmailAsync(user);
        if (bitUser == null)
        {
            UserViewModel userViewModelfail = new UserViewModel
            {
                Username = "Not found"
            };
            return View(userViewModelfail);
        }*/
        //var user = await _userManager.FindByIdAsync(bitUser.AspnetIdentityId);
        //var userEmail = _userManager.GetEmailAsync(user);

       
        /*UserViewModel userViewModel = new UserViewModel
        {
            Username = bitUser.Username,
            Email = userEmail.Result,
            Bio = bitUser.Bio,
            Tag = bitUser.Tag,
            ProfilePictureUrl = bitUser.ProfilePicture != null ? "data:image/png;base64," + Convert.ToBase64String(bitUser.ProfilePicture) : "https://bitbracketimagestorage.blob.core.windows.net/images/Blank_Profile.png"
        };*/
        UserViewModel userViewModel = new UserViewModel
        {
            Username = username
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