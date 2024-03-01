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


    }
}
