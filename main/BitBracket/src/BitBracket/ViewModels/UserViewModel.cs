using Microsoft.AspNet.Identity;

namespace BitBracket.ViewModels
{
    public class UserViewModel
    {
        public string Username { get; set; } = null!;
        public string Bio { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Tag { get; set; } = null!;
        public string? ProfilePictureUrl { get; set; } = null;
        public bool Friends { get; set; } = false;
        public bool FriendRequestSent { get; set; } = false;
        public int? ProfileID { get; set; } = null;
        public int? PersonID { get; set; }

    }
}
