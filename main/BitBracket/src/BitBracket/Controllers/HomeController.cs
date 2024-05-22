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
    private readonly ITournamentRepository _tournamentRepository;
    private readonly IUserAnnouncementRepository _announcementRepo;

    public HomeController(IUserAnnouncementRepository announcementRepo, ITournamentRepository tournamentRepository, ILogger<HomeController> logger, UserManager<IdentityUser> userManager, IBitUserRepository bitUserRepository, IAnnouncementRepository announcementRepository)
    {
        _logger = logger;
        _userManager = userManager;
        _bitUserRepository = bitUserRepository;
        _announcementRepository = announcementRepository;
        _tournamentRepository = tournamentRepository;
        _announcementRepo = announcementRepo;
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
    public IActionResult UserAnnouncementForm()
    {
        return View();
    }
    public IActionResult UserAnnouncementCreate()
    {
        return View();
    }
    public async Task<IActionResult> ManageAnnouncement()
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null)
        {
            return Unauthorized("User must be logged in.");
        }

        var drafts = await _announcementRepo.GetByUserIdAndStatus(userId, true);
        var published = await _announcementRepo.GetByUserIdAndStatus(userId, false);

        var viewModel = new ManageAnnouncementsViewModel
        {
            DraftAnnouncements = drafts,
            PublishedAnnouncements = published
        };

        return View(viewModel);
    }

    // [Authorize]  // Ensures that only logged-in users can access this action
    // public async Task<IActionResult> TournamentImIn()
    // {
    //     var userId = _userManager.GetUserId(User);  // Get the current logged-in user's ID

    //     if (string.IsNullOrEmpty(userId))
    //     {
    //         return Redirect("/Login");  // Redirect to login page if user is not found (or not logged in)
    //     }

    //     try
    //     {
    //         // Assuming the method GetParticipatesByUserId exists and fetches all the tournaments a user is participating in
    //         var tournaments = await _tournamentRepository.GetTournamentsByUserId(userId);
            
    //         // We could use a ViewModel to pass data to the view if needed, or pass the model directly
    //         return View(tournaments);
    //     }
    //     catch
    //     {
    //         // Handle any exceptions or errors that might occur
    //         return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    //     }
    // }
    public async Task<IActionResult> TournamentImIn()
    {
        var userId = _userManager.GetUserId(User); // Get the current user's ID from UserManager
        if (string.IsNullOrEmpty(userId))
        {
            return NotFound("User not found");
        }

        var bitUser = _bitUserRepository.GetBitUserByEntityId(userId);
        if (bitUser == null)
        {
            return NotFound("Bit user not found");
        }

        ViewBag.UserId = bitUser.Id; // Pass the user ID to the view through ViewBag

        var tournaments = await _tournamentRepository.GetTournamentsUserParticipatesInAsync(bitUser.Id);
        if (tournaments == null || !tournaments.Any())
        {
            return NotFound(); // You could have a view to display when there are no tournaments
        }

        return View(tournaments); // Pass the list of tournaments to the view
    }
    public IActionResult AllTournaments()
    {
        return View();
    }
    public async Task<IActionResult> TournamentProfile(int id)
    {
        var tournament = await _tournamentRepository.Get(id);
        if (tournament == null)
        {
            return NotFound();
        }

        // Fetch the user ID using UserManager
        var userId = _userManager.GetUserId(User);
        if (string.IsNullOrEmpty(userId))
        {
            return NotFound("User not found");
        }

        // Asynchronously get the BitUser associated with the userID
        var bitUser = _bitUserRepository.GetBitUserByEntityId(userId);
        if (bitUser == null)
        {
            return NotFound("Bit user not found");
        }

        // Use ViewBag to pass data to the view
        ViewBag.UserId = bitUser.Id;

        return View(tournament);
    }

    public IActionResult OptInConfirmation()
    {
        return View();
    }
    public IActionResult WhisperTest()
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
            ProfilePictureUrl = bitUser?.ProfilePicture != null ? "data:image/png;base64," + Convert.ToBase64String(bitUser.ProfilePicture) : "https://bitbracketimagestorage.blob.core.windows.net/images/Blank_Profile.png"

        };

        return View(userViewModel);
    }
    public IActionResult Search()
    {
        return View();
    } 
    public async Task<IActionResult> Tournaments(int id)
    {
        string name = User?.Identity?.Name ?? "Not signed in";

        Tournament tournament = await _tournamentRepository.Get(id);
        BitUser Owner = _bitUserRepository.GetBitUserByRegularId(tournament.Owner);
        IEnumerable<Bracket> bracket = tournament.Brackets;
        TournamentViewModel tournamentView = new TournamentViewModel
        {
            Name = tournament.Name,
            Location = tournament.Location,
            Owner = Owner.Username,
            Status = tournament.Status,
            CurrentUserName = name
        };
        return View(tournamentView);
    }
    public async Task<IActionResult> SearchProfiles(int id)
    { 
        string senderId = _userManager.GetUserId(User);
        BitUser sender = _bitUserRepository.GetBitUserByEntityId(senderId);
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
        }
        else
        {
            UserViewModel userViewModel = new UserViewModel
            {
                Username = bitUser.Username,
                Email = userEmail.Result,
                Bio = bitUser.Bio,
                Tag = bitUser.Tag,
                ProfilePictureUrl = bitUser.ProfilePicture != null ? "data:image/png;base64," + Convert.ToBase64String(bitUser.ProfilePicture) : "https://bitbracketimagestorage.blob.core.windows.net/images/Blank_Profile.png",
                Friends = _bitUserRepository.CheckIfFriends(sender, bitUser),
                FriendRequestSent = _bitUserRepository.CheckIfRequestSent(sender, bitUser),
                ProfileID = bitUser.Id
            };
            return View(userViewModel);
        }
    }
    public IActionResult Test()
    {
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