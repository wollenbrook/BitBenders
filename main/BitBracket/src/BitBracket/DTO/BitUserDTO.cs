using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using BitBracket.Models;
namespace BitBracket.DTO
{
    public class BitUserDTO
    {
        public int Id { get; set; }

        public string Username { get; set; } = null!;

        public string Tag { get; set; } = null!;

        public string Bio { get; set; } = "bio not set";

        public byte[]? ProfilePicture { get; set; } = null;


    }
}
namespace BitBracket.ExtensionMethods
{
    public static class BitUserExtensions
    {
        public static BitBracket.DTO.BitUserDTO ReturnBitUserSearchDTO(this BitUser bitUser)
        {


            return new BitBracket.DTO.BitUserDTO
            {
                Id = bitUser.Id,
                Username = bitUser.Username,
                Tag = bitUser.Tag,
                Bio = bitUser.Bio,
                ProfilePicture = bitUser.ProfilePicture
            };

        }
    }
}